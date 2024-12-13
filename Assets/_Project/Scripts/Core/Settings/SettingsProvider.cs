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

        [BoxGroup("Events")] public UnityEvent<SettingPreset> onSettingsChangedEvent;

        public PresetQuality CurrentPreset { get; private set; }
        public PresetQuality DefaultPreset => defaultPreset;

        #endregion

        #region Start

        protected virtual void Awake()
        {
            CurrentPreset = defaultPreset;
            InitSettings();
        }

        protected virtual void Start()
        {

        }
        #endregion


        [BoxGroup("Apply")] [Button("Very Low")]
        public void ApplyVeryLowPreset()
        {
            CurrentPreset = PresetQuality.VeryLow;
            ApplySettings(veryLowPreset);
        }


        [BoxGroup("Apply")] [Button("Low")]
        public void ApplyLowPreset()
        {
            CurrentPreset = PresetQuality.Low;
            ApplyPreset(lowPreset);
        }

        [BoxGroup("Apply")] [Button("Medium")]
        public void ApplyMediumPreset()
        {
            CurrentPreset = PresetQuality.Medium;
            ApplyPreset(mediumPreset);
        }

        [BoxGroup("Apply")] [Button("High")]
        public void ApplyHighPreset()
        {
            CurrentPreset = PresetQuality.High;
            ApplyPreset(highPreset);
        }

        [BoxGroup("Apply")] [Button("Very High")]
        public void ApplyVeryHighPreset()
        {
            CurrentPreset = PresetQuality.VeryHigh;
            ApplyPreset(veryHighPreset);
        }

        [BoxGroup("Apply")] [Button("Ultra")]
        public void ApplyUltraPreset()
        {
            CurrentPreset = PresetQuality.Ultra;
            ApplyPreset(ultraPreset);
        }

        private void ApplyPreset(SettingPreset preset)
        {
            ApplySettings(preset);
            onSettingsChangedEvent.Invoke(preset);
        }

        protected abstract void ApplySettings(SettingPreset volumeSettings);

        protected abstract void InitSettings();

    }
}