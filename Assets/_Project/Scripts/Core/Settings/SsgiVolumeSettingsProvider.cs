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
                _giProfileOverrides.quality.levelAndOverride = ((int)ssgiVolumeSettings.ssgiQualityLevel, true);
                _giProfileOverrides.maxRaySteps = ssgiVolumeSettings.maxRaySteps;
                _giProfileOverrides.denoise = ssgiVolumeSettings.denoiseEnabled;
                _giProfileOverrides.halfResolutionDenoiser = ssgiVolumeSettings.halfResDenoiserEnabled;
                _giProfileOverrides.secondDenoiserPass = ssgiVolumeSettings.secondDenoiserEnabled;
            }
        }

        protected override void InitSettings()
        {
            base.InitSettings();
            if(!LightingVolumeProfile.TryGet<GlobalIllumination>(out _giProfileOverrides))
            {
                Debug.LogError($"SsgiVolumeSettingsProvider: cannot find GlobalIllumination override on volume profile {LightingVolumeProfile.name}");
            }
            _giProfileOverrides.active = true;
            _giProfileOverrides.quality.overrideState = true;
            _giProfileOverrides.enable.overrideState = true;
            _giProfileOverrides.quality.levelAndOverride = (0, true);
            _giProfileOverrides.fullResolutionSS.overrideState = true;
        }
    }
}