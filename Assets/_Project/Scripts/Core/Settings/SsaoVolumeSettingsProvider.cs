using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public class SsaoVolumeSettingsProvider : VolumeSettingsProvider
    {
        private ScreenSpaceAmbientOcclusion _ssaoProfileOverrides;

        protected override void ApplySettings(SettingPreset volumeSettings)
        {
            if (volumeSettings is SsaoVolumePresets ssaoVolumeSettings)
            {
                _ssaoProfileOverrides.intensity.value = ssaoVolumeSettings.settingEnabled? 0 : ssaoVolumeSettings.intensity;
                _ssaoProfileOverrides.quality.levelAndOverride = ((int)ssaoVolumeSettings.ssaoQualityLevel, true);
                _ssaoProfileOverrides.fullResolution = ssaoVolumeSettings.fullResolution;
                _ssaoProfileOverrides.stepCount = ssaoVolumeSettings.stepCount;
            }
        }

        protected override void InitSettings()
        {
            base.InitSettings();
            if(!LightingVolumeProfile.TryGet<ScreenSpaceAmbientOcclusion>(out _ssaoProfileOverrides))
            {
                Debug.LogError($"SsaoVolumeSettings: cannot find ScreenSpaceAmbientOcclusion override on volume profile {LightingVolumeProfile.name}");
            }
            _ssaoProfileOverrides.active = true;
            _ssaoProfileOverrides.quality.overrideState = true;
            _ssaoProfileOverrides.intensity.overrideState = true;

        }
    }
}