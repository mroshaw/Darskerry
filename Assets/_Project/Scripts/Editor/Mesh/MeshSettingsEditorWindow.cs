using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Mesh
{
    public class MeshSettingsEditorWindow : OdinEditorWindow
    {
        [MenuItem("Daft Apple Games/Meshes/Mesh Settings Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MeshSettingsEditorWindow));
        }

        [PropertyOrder(1)]
        [BoxGroup("Settings")]
        [InlineEditor]
        [SerializeField]
        private MeshSettings meshSettings;

        [PropertyOrder(1)]
        [BoxGroup("Selected Objects")]
        [SerializeField]
        private GameObject[] selectedObjects;

        /// <summary>
        /// Refresh the list of GameObjects selected
        /// </summary>
        private void OnSelectionChange()
        {
            selectedObjects = Selection.gameObjects;
        }

        [PropertyOrder(2)]
        [Button("Apply Lighting")]
        private void ApplyLighting()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.receiveGI = ReceiveGI.LightProbes;
                StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(renderer.gameObject);
                StaticEditorFlags newFlags;
                if (meshSettings.isGIEnabled)
                {
                    newFlags = flags | StaticEditorFlags.ContributeGI;
                }
                else
                {
                    newFlags = flags & ~StaticEditorFlags.ContributeGI;
                }

                renderer.receiveGI = meshSettings.receiveGlobalIllumination;

                GameObjectUtility.SetStaticEditorFlags(renderer.gameObject, newFlags);
            }
        }

        [PropertyOrder(2)]
        [Button("Apply Shadows")]
        private void ApplyShadows()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.staticShadowCaster = meshSettings.isStaticShadowCaster;
                renderer.shadowCastingMode = meshSettings.shadowCastingMode;
            }
        }

        [PropertyOrder(2)]
        [Button("Apply Layers")]
        private void ApplyLayers()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                // renderer.renderingLayerMask = (uint)meshSettings.lightLayer | (uint)meshSettings.decalLayer << 8;
            }
        }

        [PropertyOrder(2)]
        [Button("Apply Additional")]
        private void ApplyAdditional()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.motionVectorGenerationMode = meshSettings.motionVectorMode;
                renderer.allowOcclusionWhenDynamic = meshSettings.allowDynamicOcclusion;
            }
        }

        /// <summary>
        /// Return all MeshRenderers in selected GameObjects
        /// </summary>
        /// <returns></returns>
        private List<MeshRenderer> GetChildMeshRenderers()
        {
            List<MeshRenderer> allRenderers = new();

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                // Check if prefab
                allRenderers.AddRange(gameObject.GetComponentsInChildren<MeshRenderer>(true));
            }

            return allRenderers;
        }

        [PropertyOrder(7)]
        [Button("Set Shadows on LODS")]
        private void SetShadowsOnLods()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                AdjustLODShadows(gameObject);
            }

            SavePrefabChanges();
        }

        /// <summary>
        /// If any of the Selection is a Prefab, mark as dirty and force a save
        /// </summary>
        private void SavePrefabChanges()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                {
                    EditorUtility.SetDirty(gameObject);
                    AssetDatabase.SaveAssetIfDirty(gameObject);
                }
            }
        }

        // Call this method to adjust shadows on all LODGroups within the target object
        public void AdjustLODShadows(GameObject targetGameObject)
        {
            // Get all LODGroup components in the target object and its children
            LODGroup[] lodGroups = targetGameObject.GetComponentsInChildren<LODGroup>();

            // Loop through each LODGroup
            foreach (LODGroup lodGroup in lodGroups)
            {
                // Get the LODs array from the LODGroup
                LOD[] lods = lodGroup.GetLODs();

                // Loop through each LOD in the LODGroup
                for (int i = 0; i < lods.Length; i++)
                {
                    // Get all renderers for the current LOD
                    Renderer[] renderers = lods[i].renderers;

                    // For each renderer, adjust the Cast Shadows property
                    foreach (Renderer renderer in renderers)
                    {
                        if (renderer is MeshRenderer meshRenderer)
                        {
                            if (i == 0)
                            {
                                // Set Cast Shadows to true for LOD0
                                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                            }
                            else
                            {
                                // Set Cast Shadows to false for all other LODs
                                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                            }
                        }
                    }
                }
            }
        }

        [PropertyOrder(8)] [BoxGroup("LOD Fixer")] [SerializeField] private LodGroupSettingsSO lodGroupSettings;

        [PropertyOrder(8)]
        [Button("Fix LOD Groups")]
        public void FixLodGroups()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ConfigureLodGroup(gameObject);
            }
        }

        private void ConfigureLodGroup(GameObject prefabGameObject)
        {
            LODGroup lodGroup = prefabGameObject.GetComponent<LODGroup>();
            if (!lodGroup)
            {
                Debug.Log($"No LODGroup on {prefabGameObject.name}");
                return;
            }

            lodGroup.fadeMode = lodGroupSettings.LodGroupSettings.fadeMode;
            int numberOfLods = lodGroup.lodCount;
            Debug.Log($"Number of LODs in {prefabGameObject.name}: {numberOfLods}");
            LOD[] lods = lodGroup.GetLODs();

            if (numberOfLods == 0)
            {
                Debug.Log($"LODGroup has no LODs on {prefabGameObject.name}");
                return;
            }

            float[] lodWeights = new[] { 0.0f };

            switch (numberOfLods)
            {
                case 2:
                    lodWeights = lodGroupSettings.LodGroupSettings.twoLodWeights;
                    break;
                case 3:
                    lodWeights = lodGroupSettings.LodGroupSettings.threeLodWeights;
                    break;
                case 4:
                    lodWeights = lodGroupSettings.LodGroupSettings.fourLodWeights;
                    break;
                case 5:
                    lodWeights = lodGroupSettings.LodGroupSettings.fiveLodWeights;
                    break;
            }

            float weightSum = 0;
            for (int k = 0; k < lods.Length; k++)
            {
                if (k >= lodWeights.Length)
                {
                    weightSum += lodWeights[lodWeights.Length - 1];
                }
                else
                {
                    weightSum += lodWeights[k];
                }
            }

            Debug.Log($"Total LOD weights: {weightSum}");

            float maxLength = 1 - lodGroupSettings.LodGroupSettings.cullRatio;
            float curLodPos = 1;
            for (int j = 0; j < lods.Length; j++)
            {
                float weight = j < lodWeights.Length ? lodWeights[j] : lodWeights[lodWeights.Length - 1];

                float lengthRatio = weightSum != 0 ? weight / weightSum : 1;

                float lodLength = maxLength * lengthRatio;
                curLodPos = curLodPos - lodLength;

                lods[j].screenRelativeTransitionHeight = curLodPos;
            }

            for (int i = 0; i < lods.Length; i++)
            {
                Debug.Log($"Lod {i}: {lods[i].screenRelativeTransitionHeight}");
            }

            lodGroup.SetLODs(lods);

            // Recalculate bounds
            lodGroup.RecalculateBounds();

            SavePrefabChanges();
        }
    }
}