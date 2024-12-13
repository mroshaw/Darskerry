using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public class SsgiVolumeSettingsProvider : VolumeSettingsProvider
    {
        private GlobalIllumination _giProfileOverrides;

        protected override void ApplySettings(SettingPreset volumeSettings)
        {
            if (volumeSettings is SsgiVolumePresets ssgiVolumeSettings)
            {
                _giProfileOverrides.enable.value = ssgiVolumeSettings.settingEnabled;
                _giProfileOverrides.fullResolutionSS.value = ssgiVolumeSettings.fullResolution;
                _giProfileOverrides.quality.value = (int)ssgiVolumeSettings.ssgiQualityLevel;
            }
        }

        protected override void InitSettings()
        {
            base.InitSettings();
            if(!LightingVolumeProfile.TryGet<GlobalIllumination>(out _giProfileOverrides))
            {
                Debug.LogError($"SSGIVolumeSettings: cannot find SSGI override on volume profile {LightingVolumeProfile.name}");
            }
            _giProfileOverrides.active = true;
            _giProfileOverrides.quality.overrideState = true;
            _giProfileOverrides.enable.overrideState = true;
            _giProfileOverrides.fullResolutionSS.overrideState = true;
        }
    }
}