using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    [CreateAssetMenu(fileName = "FogVolumeSettingsSO", menuName = "Daft Apple Games/Quality Presets/FogPreset", order = 1)]
    public class FogVolumePresets : VolumePresets
    {
        [BoxGroup("Defaults")] public ScalableSettingLevelParameter.Level volumetricFogQualityLevel;
        [BoxGroup("Defaults")] public bool volumetricEnabled = false;
    }
}