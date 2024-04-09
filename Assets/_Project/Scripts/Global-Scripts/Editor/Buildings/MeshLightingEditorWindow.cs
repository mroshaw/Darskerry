using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Scripts.Editor.Buildings
{
    public class MeshLightingEditorWindow : OdinEditorWindow
    {
        [MenuItem("Window/Buildings/Mesh Lighting Settings")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MeshLightingEditorWindow));
        }

        [BoxGroup("Lighting")] public bool isGIEnabled = true;
        [BoxGroup("Lighting")] public ReceiveGI receiveGlobalIllumination = ReceiveGI.LightProbes;
        [BoxGroup("Shadows")] public bool isStaticShadowCaster = true;
        [BoxGroup("Shadows")] public ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;
        [BoxGroup("Light Layers")] public LightLayerEnum lightLayer;
        [BoxGroup("Light Layers")] public DecalLayerEnum decalLayer;
        [BoxGroup("Additional")] public MotionVectorGenerationMode motionVectorMode = MotionVectorGenerationMode.Camera;
        [BoxGroup("Additional")] public bool allowDynamicOcclusion = false;

        [BoxGroup("Selected Objects")] public GameObject[] selectedObjects;

        private bool _isPrefabDirty = false;

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
                if (isGIEnabled)
                {
                    newFlags = flags | StaticEditorFlags.ContributeGI;
                }
                else
                {
                    newFlags = flags & ~StaticEditorFlags.ContributeGI;
                }

                renderer.receiveGI = receiveGlobalIllumination;

                GameObjectUtility.SetStaticEditorFlags(renderer.gameObject, newFlags);
            }
        }

        [Button("Apply Shadows")]
        private void ApplyShadows()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.staticShadowCaster = isStaticShadowCaster;
                renderer.shadowCastingMode = shadowCastingMode;
            }
        }

        [Button("Apply Layers")]
        private void ApplyLayers()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.renderingLayerMask = (uint) lightLayer | (uint) decalLayer << 8;
            }
        }

        [Button("Apply Additional")]
        private void ApplyAdditional()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.motionVectorGenerationMode = motionVectorMode;
                renderer.allowOcclusionWhenDynamic = allowDynamicOcclusion;
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
        /// Marks any amended prefabs as dirty
        /// </summary>
        private void MarkPrefabsAsDirty()
        {

        }
    }
}