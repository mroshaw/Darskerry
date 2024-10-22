using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace DaftAppleGames.Editor.ObjectTools
{
    public enum MeshProperties { CastShadows, StaticShadowCaster, ContributeGI, ReceiveGI, MotionVectors, DynamicOcclusion, RenderLayerMask, Priority }

    public class MeshSettingsEditorWindow : OdinEditorWindow
    {
        [MenuItem("Daft Apple Games/Buildings/Mesh Tool")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MeshSettingsEditorWindow));
        }

        [PropertyOrder(1)] [BoxGroup("Settings")] [InlineEditor][SerializeField] private MeshSettings meshSettings;
        [PropertyOrder(1)] [BoxGroup("Selected Objects")] [SerializeField] private GameObject[] selectedObjects;

        /// <summary>
        /// Refresh the list of GameObjects selected
        /// </summary>
        private void OnSelectionChange()
        {
            selectedObjects = Selection.gameObjects;
        }

        [PropertyOrder(2)] [Button("Apply Lighting")]
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

        [PropertyOrder(2)] [Button("Apply Shadows")]
        private void ApplyShadows()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.staticShadowCaster = meshSettings.isStaticShadowCaster;
                renderer.shadowCastingMode = meshSettings.shadowCastingMode;
            }
        }

        [PropertyOrder(2)] [Button("Apply Layers")]
        private void ApplyLayers()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.renderingLayerMask = (uint) meshSettings.lightLayer | (uint) meshSettings.decalLayer << 8;
            }
        }

        [PropertyOrder(2)] [Button("Apply Additional")]
        private void ApplyAdditional()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.motionVectorGenerationMode = meshSettings.motionVectorMode;
                renderer.allowOcclusionWhenDynamic = meshSettings.allowDynamicOcclusion;
            }
        }

        [PropertyOrder(3)][BoxGroup("Individual Settings")][SerializeField] private ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;
        [PropertyOrder(3)]
        [BoxGroup("Individual Settings")]
        [Button("Apply")]
        private void ApplyContributeGi()
        {
            ApplyIndividualSetting(MeshProperties.ContributeGI);
        }

        [PropertyOrder(4)][BoxGroup("Individual Settings")][SerializeField] private bool staticShadowCaster = true;
        [PropertyOrder(4)]
        [BoxGroup("Individual Settings")]
        [Button("Apply")]
        private void ApplyStaticShadowCaster()
        {
            ApplyIndividualSetting(MeshProperties.StaticShadowCaster);
        }

        [PropertyOrder(5)][BoxGroup("Individual Settings")][SerializeField] private bool contributeGI = true;
        [PropertyOrder(5)]
        [BoxGroup("Individual Settings")]
        [Button("Apply")]
        private void ApplyContributeGI()
        {
            ApplyIndividualSetting(MeshProperties.ContributeGI);
        }

        [PropertyOrder(6)][BoxGroup("Individual Settings")][SerializeField] private ReceiveGI receiveGI = ReceiveGI.LightProbes;
        [PropertyOrder(6)]
        [BoxGroup("Individual Settings")]
        [Button("Apply")]
        private void ApplyReceiveGi()
        {
            ApplyIndividualSetting(MeshProperties.ReceiveGI);
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

                        break;
                    case MeshProperties.ReceiveGI:
                        renderer.receiveGI = receiveGI;
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
    }
}