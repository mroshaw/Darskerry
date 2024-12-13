using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    [CreateAssetMenu(fileName = "ShadowsPreset", menuName = "Daft Apple Games/Quality Presets/ShadowsPreset", order = 1)]
    public class ShadowsVolumePresets : VolumePresets
    {
        [BoxGroup("Shadow Defaults")] public float maxDistance = 500;
        [BoxGroup("Shadow Defaults")] public int cascadeCount = 3;
        [BoxGroup("Contact Shadow Defaults")] public bool contactShadowState = true;
        [BoxGroup("Micro Shadow Defaults")] public bool microShadowState = true;
    }
}