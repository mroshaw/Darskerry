using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using RenderingLayerMask = UnityEngine.Rendering.HighDefinition.RenderingLayerMask;

namespace DaftAppleGames.Editor.ObjectTools
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "MeshToolSettings", menuName = "Daft Apple Games/Buildings/Mesh Tool Settings", order = 1)]
    public class MeshSettings : ScriptableObject
    {
        [BoxGroup("Lighting")] public bool isGIEnabled = true;
        [BoxGroup("Lighting")] public ReceiveGI receiveGlobalIllumination = ReceiveGI.LightProbes;
        [BoxGroup("Shadows")] public bool isStaticShadowCaster = true;
        [BoxGroup("Shadows")] public ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;
        [BoxGroup("Light Layers")] public UnityEngine.Rendering.HighDefinition.RenderingLayerMask lightLayer;
        [BoxGroup("Light Layers")] public RenderingLayerMask decalLayer;
        [BoxGroup("Additional")] public MotionVectorGenerationMode motionVectorMode = MotionVectorGenerationMode.Camera;
        [BoxGroup("Additional")] public bool allowDynamicOcclusion = false;
    }
}