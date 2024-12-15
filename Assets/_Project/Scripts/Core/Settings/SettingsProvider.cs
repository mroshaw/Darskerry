using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public abstract class SettingsProvider : MonoBehaviour
    {
        #region Properties and Fields

        [BoxGroup("Presets")] [SerializeField] private PresetQuality defaultPreset;

        [BoxGroup("Presets")] public SettingPreset veryLowPreset;
        [BoxGroup("Presets")] public SettingPreset lowPreset;
        [BoxGroup("Presets")] public SettingPreset mediumPreset;
        [BoxGroup("Presets")] public SettingPreset highPreset;
        [BoxGroup("Presets")] public SettingPreset veryHighPreset;
        [BoxGroup("Presets")] public SettingPreset ultraPreset;

        [BoxGroup("Events")] public UnityEvent<PresetQuality> onSettingsChangedEvent;

        public PresetQuality CurrentPreset { get; private set; }
        public PresetQuality DefaultPreset => defaultPreset;

        #endregion

        #region Start

        protected virtual void Awake()
        {
            CurrentPreset = defaultPreset;
            InitSettings();
        }

        #endregion

        public void ApplyDefaultPreset()
        {
            Apply(defaultPreset);
        }

        public void Apply(PresetQuality presetQuality)
        {
            switch (presetQuality)
            {
                case PresetQuality.VeryLow:
                    ApplySettings(veryLowPreset);
                    break;
                case PresetQuality.Low:
                    ApplySettings(lowPreset);
                    break;
                case PresetQuality.Medium:
                    ApplySettings(mediumPreset);
                    break;
                case PresetQuality.High:
                    ApplySettings(highPreset);
                    break;
                case PresetQuality.VeryHigh:
                    ApplySettings(veryHighPreset);
                    break;
                case PresetQuality.Ultra:
                    ApplySettings(ultraPreset);
                    break;
            }
            CurrentPreset = presetQuality;
            onSettingsChangedEvent.Invoke(presetQuality);
        }

        protected abstract void ApplySettings(SettingPreset volumeSettings);

        protected abstract void InitSettings();

#region Editor methods
        #if UNITY_EDITOR

        [BoxGroup("Apply")] [Button("Very Low")]
        private void ApplyVeryLowPreset()
        {
            Apply(PresetQuality.VeryLow);
        }


        [BoxGroup("Apply")] [Button("Low")]
        private void ApplyLowPreset()
        {
            Apply(PresetQuality.Low);
        }

        [BoxGroup("Apply")] [Button("Medium")]
        private void ApplyMediumPreset()
        {
            Apply(PresetQuality.Medium);
        }

        [BoxGroup("Apply")] [Button("High")]
        private void ApplyHighPreset()
        {
            Apply(PresetQuality.High);
        }

        [BoxGroup("Apply")] [Button("Very High")]
        private void ApplyVeryHighPreset()
        {
            Apply(PresetQuality.VeryHigh);
        }

        [BoxGroup("Apply")] [Button("Ultra")]
        private void ApplyUltraPreset()
        {
            Apply(PresetQuality.Ultra);
        }
#endif
        #endregion

    }
}