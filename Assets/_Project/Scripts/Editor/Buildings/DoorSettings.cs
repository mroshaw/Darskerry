using System;
using DaftAppleGames.Darskerry.Core.Buildings;
using DaftAppleGames.Extensions;
using DaftAppleGames.Editor.Mesh;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [Serializable]
    internal class DoorSettings
    {
        [SerializeField] internal string[] doorNames;
        [SerializeField] internal string doorTriggerName = "Door Trigger";
        [SerializeField] internal float colliderHeight = 1.0f;
        [SerializeField] internal float colliderDepth = 1.0f;

        [SerializeField] internal float openAngle = 110.0f;
        [SerializeField] internal float openingTime = 2.0f;
        [SerializeField] internal float stayOpenTime = 5.0f;
        [SerializeField] internal float closingTIme = 2.0f;

        [SerializeField] internal AudioMixerGroup audioMixerGroup;
        [SerializeField] internal AudioClip[] openingClips;
        [SerializeField] internal AudioClip[] closingClips;
        [SerializeField] internal AudioClip[] closedClips;

        internal void ApplyToBuilding(Building building)
        {
            Apply(building.gameObject);
        }

        private void Apply(GameObject buildingGameObjects)
        {
            foreach (string doorName in doorNames)
            {
                GameObject[] doorGameObjects = buildingGameObjects.FindChildGameObjects(doorName, false);

                foreach (GameObject doorGameObject in doorGameObjects)
                {
                    AudioSource audioSource = doorGameObject.EnsureComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                    audioSource.outputAudioMixerGroup = audioMixerGroup;
                    audioSource.spatialBlend = 1.0f;

                    Door door = doorGameObject.EnsureComponent<Door>();
                    door.openAngle = openAngle;
                    door.openingTime = openingTime;
                    door.stayOpenTime = stayOpenTime;
                    door.closingTime = closingTIme;
                    door.openingClips = openingClips;
                    door.closingClips = closingClips;
                    door.closedClips = closedClips;

                    ConfigureDoorTrigger(door);
                }
            }
        }

        private void ConfigureDoorTrigger(Door door)
        {
            GameObject doorTriggerGameObject = door.gameObject.FindChildGameObject(doorTriggerName);
            if (!doorTriggerGameObject)
            {
                doorTriggerGameObject = new GameObject(doorTriggerName);
                doorTriggerGameObject.transform.SetParent(door.transform);
                doorTriggerGameObject.transform.localPosition = Vector3.zero;
                doorTriggerGameObject.transform.localRotation = Quaternion.identity;
            }

            BoxCollider triggerCollider = doorTriggerGameObject.EnsureComponent<BoxCollider>();
            ConfigureCollider(door, triggerCollider);
            doorTriggerGameObject.EnsureComponent<DoorTrigger>();
        }

        private void ConfigureCollider(Door door, BoxCollider doorCollider)
        {
            Vector3 doorDimensions = MeshSettings.GetMeshBoundsSize(door.gameObject);
            Vector3 doorCenter = MeshSettings.GetMeshBoundsCenter(door.gameObject);
            doorCenter = door.gameObject.transform.InverseTransformPoint(doorCenter);

            doorCollider.size = new Vector3(doorDimensions.x, colliderHeight, colliderDepth);
            doorCollider.center = new Vector3(doorCenter.x, 0, doorCenter.z);
        }
    }
}