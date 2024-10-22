using TMPro;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    public class UiThemeApplier : MonoBehaviour
    {
        [SerializeField] private UiTheme uiTheme;

        private AudioSource _audioSource;
        private TMP_Text _tmpText;
        private UiObject _baseUiObject;
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _baseUiObject = GetComponent<UiObject>();
            _tmpText = GetComponentInChildren<TMP_Text>();
            ApplyAppearance();
            ApplySound();
            ApplyFont();
        }

        private void ApplyAppearance()
        {
            _baseUiObject.SetBackgroundColour(uiTheme.backgroundColour);
            if (uiTheme.backgroundImageSprite)
            {
                _baseUiObject.SetBackgroundImage(uiTheme.backgroundImageSprite);
            }
        }

        private void ApplyFont()
        {
            _tmpText.color = uiTheme.textColour;
        }

        private void ApplySound()
        {
            _baseUiObject.clickedEvent.RemoveListener(PlayClickedSound);
            _baseUiObject.clickedEvent.AddListener(PlayClickedSound);
        }

        private void PlayClickedSound()
        {
            if (uiTheme)
            {
                _audioSource.PlayOneShot(uiTheme.clickAudio);
            }
        }
    }
}