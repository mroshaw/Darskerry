using UnityEngine;

namespace DaftAppleGames.Common.Settings
{
    public class GameSettingsManager : MonoBehaviour
    {
        private static AudioSettingsManager _audioSettingsManager;
        private static DisplaySettingsManager _displaySettingsManager;
        private static GameplaySettingsManager _gameplaySettingsManager;
        private static PerformanceSettingsManager _performanceSettingsManager;

        public AudioSettingsManager AudioSettings => _audioSettingsManager;
        public DisplaySettingsManager DisplaySettings => _displaySettingsManager;
        public GameplaySettingsManager GameplaySettings => _gameplaySettingsManager;
        public PerformanceSettingsManager PerformanceSettings => _performanceSettingsManager;

        // Singleton static instance
        private static GameSettingsManager _instance;
        public static GameSettingsManager Instance => _instance;

        /// <summary>
        /// Initialise setting controllers
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            
            _audioSettingsManager = GetComponentInChildren<AudioSettingsManager>(true);
            _displaySettingsManager = GetComponentInChildren<DisplaySettingsManager>(true);
            _gameplaySettingsManager = GetComponentInChildren<GameplaySettingsManager>(true);
            _performanceSettingsManager = GetComponentInChildren<PerformanceSettingsManager>(true);
        }

        /// <summary>
        /// Save all settings
        /// </summary>
        public void SaveSettings()
        {
            _audioSettingsManager.SaveSettings();
            _displaySettingsManager.SaveSettings();
            _gameplaySettingsManager.SaveSettings();
            _performanceSettingsManager.SaveSettings();
        }

        /// <summary>
        /// Load all settings
        /// </summary>
        public void LoadSettings()
        {
            _audioSettingsManager.LoadSettings();
            _displaySettingsManager.LoadSettings();
            _gameplaySettingsManager.LoadSettings();
            _performanceSettingsManager.LoadSettings();
        }

        /// <summary>
        /// Apply all settings
        /// </summary>
        public void ApplySettings()
        {
            _audioSettingsManager.ApplySettings();
            _displaySettingsManager.ApplySettings();
            _gameplaySettingsManager.ApplySettings();
            _performanceSettingsManager.ApplySettings();
        }

        /// <summary>
        /// Load then apply settings
        /// </summary>
        public void LoadAndApplySettings()
        {
            LoadSettings();
            ApplySettings();
        }
    }
}