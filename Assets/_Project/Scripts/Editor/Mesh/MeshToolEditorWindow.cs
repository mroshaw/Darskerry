using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace DaftAppleGames.Editor.ObjectTools
{
    public enum MeshProperties { CastShadows, StaticShadowCaster, ContributeGI, ReceiveGI, MotionVectors, DynamicOcclusion, RenderLayerMask, Priority }
    public enum LightLayerPresets { Interior, Exterior, Both }

    public class MeshToolEditorWindow : OdinEditorWindow
    {
        [MenuItem("Daft Apple Games/Meshes/Mesh Tool")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MeshToolEditorWindow));
        }

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


        [PropertyOrder(3)]
        [BoxGroup("Lighting Settings")]
        [SerializeField]
        private ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;

        [PropertyOrder(3)]
        [BoxGroup("Lighting Settings")]
        [Button("Apply Shadow Casting")]
        private void ApplyContributeGi()
        {
            ApplyIndividualSetting(MeshProperties.CastShadows);
        }

        [PropertyOrder(4)]
        [BoxGroup("Lighting Settings")]
        [SerializeField]
        private bool staticShadowCaster = true;

        [PropertyOrder(4)]
        [BoxGroup("Lighting Settings")]
        [Button("Apply Static Shadow Caster")]
        private void ApplyStaticShadowCaster()
        {
            ApplyIndividualSetting(MeshProperties.StaticShadowCaster);
        }

        [PropertyOrder(5)]
        [BoxGroup("Lighting Settings")]
        [SerializeField]
        private bool contributeGI = true;

        [PropertyOrder(5)]
        [BoxGroup("Lighting Settings")]
        [Button("Apply Contribute GI")]
        private void ApplyContributeGI()
        {
            ApplyIndividualSetting(MeshProperties.ContributeGI);
        }

        [PropertyOrder(6)]
        [BoxGroup("Lighting Settings")]
        [SerializeField]
        private ReceiveGI receiveGI = ReceiveGI.LightProbes;

        [PropertyOrder(6)]
        [BoxGroup("Lighting Settings")]
        [Button("Apply Receive GI")]
        private void ApplyReceiveGi()
        {
            ApplyIndividualSetting(MeshProperties.ReceiveGI);
        }

        [PropertyOrder(7)][BoxGroup("Lighting Settings")][SerializeField] private LightLayerPresets lightlayerPreset;
        [PropertyOrder(7)]
        [BoxGroup("Lighting Settings")]
        [Button("Apply Render Layers")]
        private void ApplyRenderLayer()
        {
            ApplyIndividualSetting(MeshProperties.RenderLayerMask);
        }


        [PropertyOrder(8)]
        [BoxGroup("Lighting Settings")]
        [Button("Set Shadows on LODS")]
        private void SetShadowsOnLods()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                AdjustLODShadows(gameObject);
            }

            SavePrefabChanges();
        }



        [PropertyOrder(8)][BoxGroup("LOD Fixer")][SerializeField] private LodGroupSettings lodGroupSettings = new();
        [PropertyOrder(8)]
        [Button("Fix LOD Groups")]
        public void FixLodGroups()
        {

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ConfigureLodGroup(gameObject);
            }
        }


        private void ApplyIndividualSetting(MeshProperties meshProperties)
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                Debug.Log($"Updating '{meshProperties}' on '{renderer.gameObject.name}'");
                switch (meshProperties)
                {
                    case MeshProperties.CastShadows:
                        renderer.shadowCastingMode = shadowCastingMode;
                        break;
                    case MeshProperties.StaticShadowCaster:
                        renderer.staticShadowCaster = staticShadowCaster;
                        break;
                    case MeshProperties.ContributeGI:
                        StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(renderer.gameObject);
                        StaticEditorFlags newFlags;
                        if (contributeGI)
                        {
                            newFlags = flags | StaticEditorFlags.ContributeGI;
                        }
                        else
                        {
                            newFlags = flags & ~StaticEditorFlags.ContributeGI;
                        }
                        GameObjectUtility.SetStaticEditorFlags(renderer.gameObject, newFlags);
                        break;
                    case MeshProperties.ReceiveGI:
                        renderer.receiveGI = receiveGI;
                        break;
                    case MeshProperties.RenderLayerMask:
                        switch (lightlayerPreset)
                        {
                            case LightLayerPresets.Interior:
                                renderer.renderingLayerMask = RenderingLayerMask.GetMask("InteriorOnly");
                                break;

                            case LightLayerPresets.Exterior:
                                renderer.renderingLayerMask = RenderingLayerMask.GetMask("ExteriorOnly");
                                break;

                            case LightLayerPresets.Both:
                                renderer.renderingLayerMask = RenderingLayerMask.GetMask("InteriorOnly", "ExteriorOnly");
                                break;
                        }

                        break;

                }
            }
        }

        /// <summary>
        /// Return all MeshRenderers in selected GameObjects
        /// </summary>
        /// <returns></returns>
        private List<MeshRenderer> GetChildMeshRenderers()
        {
            List<MeshRenderer> allRenderers = new List<MeshRenderer>();

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                // Check if prefab
                allRenderers.AddRange(gameObject.GetComponentsInChildren<MeshRenderer>(true));
            }

            return allRenderers;
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
                    UnityEditor.EditorUtility.SetDirty(gameObject);
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

        private void ConfigureLodGroup(GameObject prefabGameObject)
        {
            LODGroup lodGroup = prefabGameObject.GetComponent<LODGroup>();
            if (!lodGroup)
            {
                Debug.Log($"No LODGroup on {prefabGameObject.name}");
                return;
            }
            lodGroup.fadeMode = lodGroupSettings.fadeMode;
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
                    lodWeights = lodGroupSettings.twoLodWeights;
                    break;
                case 3:
                    lodWeights = lodGroupSettings.threeLodWeights;
                    break;
                case 4:
                    lodWeights = lodGroupSettings.fourLodWeights;
                    break;
                case 5:
                    lodWeights = lodGroupSettings.fiveLodWeights;
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

            float maxLength = 1 - lodGroupSettings.cullRatio;
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