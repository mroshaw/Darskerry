using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Editor.UiThemes
{
    public class UiThemeEditor : OdinEditorWindow
    {
        [Header("UI Theme Settings")]
        [SerializeField]
        public UiThemeEditorSettings themeSettings;

        [Header("Target UI Game Object")]
        public GameObject targetUiGameObject;

        public bool reportOnly = false;

        [Multiline(10)]
        [PropertyOrder(2)]
        [Tooltip("Summary reporting data will be shown here. Refer to the console for more detailed output.")]
        public string outputArea = "";

        [MenuItem("Window/UI Config/Apply UI Theme")]
        public static void ShowWindow()
        {
            GetWindow(typeof(UiThemeEditor));
        }

        /// <summary>
        /// Configure button
        /// </summary>
        [Button("Configure")]
        [Tooltip("Run the editor configuration process.")]
        private void ConfigureClick()
        {
            if (!themeSettings)
            {
                Debug.LogError("Please load a theme settings file!");
                return;
            }

            if(!targetUiGameObject)
            {
                Debug.LogError("Please select a root UI based Game Object to process.");
                return;
            }

            ApplyTheme();
        }

        /// <summary>
        /// Apply the selected Theme scriptable object
        /// to all Themeable UI
        /// </summary>
        private void ApplyTheme()
        {
            int count = 0;

            // Find all Themable UI elements
            Button[] allButtons = targetUiGameObject.GetComponentsInChildren<Button>(true);
            Toggle[] allToggles = targetUiGameObject.GetComponentsInChildren<Toggle>(true);
            TMP_Dropdown[] allDropdowns = targetUiGameObject.GetComponentsInChildren<TMP_Dropdown>(true);
            Slider[] allSliders = targetUiGameObject.GetComponentsInChildren<Slider>(true);

            ColorBlock newButtonColourBlock = new ColorBlock
            {
                normalColor = themeSettings.buttonNormalColour,
                selectedColor = themeSettings.buttonSelectedColour,
                disabledColor = themeSettings.buttonDisabledColour,
                pressedColor = themeSettings.buttonPressedColour,
                highlightedColor = themeSettings.buttonHighlightedColour,
                colorMultiplier = themeSettings.buttonColourMultiplier,
                fadeDuration = themeSettings.buttonFadeDuration
            };

            // Iterate over buttons
            foreach (Button currentButton in allButtons)
            {
                GameObject buttonGameObject = currentButton.gameObject;

                if (!reportOnly)
                {
                    currentButton.colors = newButtonColourBlock;
                    currentButton.GetComponent<Image>().sprite = themeSettings.buttonSourceImage;
                    TextMeshProUGUI buttonText = buttonGameObject.GetComponentInChildren<TextMeshProUGUI>();
                    if(!buttonText)
                    {
                        Debug.LogError($"Error: Button {currentButton.name} should contain a TextMeshPro control!");
                        continue;
                    }

                    buttonText.font = themeSettings.buttonFont;
                    buttonText.fontSize = themeSettings.buttonFontSize;
                    buttonText.color = themeSettings.buttonFontColour;
                }
                outputArea += $"\nButton {buttonGameObject.name} processed: ";
            }

            // Iterate over drop downs
            foreach (TMP_Dropdown currentDropdown in allDropdowns)
            {
                GameObject dropdownGameObject = currentDropdown.gameObject;

                if (!reportOnly)
                {
                    currentDropdown.colors = newButtonColourBlock;
                }
                outputArea += $"\nDropdown {dropdownGameObject.name} processed: ";
            }

            // Iterate over toggles
            foreach (Toggle currentToggle in allToggles)
            {
                GameObject toggleGameObject = currentToggle.gameObject;

                if (!reportOnly)
                {
                    currentToggle.colors = newButtonColourBlock;
                }
                outputArea += $"\nToggle {toggleGameObject.name} processed: ";
            }

            // Iterate over sliders
            foreach (Slider currentSlider in allSliders)
            {
                GameObject sliderGameObject = currentSlider.gameObject;

                if (!reportOnly)
                {
                    currentSlider.colors = newButtonColourBlock;
                }
                outputArea += $"\nSlider {sliderGameObject.name} processed: ";
            }

            count++;
            outputArea += $"\n{count} processed.";
        }
    }
}