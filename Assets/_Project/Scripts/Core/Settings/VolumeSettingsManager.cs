using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public enum PresetQuality
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
        Ultra
    }

    public class VolumeSettingsManager : Singleton<VolumeSettingsManager>
    {
        #region Class Variables

        [BoxGroup("Targets")] [SerializeField] private Volume skyGlobalVolume;
        [BoxGroup("Targets")] [SerializeField] private Volume lightingVolume;
        [BoxGroup("Targets")] [SerializeField] private Volume postProcessingVolume;
        [BoxGroup("Targets")] [SerializeField] private Light sunLight;
        [BoxGroup("Targets")] [SerializeField] private Light moonLight;

        [BoxGroup("Providers")] [SerializeField] private SsgiVolumeSettingsProvider ssgiVolumeSettings;
        [BoxGroup("Providers")] [SerializeField] private FogVolumeSettingsProvider fogVolumeSettings;
        [BoxGroup("Providers")] [SerializeField] private ShadowsVolumeSettingsProvider shadowsVolumeSettings;

        public VolumeProfile SkyGlobalVolumeProfile => skyGlobalVolume.profile;
        public VolumeProfile LightingVolumeProfile => lightingVolume.profile;
        public VolumeProfile PostProcessingVolumeProfile => postProcessingVolume.profile;

        public Light Sun => sunLight;
        public Light Moon => moonLight;

        #endregion

        #region Public Methods

        public void ApplySsgiSettings(PresetQuality presetQuality)
        {
            ApplySettings(ssgiVolumeSettings, presetQuality);
        }

        public PresetQuality GetSsgiDefault()
        {
            return ssgiVolumeSettings.DefaultPreset;
        }

        public PresetQuality GetFogDefault()
        {
            return fogVolumeSettings.DefaultPreset;
        }

        public PresetQuality GetShadowsDefault()
        {
            return shadowsVolumeSettings.DefaultPreset;
        }


        public void ApplyFogSettings(PresetQuality presetQuality)
        {
            ApplySettings(fogVolumeSettings, presetQuality);
        }

        public void ApplyShadowsSettings(PresetQuality presetQuality)
        {
            ApplySettings(shadowsVolumeSettings, presetQuality);
        }


        private void ApplySettings(SettingsProvider settingProvider, PresetQuality presetQuality)
        {
            switch (presetQuality)
            {
                case PresetQuality.VeryLow:
                    settingProvider.ApplyVeryLowPreset();
                    break;
                case PresetQuality.Low:
                    settingProvider.ApplyLowPreset();
                    break;
                case PresetQuality.Medium:
                    settingProvider.ApplyMediumPreset();
                    break;
                case PresetQuality.High:
                    settingProvider.ApplyHighPreset();
                    break;
                case PresetQuality.VeryHigh:
                    settingProvider.ApplyVeryHighPreset();
                    break;
                case PresetQuality.Ultra:
                    settingProvider.ApplyUltraPreset();
                    break;
            }
        }
        #endregion
    }
}