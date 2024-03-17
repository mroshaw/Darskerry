using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

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
        [BoxGroup("Lighting")] public bool isStaticShadowCaster = true;
        [BoxGroup("Lighting")] public ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;
        [BoxGroup("Lighting")] public LightLayerEnum lightLayer;
        [BoxGroup("Lighting")] public DecalLayerEnum decalLayer;

        [Button("Contribute GI")]
        private void ConfigureGI()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
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
                
                GameObjectUtility.SetStaticEditorFlags(renderer.gameObject, newFlags);
            }
        }

        [Button("Light Maps")]
        private void ConfigureLightMaps()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.receiveGI = ReceiveGI.Lightmaps;
            }
        }

        [Button("Light Probes")]
        private void ConfigureLightProbes()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.receiveGI = ReceiveGI.LightProbes;
            }
        }

        [Button("Light and Decal Layers")]
        private void ConfigureLightAndDecalLayers()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.renderingLayerMask = (uint) lightLayer | (uint) decalLayer << 8;
            }

        }

        [Button("Shadow Casting Mode")]
        private void ConfigureShadowCastingMode()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.shadowCastingMode = shadowCastingMode;
            }
        }

        [Button("Static Shadow Caster")]
        private void ConfigureStaticShadowCaster()
        {
            foreach (MeshRenderer renderer in GetChildMeshRenderers())
            {
                renderer.staticShadowCaster = isStaticShadowCaster;
            }
        }



        private MeshRenderer[] GetChildMeshRenderers()
        {
            return Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>(true);
        }
    }
}
