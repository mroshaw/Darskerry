using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public class FogVolumeSettingsProvider : VolumeSettingsProvider
    {
        private Fog _fogOverrides;

        protected override void ApplySettings(SettingPreset volumeSettings)
        {
            if (volumeSettings is FogVolumePresets fogVolumeSettings)
            {
                _fogOverrides.active = fogVolumeSettings.settingEnabled;
                _fogOverrides.quality.value = (int)fogVolumeSettings.fogQualityLevel;
                _fogOverrides.enableVolumetricFog.value = fogVolumeSettings.volumetricEnabled;
            }
        }

        protected override void InitSettings()
        {
            base.InitSettings();
            if(!SkyGlobalVolumeProfile.TryGet<Fog>(out _fogOverrides))
            {
                Debug.LogError($"FogVolumeSettings: cannot find Fog override on volume profile {SkyGlobalVolumeProfile.name}");
            }
            _fogOverrides.active = true;
            _fogOverrides.quality.overrideState = true;
            _fogOverrides.enableVolumetricFog.overrideState = true;

        }
    }
}