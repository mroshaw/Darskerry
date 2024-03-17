using System;
using UnityEngine;

namespace DaftAppleGames.Common.Settings
{
    public class DisplaySettingsManager : BaseSettingsManager, ISettings
    {
        [Header("Screen Defaults")]
        public float defaultScreenBrightness = 1.0f;
        public bool defaultFullScreen = true;
        public int defaultDisplayResIndex = 0;

        [Header("Setting Keys")]
        public string screenBrightnessKey = "ScreenBrightness";
        public string fullScreenKey = "FullScreen";
        public string displayResIndexKey = "ScreenResolution";

        private float _screenBrightness;
        private bool _fullScreen;
        private int _displayResIndex;
        private Resolution[] _displayResolutionArray;

        // Public properties
        public float ScreenBrightness
        {
            get => _screenBrightness;
            set => _screenBrightness = value;
        }

        public bool FullScreen
        {
            get => _fullScreen;
            set => _fullScreen = value;
        }

        public int DisplayResIndex
        {
            get => _displayResIndex;
            set => _displayResIndex = value;
        }

        public Resolution[] DisplayResArray
        {
            get => _displayResolutionArray;
            set => _displayResolutionArray = value;
        }
    
        /// <summary>
        /// Save settings to Player Prefs
        /// </summary>
        public override void SaveSettings()
        {
            PlayerPrefs.SetFloat(screenBrightnessKey, _screenBrightness);
            PlayerPrefs.SetInt(fullScreenKey, Convert.ToInt32(_fullScreen));
            PlayerPrefs.SetInt(displayResIndexKey, _displayResIndex);
            
            base.SaveSettings();
        }

        /// <summary>
        /// Load settings from Player Prefs
        /// </summary>
        public override void LoadSettings()
        {
            _screenBrightness = SettingsUtils.LoadFloatSetting(screenBrightnessKey, defaultScreenBrightness);
            _fullScreen = SettingsUtils.LoadBoolSetting(fullScreenKey, defaultFullScreen);
            _displayResIndex = SettingsUtils.LoadIntSetting(displayResIndexKey, GetCurrentResolutionIndex());
            
            base.LoadSettings();
        }
        
        /// <summary>
        /// Apply all current settings
        /// </summary>
        public override void ApplySettings()
        {
            ApplyScreenBrightness();
            ApplyFullScreen();
            ApplyDisplayResolution();
            
            base.ApplySettings();
        }
        
        /// <summary>
        /// Setup any lists or arrays
        /// </summary>
        public override void InitSettings()
        {
            // Populate the "Screen Resolution" list
            PopulateDisplayResolutions();
            
            base.InitSettings();
        }

        /// <summary>
        /// Populate the list of available screen resolutions
        /// </summary>
        private void PopulateDisplayResolutions()
        {
            _displayResolutionArray = Screen.resolutions;
        }

        /// <summary>
        /// Returns the index of the current, active screen resolution
        /// </summary>
        /// <returns></returns>
        public int GetCurrentResolutionIndex()
        {
            for (int currRes = 0; currRes < _displayResolutionArray.Length; currRes++)
            {
                if (_displayResolutionArray[currRes].width == Screen.currentResolution.width &&
                    _displayResolutionArray[currRes].height == Screen.currentResolution.height &&
                    _displayResolutionArray[currRes].refreshRate == Screen.currentResolution.refreshRate)
                {
                    return currRes;
                }
            }
            return defaultDisplayResIndex;
        }
        
        /// <summary>
        /// Update and apply Display Resolution
        /// </summary>
        /// <param name="displayResIndexToSet"></param>
        public void SetDisplayResIndex(int displayResIndexToSet)
        {
            _displayResIndex = displayResIndexToSet;
            ApplyDisplayResolution();
        }

        /// <summary>
        /// Update and apply Screen Brightness
        /// </summary>
        /// <param name="screenBrightnessToSet"></param>
        public void SetScreenBrightness(float screenBrightnessToSet)
        {
            _screenBrightness = screenBrightnessToSet;
            ApplyScreenBrightness();
        }

        /// <summary>
        /// Update and apply Full Screen
        /// </summary>
        /// <param name="fullScreenToSet"></param>
        public void SetFullScreen(bool fullScreenToSet)
        {
            _fullScreen = fullScreenToSet;
            ApplyFullScreen();
        }

        /// <summary>
        /// Apply current Screen Brightness
        /// </summary>
        private void ApplyScreenBrightness()
        {

        }

        /// <summary>
        /// Apply current Full Screen
        /// </summary>
        private void ApplyFullScreen()
        {
            Screen.fullScreen = _fullScreen;
        }

        /// <summary>
        /// Applys the current Display Resolution settings
        /// </summary>
        private void ApplyDisplayResolution()
        {
            Resolution currentResolution = _displayResolutionArray[_displayResIndex];
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen, currentResolution.refreshRate);
        }
    }
}