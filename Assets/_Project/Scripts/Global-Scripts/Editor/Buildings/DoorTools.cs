using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DaftAppleGames.Common.Buildings;
#if MCS
using MeshCombineStudio;
#endif
using Unity.AI.Navigation;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    /// <summary>
    /// Tooling function to manage building doors
    /// </summary>
    public class DoorTools : MonoBehaviour
    {
        /// <summary>
        /// Find and process all door meshes in the root
        /// </summary>
        /// <param name="rootGameObject"></param>
        /// <param name="settings"></param>
        /// <param name="isPrefab"></param>
        public static void ConfigureAllDoors(GameObject rootGameObject, DoorEditorSettings settings, bool isPrefab)
        {
            if (settings.nameSearchStrings == null || settings.nameSearchStrings.Length == 0)
            {
                Debug.LogError("DoorTools: No search strings set for door names. Aborting.");
                return;
            }
            
            // Iterate through root, finding door game objects
            MeshFilter[] allDoors = rootGameObject.GetComponentsInChildren<MeshFilter>(true);
            foreach (MeshFilter door in allDoors)
            {
                if (settings.nameSearchStrings.Any(door.name.Contains))
                {
                    Debug.Log($"DoorTools: Configuring Door {door.gameObject} in {rootGameObject.name} ");
                    ConfigureDoor(door.gameObject, settings, isPrefab);
                }
            }
        }

        /// <summary>
        /// Configure the base Door component
        /// </summary>
        /// <param name="buildingGameObject"></param>
        public static void ConfigureDoor(GameObject buildingGameObject,DoorEditorSettings settings, bool isPrefab)
        {
            Door theDoor = buildingGameObject.GetComponent<Door>();
            if (!theDoor)
            {
                theDoor = buildingGameObject.AddComponent<Door>();
                Debug.Log($"DoorTools: Added new Door component on {buildingGameObject} ");
            }

            // Update audio
            ConfigureAudio(theDoor, settings);

            // Update config
            ConfigureSettings(theDoor, settings);
            
            // Update triggers
            ConfigureTriggers(theDoor, settings);
            
            // Configure NavMesh Modifier
            ConfigureNavMesh(theDoor);
            
            // Configure Static flags
            ConfigureStatic(theDoor);

            
            // Mark prefab as "Dirty" to force save
            if(isPrefab)
            {
                EditorTools.ForcePrefabSave();
            }
        }

        /// <summary>
        /// Configure Static flag settings
        /// </summary>
        /// <param name="door"></param>
        private static void ConfigureStatic(Door door)
        {

            door.gameObject.isStatic = false;

#if MCS
            // Configure MCSDynamic component
            AddMCSDynamicComponent(theDoor);
#endif
        }
        
        private static void ConfigureSettings(Door door, DoorEditorSettings settings)
        {
            door.openDuration = settings.openDuration;
            door.stayOpenDuration = settings.stayOpenDuration;
            door.closeDuration = settings.closeDuration;
            door.openAngle = settings.openAngle;
            door.autoOpen = settings.autoOpen;

            // Set the opening type
            if (door.gameObject.name.Contains("left"))
            {
                door.pivotSide = DoorPivotSide.Left;
            }
            else if (door.gameObject.name.Contains("right"))
            {
                door.pivotSide = DoorPivotSide.Right;
            }
            else
            {
                // Use the provided default
                door.pivotSide = settings.pivotSide;
            }
            Debug.Log($"DoorTools: Configured settings on {door.gameObject.name} ");
        }
        
        /// <summary>
        /// Configure the AudioSource
        /// </summary>
        /// <param name="door"></param>
        private static void ConfigureAudio(Door door, DoorEditorSettings settings)
        {
            AudioSource theAudioSource = door.gameObject.GetComponent<AudioSource>();
            if (!theAudioSource)
            {
                theAudioSource = door.gameObject.AddComponent<AudioSource>();
            }

            // Configure Mixer Group
            theAudioSource.outputAudioMixerGroup = settings.audioMixerGroup;
            theAudioSource.spatialBlend = 1.0f;
            theAudioSource.playOnAwake = false;
            theAudioSource.loop = false;
            
            // Configure clips
            if (settings.doorOpenClip)
            {
                door.openAudioClip = settings.doorOpenClip;
            }

            if (settings.doorClosingClip)
            {
                door.closingAudioClip = settings.doorClosingClip;
            }

            if (settings.doorClosedClip)
            {
                door.closedAudioClip = settings.doorClosedClip;
            }
            Debug.Log($"DoorTools: Configured audio on {door.gameObject.name} ");
        }


        /// <summary>
        /// Configure the opening triggers
        /// </summary>
        /// <param name="door"></param>
        private static void ConfigureTriggers(Door door, DoorEditorSettings settings)
        {
            Transform[] allChildTransforms= door.gameObject.GetComponentsInChildren<Transform>();
            // Inside Trigger
            // Look for inside trigger. Create it, if it's not there
            bool foundInside = false;
            GameObject insideTriggerGameObject = null;
            bool foundOutside = false;
            GameObject outsideTriggerGameObject = null;

            foreach(Transform childTransform in allChildTransforms)
            {
                GameObject childGameObject = childTransform.gameObject;

                if(childGameObject.name.Contains("Inside Trigger"))
                {
                    insideTriggerGameObject = childGameObject;
                    foundInside = true;
                }

                if (childGameObject.name.Contains("Outside Trigger"))
                {
                    outsideTriggerGameObject = childGameObject;
                    foundOutside = true;
                }
            }

            if(!foundInside)
            {
                insideTriggerGameObject = new GameObject();
                insideTriggerGameObject.name = "Inside Trigger";
                insideTriggerGameObject.transform.SetParent(door.gameObject.transform);
                insideTriggerGameObject.transform.localPosition = Vector3.zero;
                insideTriggerGameObject.transform.localRotation = Quaternion.identity;
            }

            if (!foundOutside)
            {
                outsideTriggerGameObject = new GameObject();
                outsideTriggerGameObject.name = "Outside Trigger";
                outsideTriggerGameObject.transform.SetParent(door.gameObject.transform);
                outsideTriggerGameObject.transform.localPosition = Vector3.zero;
                outsideTriggerGameObject.transform.localRotation = Quaternion.identity;
            }
            
            if(settings.swapTriggers)
            {
                ConfigureTrigger(insideTriggerGameObject, settings.colliderxOffset, DoorTriggerLocation.Inside, settings);
                ConfigureTrigger(outsideTriggerGameObject, -settings.colliderxOffset, DoorTriggerLocation.Outside, settings);
            }
            else
            {
                ConfigureTrigger(insideTriggerGameObject, -settings.colliderxOffset, DoorTriggerLocation.Inside, settings);
                ConfigureTrigger(outsideTriggerGameObject, settings.colliderxOffset, DoorTriggerLocation.Outside, settings);
            }
            Debug.Log($"DoorTools: Configured triggers on {door.gameObject.name} ");
        }

        /// <summary>
        /// Configure instance of open trigger
        /// </summary>
        /// <param name="triggerGameObject"></param>
        /// <param name="xOffset"></param>
        /// <param name="doorTriggerLocation"></param>
        private static void ConfigureTrigger(GameObject triggerGameObject, float xOffset, DoorTriggerLocation doorTriggerLocation, DoorEditorSettings settings)
        {
            triggerGameObject.layer = LayerMask.NameToLayer("Triggers");

            BoxCollider triggerCollider = triggerGameObject.GetComponent<BoxCollider>();
            if(!triggerCollider)
            {
                triggerCollider = triggerGameObject.AddComponent<BoxCollider>();
            }

            triggerCollider.size = new Vector3(settings.colliderDepth, settings.colliderHeight, settings.colliderWidth);
            triggerCollider.center = new Vector3(xOffset, settings.collideryOffset, settings.colliderzOffset);
            triggerCollider.isTrigger = true;

            DoorTrigger theDoorTrigger = triggerGameObject.GetComponent<DoorTrigger>();
            if (!theDoorTrigger)
            {
                theDoorTrigger = triggerGameObject.AddComponent<DoorTrigger>();
            }

            theDoorTrigger.doorTriggerLocation = doorTriggerLocation;
            theDoorTrigger.triggerTags = settings.triggerTags;
        }
        

        /// <summary>
        /// Adds a NavMeshModifier component to the Mesh GameObjects
        /// </summary>
        /// <param name="door"></param>
        private static void ConfigureNavMesh(Door door)
        {
            NavMeshModifier navMeshMod = door.gameObject.GetComponent<NavMeshModifier>();
            if (!navMeshMod)
            {
                navMeshMod = door.gameObject.AddComponent<NavMeshModifier>();
            }
            navMeshMod.ignoreFromBuild = true;
            Debug.Log($"DoorTools: Configured NavMeshModifier on {door.gameObject.name} ");
        }

        #if MCS
        /// <summary>
        /// Adds an MCS Dynamic Component to the door
        /// </summary>
        /// <param name="door"></param>
        private static void AddMCSDynamicComponent(Door door)
        {
            MCSDynamicObject mcsDynamic = door.gameObject.GetComponent<MCSDynamicObject>();
            if (!mcsDynamic)
            {
                door.gameObject.AddComponent<MCSDynamicObject>();
            }
            
            Debug.Log($"DoorTools: Configured MCSDynamic component on {door.gameObject.name} ");
        }
        #endif

        /// <summary>
        /// Returns a vector representing the dimensions of the door mesh
        /// </summary>
        /// <param name="door"></param>
        /// <returns></returns>
        private static Vector3 GetDoorDimensions(Door door)
        {
            Bounds bounds = new Bounds(door.transform.position, Vector3.zero);
            Quaternion currentRotation = door.gameObject.transform.rotation;
            door.gameObject.transform.rotation = Quaternion.Euler(0f,0f,0f);
            foreach(Renderer renderer in door.gameObject.GetComponentsInChildren<Renderer>(true))
            {
                bounds.Encapsulate(renderer.bounds);
            }
            door.gameObject.transform.rotation = currentRotation;

            return bounds.size;
        }
    }
}
