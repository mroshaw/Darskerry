using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Settings
{
    public class AudioSettingsUiWindow : SettingsUiWindow, ISettingsUiWindow
    {
        [Header("UI Configuration")]
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider soundFxVolumeSlider;
        public Slider ambientFxVolumeSlider;
        public Slider uiFxVolumeSlider;

        [Header("Settings Model")]
        public AudioSettingsManager audioSettingsManager;
        
        /// <summary>
        /// Configure the UI control handlers to call public methods
        /// </summary>
        public override void InitControls()
        {
            // Remove all listeners, to prevent doubling up.
            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            soundFxVolumeSlider.onValueChanged.RemoveAllListeners();
            ambientFxVolumeSlider.onValueChanged.RemoveAllListeners();
            uiFxVolumeSlider.onValueChanged.RemoveAllListeners();

            // Configure the Audio setting sliders
            masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
            soundFxVolumeSlider.onValueChanged.AddListener(UpdateSoundFxVolume);
            ambientFxVolumeSlider.onValueChanged.AddListener(UpdateAmbientVolume);
            uiFxVolumeSlider.onValueChanged.AddListener(UpdateUiFxVolume);
        }

        /// <summary>
        /// Initiatlise the controls with current settings
        /// </summary>
        public override void RefreshControlState()
        {
            base.RefreshControlState();
            masterVolumeSlider.SetValueWithoutNotify(audioSettingsManager.MasterVolume);
            musicVolumeSlider.SetValueWithoutNotify(audioSettingsManager.MusicVolume);
            soundFxVolumeSlider.SetValueWithoutNotify(audioSettingsManager.SoundFxVolume);
            ambientFxVolumeSlider.SetValueWithoutNotify(audioSettingsManager.AmbientVolume);
            uiFxVolumeSlider.SetValueWithoutNotify(audioSettingsManager.UiFxVolume);
        }

        /// <summary>
        /// UI controller method to manage "Master Volume" UI changes
        /// </summary>
        /// <param name="masterVolumeValue"></param>
        public void UpdateMasterVolume(float masterVolumeValue)
        {
            audioSettingsManager.SetMasterVolume(masterVolumeValue);
        }

        /// <summary>
        /// Manager updating "Music Volume"
        /// </summary>
        /// <param name="musicVolumeValue"></param>
        public void UpdateMusicVolume(float musicVolumeValue)
        {
            audioSettingsManager.SetMusicVolume(musicVolumeValue);
        }

        /// <summary>
        /// Manage updating "SoundFX Volume"
        /// </summary>
        /// <param name="soundFxVolumeValue"></param>
        public void UpdateSoundFxVolume(float soundFxVolumeValue)
        {
            audioSettingsManager.SetSoundFxVolume(soundFxVolumeValue);
        }

        /// <summary>
        /// Manage updating "Ambient FX Volume"
        /// </summary>
        /// <param name="ambientVolumeValue"></param>
        public void UpdateAmbientVolume(float ambientVolumeValue)
        {
            audioSettingsManager.SetAmbientVolume(ambientVolumeValue);
        }

        /// <summary>
        /// Manage updating "UI FX Volume"
        /// </summary>
        /// <param name="uiFxVolumeValue"></param>
        public void UpdateUiFxVolume(float uiFxVolumeValue)
        {
            audioSettingsManager.SetUiFxVolume(uiFxVolumeValue);
        }
    }
}