using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Editor.UiThemes
{
    [CreateAssetMenu(fileName = "UiThemeEditorSettings", menuName = "Settings/User Interface/UiTheme", order = 1)]
    public class UiThemeEditorSettings : ScriptableObject
    {
        [Header("Button")]
        public Sprite buttonSourceImage;
        public Color buttonNormalColour;
        public Color buttonHighlightedColour;
        public Color buttonPressedColour;
        public Color buttonSelectedColour;
        public Color buttonDisabledColour;
        public float buttonColourMultiplier;
        public float buttonFadeDuration;
        public int buttonFontSize;
        public TMP_FontAsset buttonFont;
        public Color buttonFontColour;

        [Header("Slider")]
        public Color sliderColour;
    }
}