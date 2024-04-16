using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Editor.ObjectTools
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LodGroupSettings", menuName = "Daft Apple Games/Objects/Mesh settings", order = 1)]
    public class MeshSettings : ScriptableObject
    {
        [BoxGroup("Lighting")] public bool isGIEnabled = true;
        [BoxGroup("Lighting")] public ReceiveGI receiveGlobalIllumination = ReceiveGI.LightProbes;
        [BoxGroup("Shadows")] public bool isStaticShadowCaster = true;
        [BoxGroup("Shadows")] public ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;
        [BoxGroup("Light Layers")] public LightLayerEnum lightLayer;
        [BoxGroup("Light Layers")] public DecalLayerEnum decalLayer;
        [BoxGroup("Additional")] public MotionVectorGenerationMode motionVectorMode = MotionVectorGenerationMode.Camera;
        [BoxGroup("Additional")] public bool allowDynamicOcclusion = false;
    }
}