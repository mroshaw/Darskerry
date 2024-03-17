using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Editor.UiThemes
{
    [CreateAssetMenu(fileName = "UiSoundEditorSettings", menuName = "Settings/User Interface/UiSound", order = 2)]
    public class UiSoundEditorSettings : ScriptableObject
    {
        [Header("Audio Settings")]
        public AudioMixerGroup uiSoundAudioMixerGroup;

        [Header("Button Sounds")]
        public AudioClip clickSound;
        public string[] buttonClickIdentifiers;
        public AudioClip cancelClickSound;
        public string[] cancelButtonClickIdentifiers;
        public AudioClip bigClickSound;
        public string[] bigButtonClickIdentifiers;
        public AudioClip backClickSound;
        public string[] backButtonClickIdentifiers;

        [Header("Drop Down Sounds")]
        public AudioClip dropDownClickSound;
        public string[] dropDownClickIdentifiers;

        [Header("Slider Sounds")]
        public AudioClip sliderClickSound;
        public string[] sliderClickIdentifiers;

        [Header("Toggle Sounds")]
        public AudioClip toggleClickSound;
        public string[] toggleClickIdentifiers;

    }
}