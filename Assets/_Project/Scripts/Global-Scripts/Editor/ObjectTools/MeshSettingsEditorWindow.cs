using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.ObjectTools
{
    public class MeshSettingsEditorWindow : OdinEditorWindow
    {
        [MenuItem("Daft Apple Games/Tools/Objects/Mesh settings editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MeshSettingsEditorWindow));
        }

        [BoxGroup("Settings")] [InlineEditor] public MeshSettings meshSettings;
        [BoxGroup("Selected Objects")] public GameObject[] selectedObjects;

        /// <summary>
        /// Refresh the list of GameObjects selected
        /// </summary>
        private void OnSelectionChange()
        {
            selectedObjects = Selection.gameObjects;
        }

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

        [Button("Apply Shadows")]
        private void ApplyShadows()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.staticShadowCaster = meshSettings.isStaticShadowCaster;
                renderer.shadowCastingMode = meshSettings.shadowCastingMode;
            }
        }

        [Button("Apply Layers")]
        private void ApplyLayers()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.renderingLayerMask = (uint) meshSettings.lightLayer | (uint) meshSettings.decalLayer << 8;
            }
        }

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