using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    [CreateAssetMenu(fileName = "UiTheme", menuName = "Daft Apple Games/Ui/UiTheme")]
    public class UiTheme : ScriptableObject
    {
        [BoxGroup("Appearance")] public Color backgroundColour;
        [BoxGroup("Appearance")] public Sprite backgroundImageSprite;

        [BoxGroup("Text")] public Color textColour;
        [BoxGroup("Text")] public int fontSize;
        [BoxGroup("Text")] public TMP_FontAsset textFont;

        [BoxGroup("Audio")] public AudioClip clickAudio;

    }
}