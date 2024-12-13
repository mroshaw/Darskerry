using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public class ShadowsVolumeSettingsProvider : VolumeSettingsProvider
    {
        private HDShadowSettings _shadowOverrides;
        private ContactShadows _contactShadowOverrides;
        private MicroShadowing _microShadowOverrides;

        private HDAdditionalLightData _sunHdLightData;
        private HDAdditionalLightData _moonHdLightData;

        protected override void ApplySettings(SettingPreset volumeSettings)
        {
            if (volumeSettings is ShadowsVolumePresets shadowVolumeSettings)
            {
                // Shadows
                _sunHdLightData.EnableShadows(shadowVolumeSettings.settingEnabled);
                _moonHdLightData.EnableShadows(shadowVolumeSettings.settingEnabled);
                _shadowOverrides.cascadeShadowSplitCount.value = shadowVolumeSettings.cascadeCount;

                // Contact shadows
                _contactShadowOverrides.enable.value = shadowVolumeSettings.contactShadowState;

                // Micro shadows
                _microShadowOverrides.enable.value = shadowVolumeSettings.microShadowState;

            }
        }

        protected override void InitSettings()
        {
            base.InitSettings();
            if(!LightingVolumeProfile.TryGet<HDShadowSettings>(out _shadowOverrides))
            {
                Debug.LogError($"ShadowsVolumeSettings: cannot find HDShadowSettings override on volume profile {LightingVolumeProfile.name}");
            }

            if(!LightingVolumeProfile.TryGet<ContactShadows>(out _contactShadowOverrides))
            {
                Debug.LogError($"ShadowsVolumeSettings: cannot find ContactShadows override on volume profile {LightingVolumeProfile.name}");
            }

            if(!LightingVolumeProfile.TryGet<MicroShadowing>(out _microShadowOverrides))
            {
                Debug.LogError($"ShadowsVolumeSettings: cannot find MicroShadowing override on volume profile {LightingVolumeProfile.name}");
            }

            _sunHdLightData = Sun.GetComponent<HDAdditionalLightData>();
            _moonHdLightData = Moon.GetComponent<HDAdditionalLightData>();

            _shadowOverrides.cascadeShadowSplitCount.overrideState = true;
            _contactShadowOverrides.active = true;
            _contactShadowOverrides.enable.overrideState = true;

            _microShadowOverrides.active = true;
            _microShadowOverrides.enable.overrideState = true;
        }
    }
}