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
    [DefaultExecutionOrder(-500)]
    public class VolumeSettingsManager : Singleton<VolumeSettingsManager>
    {
        #region Class Variables

        [BoxGroup("Targets")] [SerializeField] private Volume skyGlobalVolume;
        [BoxGroup("Targets")] [SerializeField] private Volume lightingVolume;
        [BoxGroup("Targets")] [SerializeField] private Volume postProcessingVolume;
        [BoxGroup("Targets")] [SerializeField] private Light sunLight;
        [BoxGroup("Targets")] [SerializeField] private Light moonLight;

        [BoxGroup("Providers")] [SerializeField] private VolumeSettingsProvider[] volumeSettingsProviders;

        public VolumeProfile SkyGlobalVolumeProfile => skyGlobalVolume.profile;
        public VolumeProfile LightingVolumeProfile => lightingVolume.profile;
        public VolumeProfile PostProcessingVolumeProfile => postProcessingVolume.profile;
        public Light Sun => sunLight;
        public Light Moon => moonLight;

        #endregion

        #region Startup

        protected override void Awake()
        {
            base.Awake();
            volumeSettingsProviders = GetComponentsInChildren<VolumeSettingsProvider>(false);
        }

        #endregion

        #region Public Methods

        public void ApplySettings<T>(PresetQuality presetQuality) where T : VolumeSettingsProvider
        {
            foreach (VolumeSettingsProvider currProvider in volumeSettingsProviders)
            {
                if (currProvider.GetType() == typeof(T))
                {
                    currProvider.Apply(presetQuality);
                }
            }
        }

        public PresetQuality GetCurrentPreset<T>() where T : VolumeSettingsProvider
        {
            foreach (VolumeSettingsProvider currProvider in volumeSettingsProviders)
            {
                if (currProvider.GetType() == typeof(T))
                {
                    return currProvider.CurrentPreset;
                }
            }

            return PresetQuality.Medium;
        }

        public PresetQuality GetDefaultPreset<T>() where T : VolumeSettingsProvider
        {
            foreach (VolumeSettingsProvider currProvider in volumeSettingsProviders)
            {
                if (currProvider.GetType() == typeof(T))
                {
                    return currProvider.DefaultPreset;
                }
            }
            return PresetQuality.Medium;
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        [Button("Refresh Providers")]
        private void RefreshProviders()
        {
            volumeSettingsProviders = GetComponentsInChildren<VolumeSettingsProvider>(false);
        }

#endif
        #endregion
    }
}