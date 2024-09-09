using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.UserInterface.Settings
{
    public class GameplaySettingsManager : BaseSettingsManager, ISettings
    {
        [BoxGroup("Defaults")] public bool defaultBloodAndGore = true;
        [BoxGroup("Defaults")] public bool defaultHarmAnimals = false;
        [BoxGroup("Defaults")] public bool defaultActionPrompt = true;
        [BoxGroup("Defaults")] public int defaultDifficultyIndex = 2;
        
        [BoxGroup("Setting Keys")] public string bloodAndGoreKey = "BloodAndGore";
        [BoxGroup("Setting Keys")] public string harmAnimalsKey = "HarmAnimals";
        [BoxGroup("Setting Keys")] public string actionPromptKey = "ActionPrompt";
        [BoxGroup("Setting Keys")] public string difficultyIndexKey = "Difficulty";

        // Public properties
        public bool BloodAndGore { get; set; }
        public bool HarmAnimals { get; set; }
        public bool ActionPrompt { get; set; }
        public int DifficultyIndex { get; set;  }

        /// <summary>
        /// Save all settings
        /// </summary>
        public override void SaveSettings()
        {
            SettingsUtils.SaveBoolSetting(bloodAndGoreKey,BloodAndGore);
            SettingsUtils.SaveBoolSetting(harmAnimalsKey, HarmAnimals);
            SettingsUtils.SaveBoolSetting(actionPromptKey, ActionPrompt);
            SettingsUtils.SaveIntSetting(difficultyIndexKey, DifficultyIndex);
            base.SaveSettings();
        }

        /// <summary>
        /// Load all settings
        /// </summary>
        public override void LoadSettings()
        {
            BloodAndGore = SettingsUtils.LoadBoolSetting(bloodAndGoreKey, defaultBloodAndGore);
            HarmAnimals = SettingsUtils.LoadBoolSetting(harmAnimalsKey, defaultHarmAnimals);
            ActionPrompt = SettingsUtils.LoadBoolSetting(actionPromptKey, defaultActionPrompt);
            DifficultyIndex = SettingsUtils.LoadIntSetting(difficultyIndexKey, defaultDifficultyIndex);
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

        }
    }
}