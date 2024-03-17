using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Settings
{
    public class GameplaySettingsUiWindow : SettingsUiWindow, ISettingsUiWindow
    {
        [Header("UI Configuration")]
        public Toggle bloodAndGoreToggle;
        public Toggle harmAnimalsToggle;
        public Toggle actionPromptToggle;
        public TMP_Dropdown difficultyDropdown;
        
        [Header("Settings Model")]
        public GameplaySettingsManager gameplaySettingsManager;

        /// <summary>
        /// Configure the UI control handlers to call public methods
        /// </summary>
        public override void InitControls()
        {
            // Remove all listeners, to prevent doubling up.
            bloodAndGoreToggle.onValueChanged.RemoveAllListeners();
            harmAnimalsToggle.onValueChanged.RemoveAllListeners();
            actionPromptToggle.onValueChanged.RemoveAllListeners();
            difficultyDropdown.onValueChanged.RemoveAllListeners();
            
            // Configure the Gameplay setting controls
            bloodAndGoreToggle.onValueChanged.AddListener(UpdateBloodAndGore);
            harmAnimalsToggle.onValueChanged.AddListener(UpdateHarmAnimals);
            actionPromptToggle.onValueChanged.AddListener(UpdateActionPrompt);
            difficultyDropdown.onValueChanged.AddListener(UpdateDifficulty);
        }

        /// <summary>
        /// Initialise the controls with current settings
        /// </summary>
        public override void RefreshControlState()
        {
            base.RefreshControlState();
            bloodAndGoreToggle.SetIsOnWithoutNotify(gameplaySettingsManager.BloodAndGore);
            harmAnimalsToggle.SetIsOnWithoutNotify(gameplaySettingsManager.HarmAnimals);
            actionPromptToggle.SetIsOnWithoutNotify(gameplaySettingsManager.ActionPrompt);
            difficultyDropdown.SetValueWithoutNotify(gameplaySettingsManager.DifficultyIndex);
        }

        /// <summary>
        /// UI controller method to manage "Blood and Gore" UI changes
        /// </summary>
        /// <param name="bloodAndGoreValue"></param>
        public void UpdateBloodAndGore(bool bloodAndGoreValue)
        {
            gameplaySettingsManager.BloodAndGore = bloodAndGoreValue;
        }

        /// <summary>
        /// Manage "Ham Animals" UI changes
        /// </summary>
        /// <param name="harmAnimalsValue"></param>
        public void UpdateHarmAnimals(bool harmAnimalsValue)
        {
            gameplaySettingsManager.HarmAnimals = harmAnimalsValue;
        }
        
        /// <summary>
        /// Handle "Action Prompt" UI changes
        /// </summary>
        /// <param name="actionPromptValue"></param>
        public void UpdateActionPrompt(bool actionPromptValue)
        {
            gameplaySettingsManager.ActionPrompt = actionPromptValue;
        }
        
        /// <summary>
        /// Manage "Difficulty" UI changes
        /// </summary>
        /// <param name="difficultyValue"></param>
        public void UpdateDifficulty(int difficultyValue)
        {
            gameplaySettingsManager.DifficultyIndex = difficultyValue;
        }
    }
}