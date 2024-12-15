using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    [CreateAssetMenu(fileName = "SSAOPreset", menuName = "Daft Apple Games/Quality Presets/SSAOPreset", order = 1)]
    public class SsaoVolumePresets : VolumePresets
    {
        [BoxGroup("Defaults")] public ScalableSettingLevelParameter.Level ssaoQualityLevel;
        [BoxGroup("Defaults")] public float intensity;
        [BoxGroup("Defaults")] public bool fullResolution;
        [BoxGroup("Defaults")] public int stepCount;

    }
}