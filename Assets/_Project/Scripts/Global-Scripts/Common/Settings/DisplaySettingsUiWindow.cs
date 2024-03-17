using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Settings
{
    public class DisplaySettingsUiWindow : SettingsUiWindow, ISettingsUiWindow
    {
        [Header("UI Configuration")]
        public Toggle fullScreenToggle;
        public Slider brightnessSlider;
        public TMP_Dropdown displayResDropDown;

        [Header("Settings Model")]
        public DisplaySettingsManager displaySettingsManager;

        /// <summary>
        /// Configure the UI control handlers to call public methods
        /// </summary>
        public override void InitControls()
        {
            // Remove all listeners, to prevent doubling up.
            fullScreenToggle.onValueChanged.RemoveAllListeners();
            brightnessSlider.onValueChanged.RemoveAllListeners();
            displayResDropDown.onValueChanged.RemoveAllListeners();

            // Configure the Audio setting sliders
            fullScreenToggle.onValueChanged.AddListener(UpdateFullScreen);
            brightnessSlider.onValueChanged.AddListener(UpdateBrightness);
            displayResDropDown.onValueChanged.AddListener(UpdateDisplayResolution);

            // Populate drop downs
            PopulateDisplayResDropDown();
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
                string option = $"{displayResolutionArray[currRes].width}x{displayResolutionArray[currRes].height}({displayResolutionArray[currRes].refreshRate}Hz)";
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
            fullScreenToggle.SetIsOnWithoutNotify(displaySettingsManager.FullScreen);
            brightnessSlider.SetValueWithoutNotify(displaySettingsManager.ScreenBrightness);
        }

        /// <summary>
        /// UI controller method to manage "Full Screen" toggle"
        /// </summary>
        /// <param name="fullScreenValue"></param>
        public void UpdateFullScreen(bool fullScreenValue)
        {
            displaySettingsManager.SetFullScreen(fullScreenValue);
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