using System;
using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.Common.Buildings;
using DaftAppleGames.Editor.Buildings;
using DaftAppleGames.Editor.Common.Performance;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
#if HDPipeline
using UnityEngine.Rendering.HighDefinition;
#endif
namespace DaftAppleGames.Editor.Common
{
    [Serializable]
    public class MeshSettings
    {
        [BoxGroup("Editor Search Settings")]
        public EditorTools.EditorSearchCriteria searchCriteria;
#if HDPipeline
        [BoxGroup("Mesh Lighting (HDRP)")]
        public LightLayerEnum lightLayers = LightLayerEnum.LightLayerDefault;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public bool contributeGi = true;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public bool staticShadowCaster = true;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public ReceiveGI receiveGi = ReceiveGI.Lightmaps;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public LightProbeUsage lightProbes = LightProbeUsage.BlendProbes;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public ReflectionProbeUsage reflectionProbes = ReflectionProbeUsage.BlendProbes;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public MotionVectorGenerationMode motionVectors = MotionVectorGenerationMode.ForceNoMotion;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public bool dynamicOcclusion = false;
#else
        [BoxGroup("Mesh Lighting")]
        public ShadowCastingMode castShadows;
        [BoxGroup("Mesh Lighting")]
        public bool receiveShadows;
        [BoxGroup("Mesh Lighting")]
        public bool contributeGi;
        [BoxGroup("Mesh Lighting")]
        public ReceiveGI receiveGi;
        [BoxGroup("Mesh Lighting")]
        public LightProbeUsage lightProbes;
        [BoxGroup("Mesh Lighting")]
        public ReflectionProbeUsage reflectionProbes;
        [BoxGroup("Mesh Lighting")]
        public MotionVectorGenerationMode motionVectors;
        [BoxGroup("Mesh Lighting")]
        public bool dynamicOcclusion;
#endif
    }
    
    public class MeshTools : MonoBehaviour
    {
        /// <summary>
        /// Updates all MeshRenders with given settings
        /// </summary>
        /// <param name="meshRenderers"></param>
        /// <param name="settings"></param>
        public static void UpdateMeshProperties(MeshRenderer[] meshRenderers, MeshSettings settings)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                Debug.Log($"Update Meshes - {meshRenderer.gameObject.name}...");
#if HDPipeline
                // int bitmask = (int)meshRenderer.renderingLayerMask;
                // bitmask = LightTools.CalculateBitmask(bitmask, settings.lightLayers);
                // meshRenderer.renderingLayerMask = (uint)bitmask;
                meshRenderer.renderingLayerMask = 0;
                meshRenderer.renderingLayerMask = (uint)settings.lightLayers;
                meshRenderer.staticShadowCaster = settings.staticShadowCaster;
                // Set Contribute GI
                StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(meshRenderer.gameObject);
                flags = flags | StaticEditorFlags.ContributeGI;
                GameObjectUtility.SetStaticEditorFlags(meshRenderer.gameObject, flags);
                meshRenderer.receiveGI = settings.receiveGi;
                meshRenderer.lightProbeUsage = settings.lightProbes;
                meshRenderer.motionVectorGenerationMode = settings.motionVectors;
                meshRenderer.allowOcclusionWhenDynamic = settings.dynamicOcclusion;
                meshRenderer.shadowCastingMode = settings.shadowCastingMode;
#else
                meshRenderer.shadowCastingMode = settings.castShadows;
                meshRenderer.receiveShadows = settings.receiveShadows;
                meshRenderer.receiveGI = settings.receiveGi;
                meshRenderer.lightProbeUsage = settings.lightProbes;
                meshRenderer.reflectionProbeUsage = settings.reflectionProbes;
                meshRenderer.motionVectorGenerationMode = settings.motionVectors;
                meshRenderer.allowOcclusionWhenDynamic = settings.dynamicOcclusion;
#endif
            }
        }
        
        /// <summary>
        /// Returns all MeshRenderers in gameobject passed, using the search criteria
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <param name="includeInactive"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static MeshRenderer[] FindMeshRenderersInGameObjects(GameObject[] gameObjects, bool includeInactive, 
            EditorTools.EditorSearchCriteria searchCriteria)
        {
            List<MeshRenderer> allMeshRenderersInGameObjects = new();
            
            // Process all GameObjects - assumed to be top level "buildings"
            foreach (GameObject buildingGameObject in gameObjects)
            {
                Transform[] childTransforms = buildingGameObject.GetComponentsInChildren<Transform>(true);

                foreach (Transform currentTransform in childTransforms)
                {
                    GameObject currentGameObject = currentTransform.gameObject;
                    // Look for GameObjects that meet the search criteria
                    if ((searchCriteria == null | searchCriteria.ParentGameObjectNames.Length == 0 || searchCriteria.ParentGameObjectNames.Any(currentGameObject.name.Contains)) &&
                        (searchCriteria == null | searchCriteria.ParentGameObjectLayers.Length == 0 || searchCriteria.ParentGameObjectLayers.Any(LayerMask.LayerToName(currentGameObject.layer).Contains)) &&
                        (searchCriteria == null | searchCriteria.ParentGameObjectLayers.Length == 0 || searchCriteria.ParentGameObjectLayers.Any(currentGameObject.tag.Contains)))
                    {
                        // Process all components
                        MeshRenderer[] allMeshRenderers = currentGameObject.GetComponentsInChildren<MeshRenderer>(includeInactive);
                        foreach (MeshRenderer meshRenderer in allMeshRenderers)
                        {
                            // Look for child components
                            if(searchCriteria.ComponentNames.Length == 0 || searchCriteria.ComponentNames.Any(meshRenderer.name.Contains))
                            {
                                allMeshRenderersInGameObjects.Add(meshRenderer);
                            }
                        }
                    }
                }
            }
            return allMeshRenderersInGameObjects.ToArray();
        }

        /// <summary>
        /// Returns all Mesh Renderers in given Game Object
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static MeshRenderer[] FindAllMeshRendersInGameObject(GameObject gameObject)
        {
            return gameObject.GetComponentsInChildren<MeshRenderer>(true);
        }

        public static MeshRenderer[] FindMeshRenderersInBuildingGameObjects(GameObject[] gameObjects, BuildingLayer buildingLayer)
        {
            List<GameObject> parents = new List<GameObject>();

            // Process all GameObjects - assumed to be top level "buildings"
            foreach (GameObject buildingGameObject in gameObjects)
            {
                Building building = buildingGameObject.GetComponent<Building>();
                if (!building)
                {
                    Debug.Log("MeshTools: GameObject has no Building component");
                    continue;
                }


                switch (buildingLayer)
                {
                    case BuildingLayer.Exterior:
                        parents.AddRange(building.exteriors);
                        break;
                    case BuildingLayer.Interior:
                        parents.AddRange(building.interiors);
                        break;
                    case BuildingLayer.ExteriorProps:
                        parents.AddRange(building.exteriorProps);
                        break;
                    case BuildingLayer.InteriorProps:
                        parents.AddRange(building.interiorProps);
                        break;
                }
            }

            EditorTools.EditorSearchCriteria searchCriteria =
                new EditorTools.EditorSearchCriteria(Array.Empty<string>(), Array.Empty<string>(), Array.Empty<string>(), Array.Empty<string>());
            return FindMeshRenderersInGameObjects(parents.ToArray(), true, searchCriteria);
        }
    }
}