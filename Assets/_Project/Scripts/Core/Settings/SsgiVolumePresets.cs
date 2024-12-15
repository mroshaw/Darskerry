using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    [CreateAssetMenu(fileName = "SSGIPreset", menuName = "Daft Apple Games/Quality Presets/SSGIPreset", order = 1)]
    public class SsgiVolumePresets : VolumePresets
    {
        [BoxGroup("Defaults")] public ScalableSettingLevelParameter.Level ssgiQualityLevel;
        [BoxGroup("Defaults")] public int maxRaySteps;
        [BoxGroup("Defaults")] public bool denoiseEnabled;
        [BoxGroup("Defaults")] public bool halfResDenoiserEnabled;
        [BoxGroup("Defaults")] public bool secondDenoiserEnabled;
        [BoxGroup("Defaults")] public bool fullResolution;
    }
}