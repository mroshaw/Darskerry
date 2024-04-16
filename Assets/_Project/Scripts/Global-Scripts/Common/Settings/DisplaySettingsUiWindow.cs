using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Settings
{
    public class DisplaySettingsUiWindow : SettingsUiWindow, ISettingsUiWindow
    {
        [BoxGroup("UI Configuration")] public Toggle fullScreenToggle;
        [BoxGroup("UI Configuration")] public Slider brightnessSlider;
        [BoxGroup("UI Configuration")] public TMP_Dropdown displayResDropDown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown qualityPresetDropdown;
        [BoxGroup("UI Configuration")] public Toggle enableVSyncToggle;
        [BoxGroup("UI Configuration")] public TMP_Dropdown resolutionConfigurationDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown dlssQualityDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown fsrQualityDropdown;

        [BoxGroup("Settings Model")] public DisplaySettingsManager displaySettingsManager;

        /// <summary>
        /// Configure the UI control handlers to call public methods
        /// </summary>
        public override void InitControls()
        {
            // Remove all listeners, to prevent doubling up.
            fullScreenToggle.onValueChanged.RemoveListener(UpdateFullScreen);
            brightnessSlider.onValueChanged.RemoveListener(UpdateBrightness);
            displayResDropDown.onValueChanged.RemoveListener(UpdateDisplayResolution);
            enableVSyncToggle.onValueChanged.RemoveListener(UpdateVSync);
            resolutionConfigurationDropdown.onValueChanged.RemoveListener(UpdateResolutionConfiguration);
            dlssQualityDropdown.onValueChanged.RemoveListener(UpdateDlssQuality);
            fsrQualityDropdown.onValueChanged.RemoveListener(UpdateFsrQuality);
            qualityPresetDropdown.onValueChanged.RemoveListener(UpdateQualityPreset);

            // Configure the Audio setting sliders
            fullScreenToggle.onValueChanged.AddListener(UpdateFullScreen);
            brightnessSlider.onValueChanged.AddListener(UpdateBrightness);
            displayResDropDown.onValueChanged.AddListener(UpdateDisplayResolution);
            enableVSyncToggle.onValueChanged.AddListener(UpdateVSync);
            resolutionConfigurationDropdown.onValueChanged.AddListener(UpdateResolutionConfiguration);
            dlssQualityDropdown.onValueChanged.AddListener(UpdateDlssQuality);
            fsrQualityDropdown.onValueChanged.AddListener(UpdateFsrQuality);
            qualityPresetDropdown.onValueChanged.AddListener(UpdateQualityPreset);

            // Populate drop downs
            PopulateDisplayResDropDown();
            InitQualitySettings();
        }

        /// <summary>
        /// Populate Quality Drop Down from Quality Settings
        /// </summary>
        private void InitQualitySettings()
        {
            qualityPresetDropdown.ClearOptions();
            List<string> qualityOptions = new List<string>(QualitySettings.names);
            qualityPresetDropdown.AddOptions(qualityOptions);
        }


        /// <summary>
        /// Populate the Display Resolutions drop down with available resolutions
        /// </summary>
        private void PopulateDisplayResDropDown()
        {
            Resolution[] displayResolutionArray = displaySettingsManager.DisplayResArray;
            List<string> displayResOptions = new List<string>();

            for (int currRes = 0; currRes < displayResolutionArray.Length; currRes++)
            {
                // Add the resolution option
                string option = $"{displayResolutionArray[currRes].width}x{displayResolutionArray[currRes].height}";
                displayResOptions.Add(option);
            }

            // Configure the Drop Down
            displayResDropDown.ClearOptions();
            displayResDropDown.AddOptions(displayResOptions);
            displayResDropDown.SetValueWithoutNotify(displaySettingsManager.GetCurrentResolutionIndex());
            displayResDropDown.RefreshShownValue();
        }

        /// <summary>
        /// Initialise the controls with current settings
        /// </summary>
        public override void RefreshControlState()
        {
            base.RefreshControlState();
            displayResDropDown.SetValueWithoutNotify(displaySettingsManager.GetCurrentResolutionIndex());
            fullScreenToggle.SetIsOnWithoutNotify(displaySettingsManager.FullScreen);
            brightnessSlider.SetValueWithoutNotify(displaySettingsManager.ScreenBrightness);
            enableVSyncToggle.SetIsOnWithoutNotify(displaySettingsManager.VSync);
            resolutionConfigurationDropdown.SetValueWithoutNotify(displaySettingsManager.ResolutionConfigurationIndex);
            dlssQualityDropdown.SetValueWithoutNotify(displaySettingsManager.DlssQualityIndex);
            fsrQualityDropdown.SetValueWithoutNotify(displaySettingsManager.FsrQualityIndex);
            int qualityIndex = qualityPresetDropdown.options.FindIndex(i => i.text == displaySettingsManager.QualityPresetName);
            qualityPresetDropdown.SetValueWithoutNotify(qualityIndex);
            UpdateResConfigControlState();
        }

        /// <summary>
        /// Handle Quality Preset value changed
        /// </summary>
        /// <param name="qualityPresetIndex"></param>
        public void UpdateQualityPreset(int qualityPresetIndex)
        {
            string qualityName = qualityPresetDropdown.options[qualityPresetIndex].text;
            displaySettingsManager.SetQualityPreset(qualityName);
        }

        public void UpdateResolutionConfiguration(int indexValue)
        {
            displaySettingsManager.SetResolutionConfiguration(indexValue);
            UpdateResConfigControlState();
        }

        public void UpdateDlssQuality(int indexValue)
        {
            displaySettingsManager.SetDlssQuality(indexValue);
        }

        public void UpdateFsrQuality(int indexValue)
        {
            displaySettingsManager.SetFsrQuality(indexValue);
        }

        /// <summary>
        /// Sets the dynamic resolution quality control states
        /// </summary>
        private void UpdateResConfigControlState()
        {
            switch ((DisplaySettingsManager.DynamicResolutionType)displaySettingsManager.ResolutionConfigurationIndex)
            {
                case DisplaySettingsManager.DynamicResolutionType.None:
                    dlssQualityDropdown.interactable = false;
                    fsrQualityDropdown.interactable = false;
                    break;
                case DisplaySettingsManager.DynamicResolutionType.DLSS:
                    dlssQualityDropdown.interactable = true;
                    fsrQualityDropdown.interactable = false;
                    break;
                case DisplaySettingsManager.DynamicResolutionType.FSR:
                    dlssQualityDropdown.interactable = false;
                    fsrQualityDropdown.interactable = true;
                    break;
            }
        }


        /// <summary>
        /// UI controller method to manage "Full Screen" toggle"
        /// </summary>
        /// <param name="fullScreenValue"></param>
        public void UpdateFullScreen(bool fullScreenValue)
        {
            displaySettingsManager.SetFullScreen(fullScreenValue);
        }

        /// <summary>
        /// Handle VSync toggle changed
        /// </summary>
        /// <param name="isEnabled"></param>
        public void UpdateVSync(bool isEnabled)
        {
            displaySettingsManager.SetVSync(isEnabled);
        }


        public void UpdateBrightness(float brightnessValue)
        {
 
        }

        /// <summary>
        /// UI controller method to handle change to Display Resolution drop down
        /// </summary>
        /// <param name="displayResIndexValue"></param>
        public void UpdateDisplayResolution(int displayResIndexValue)
        {
            displaySettingsManager.SetDisplayResIndex(displayResIndexValue);
        }
    }
}