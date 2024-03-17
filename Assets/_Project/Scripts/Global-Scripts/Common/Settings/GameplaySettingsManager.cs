using UnityEngine;

namespace DaftAppleGames.Common.Settings
{
    public class GameplaySettingsManager : BaseSettingsManager, ISettings
    {
        [Header("Gameplay Defaults")]
        public bool defaultBloodAndGore = true;
        public bool defaultHarmAnimals = false;
        public bool defaultActionPrompt = true;
        public int defaultDifficultyIndex = 2;
        
        [Header("Setting Keys")]
        public string bloodAndGoreKey = "BloodAndGore";
        public string harmAnimalsKey = "HarmAnimals";
        public string actionPromptKey = "ActionPrompt";
        public string difficultyIndexKey = "Difficulty";
        
        private bool _bloodAndGore;
        private bool _harmAnimals;
        private bool _actionPrompt;
        private int _difficultyIndex;
        
        // Public properties
        public bool BloodAndGore
        {
            get => _bloodAndGore;
            set => _bloodAndGore = value;
        }

        public bool HarmAnimals
        {
            get => _harmAnimals;
            set => _harmAnimals = value;
        }

        public bool ActionPrompt
        {
            get => _actionPrompt;
            set => _actionPrompt = value;
        }

        public int DifficultyIndex
        {
            get => _difficultyIndex;
            set => _difficultyIndex = value;
        }
        
        /// <summary>
        /// Save all settings
        /// </summary>
        public override void SaveSettings()
        {
            SettingsUtils.SaveBoolSetting(bloodAndGoreKey,_bloodAndGore);
            SettingsUtils.SaveBoolSetting(harmAnimalsKey, _harmAnimals);
            SettingsUtils.SaveBoolSetting(actionPromptKey, _actionPrompt);
            SettingsUtils.SaveIntSetting(difficultyIndexKey, _difficultyIndex);
            base.SaveSettings();
        }

        /// <summary>
        /// Load all settings
        /// </summary>
        public override void LoadSettings()
        {
            _bloodAndGore = SettingsUtils.LoadBoolSetting(bloodAndGoreKey, defaultBloodAndGore);
            _harmAnimals = SettingsUtils.LoadBoolSetting(harmAnimalsKey, defaultHarmAnimals);
            _actionPrompt = SettingsUtils.LoadBoolSetting(actionPromptKey, defaultActionPrompt);
            _difficultyIndex = SettingsUtils.LoadIntSetting(difficultyIndexKey, defaultDifficultyIndex);
            base.LoadSettings();
        }

        /// <summary>
        /// Apply all settings
        /// </summary>
        public override void ApplySettings()
        {
            // ApplyBloodAndGore();
            // ApplyHarmAnimals();
            // ApplyDifficulty();
            base.ApplySettings();
        }

        /// <summary>
        /// Init settings
        /// </summary>
        public override void InitSettings()
        {
            base.InitSettings();
        }
    }
}