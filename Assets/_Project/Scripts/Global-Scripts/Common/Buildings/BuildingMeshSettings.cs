using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;
using UnityEngine.Rendering;

namespace DaftAppleGames.Common.Buildings
{
    /// <summary>
    /// Settings for lights in and outside buildings
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingMeshSettings", menuName = "Buildings/Building Mesh Settings", order = 1)]
    [Serializable]
    public class BuildingMeshSettings : ScriptableObject
    {
        [BoxGroup("Light Settings")] public bool isGIEnabled = true;
        [BoxGroup("Light Settings")] public ShadowCastingMode shadowCastingMode;
        [BoxGroup("Light Settings")] public bool isStaticShadowCaster;
        [BoxGroup("Light Settings")] public ReceiveGI receiveGI;
        [BoxGroup("Light Settings")] public bool receiveShadows;
        [BoxGroup("Light Settings")] public LightProbeUsage lightProbeUsage;
        [BoxGroup("Light Settings")] public LightLayerEnum lightLayer;
        [BoxGroup("Light Settings")] public DecalLayerEnum decalLayer;
        [BoxGroup("Layer Settings")] public string layerName;
        /// <summary>
        /// Configure a given Mesh Renderer with the settings
        /// </summary>
        /// <param name="meshRenderer"></param>
        public void ConfigureMesh(MeshRenderer meshRenderer)
        {
#if UNITY_EDITOR
            // Set static flags
            StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(meshRenderer.gameObject);
            StaticEditorFlags newFlags;
            if (isGIEnabled)
            {
                newFlags = flags | StaticEditorFlags.ContributeGI;
            }
            else
            {
                newFlags = flags & ~StaticEditorFlags.ContributeGI;
            }
            GameObjectUtility.SetStaticEditorFlags(meshRenderer.gameObject, newFlags);

            meshRenderer.shadowCastingMode = shadowCastingMode;
            meshRenderer.receiveGI = receiveGI;
            meshRenderer.receiveShadows = receiveShadows;
            meshRenderer.lightProbeUsage = lightProbeUsage;
            meshRenderer.renderingLayerMask = (uint)lightLayer | (uint)decalLayer << 8;
            meshRenderer.staticShadowCaster = isStaticShadowCaster;
            int layerIndex = LayerMask.NameToLayer(layerName);
            if (layerIndex >= 0)
            {
                meshRenderer.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
            else
            {
                Debug.Log($"Invalid layer name: {layerName}");
            }
#endif
        }
    }
}
