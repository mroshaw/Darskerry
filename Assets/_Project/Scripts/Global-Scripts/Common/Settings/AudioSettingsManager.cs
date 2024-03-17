using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Common.Settings
{
    public class AudioSettingsManager : BaseSettingsManager, ISettings
    {
        [Header("Audio Mixer Configuration")]
        public AudioMixer audioMixer;
        public string masterVolumeName = "MasterVolume";
        public string musicVolumeName = "MusicVolume";
        public string soundFxVolumeName = "SoundFxVolume";
        public string ambientFxVolumeName = "AmbientVolume";
        public string uiFxVolumeName = "UiFxVolume";

        [Header("Audio Volume Defaults")]
        public float defaultMasterVolume = 1.0f;
        public float defaultMusicVolume = 1.0f;
        public float defaultSoundFxVolume = 1.0f;
        public float defaultAmbientVolume = 1.0f;
        public float defaultUiFxVolume = 1.0f;

        [Header("Setting Keys")]
        public string masterVolumeKey = "MasterVolume";
        public string musicVolumeKey = "MusicVolume";
        public string soundFxVolumeKey = "SoundFxVolume";
        public string ambientVolumeKey = "AmbientVolume";
        public string uiFxVolumeKey = "UiFxVolume";

        public float MasterVolume {get; set;}
        public float MusicVolume { get; set; }
        public float SoundFxVolume { get; set; }
        public float AmbientVolume { get; set; }
        public float UiFxVolume { get; set; }

        /// <summary>
        /// Save settings to Player Prefs
        /// </summary>
        public override void SaveSettings()
        {
            SettingsUtils.SaveFloatSetting(masterVolumeKey, MasterVolume);
            SettingsUtils.SaveFloatSetting(musicVolumeKey, MusicVolume);
            SettingsUtils.SaveFloatSetting(soundFxVolumeKey, SoundFxVolume);
            SettingsUtils.SaveFloatSetting(ambientVolumeKey, AmbientVolume);
            SettingsUtils.SaveFloatSetting(uiFxVolumeKey, UiFxVolume);
            
            base.SaveSettings();
        }

        /// <summary>
        /// Load settings from Player prefs
        /// </summary>
        public override void LoadSettings()
        {
            MasterVolume = CheckVolume(SettingsUtils.LoadFloatSetting(masterVolumeKey, defaultMasterVolume));
            MusicVolume = CheckVolume(SettingsUtils.LoadFloatSetting(musicVolumeKey, defaultMusicVolume));
            SoundFxVolume = CheckVolume(SettingsUtils.LoadFloatSetting(soundFxVolumeKey, defaultSoundFxVolume));
            AmbientVolume = CheckVolume(SettingsUtils.LoadFloatSetting(ambientVolumeKey, defaultAmbientVolume));
            UiFxVolume = CheckVolume(SettingsUtils.LoadFloatSetting(uiFxVolumeKey, defaultUiFxVolume));
            
            base.LoadSettings();
        }

        /// <summary>
        /// Apply all current settings
        /// </summary>
        public override void ApplySettings()
        {
            // ApplyMasterVolume();
            ApplyMusicVolume();
            ApplySoundFxVolume();
            ApplyAmbientVolume();
            ApplyUiFxVolume();
            
            base.ApplySettings();
        }
        
        /// <summary>
        /// Check that volume is between 0 and 1
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        private float CheckVolume(float volume)
        {
            if (volume < 0.0f)
            {
                return 0.0f;
            }

            if(volume > 1.0f)
            {
                return 1.0f;
            }

            return volume;
        }


        /// <summary>
        /// Init lists or arrays
        /// </summary>
        public override void InitSettings()
        {
            base.InitSettings();
        }
        
        /// <summary>
        /// Set the Master Volume
        /// </summary>
        /// <param name="volumeToSet"></param>
        public void SetMasterVolume(float volumeToSet)
        {
            MasterVolume = volumeToSet;
            ApplyMasterVolume();
        }

        /// <summary>
        /// Set the Music Volume
        /// </summary>
        /// <param name="volumeToSet"></param>
        public void SetMusicVolume(float volumeToSet)
        {
            MusicVolume = volumeToSet;
            ApplyMusicVolume();
        }

        /// <summary>
        /// Set the Sound FX volume
        /// </summary>
        /// <param name="volumeToSet"></param>
        public void SetSoundFxVolume(float volumeToSet)
        {
            SoundFxVolume = volumeToSet;
            ApplySoundFxVolume();
        }

        /// <summary>
        /// Set the current Ambient FX volume
        /// </summary>
        /// <param name="volumeToSet"></param>
        public void SetAmbientVolume(float volumeToSet)
        {
            AmbientVolume = volumeToSet;
            ApplyAmbientVolume();
        }

        /// <summary>
        /// Set the UI FX volume level
        /// </summary>
        /// <param name="volumeToSet"></param>
        public void SetUiFxVolume(float volumeToSet)
        {
            UiFxVolume = volumeToSet;
            ApplyUiFxVolume();
        }

          /// <summary>
        /// Apply the current Master Volume
        /// </summary>
        private void ApplyMasterVolume()
        {
            audioMixer.SetFloat(masterVolumeName, SliderToMixer(MasterVolume));
        }

        /// <summary>
        /// Apply the current Sound FX Volume
        /// </summary>
        private void ApplySoundFxVolume()
        {
            audioMixer.SetFloat(soundFxVolumeName, SliderToMixer(SoundFxVolume));
        }

        /// <summary>
        /// Apply the current Music Volume
        /// </summary>
        private void ApplyMusicVolume()
        {
            audioMixer.SetFloat(musicVolumeName, SliderToMixer(MusicVolume));
        }

        /// <summary>
        /// Apply the current Ambient FX Volume
        /// </summary>
        private void ApplyAmbientVolume()
        {
            audioMixer.SetFloat(ambientFxVolumeName, SliderToMixer(AmbientVolume));
        }

        private void ApplyUiFxVolume()
        {
            audioMixer.SetFloat(uiFxVolumeName, SliderToMixer(UiFxVolume));
        }

        /// <summary>
        /// Returns an AudioMixer "-80 to 20" value equivalent to a "0-1" float slider value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float SliderToMixer(float value)
        {
            return Mathf.Log10(value) * 20;
        }

        /// <summary>
        /// Returns a "0-1" float slider value equivalent to a "-80 to 20" AudioMixer value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float MixerToSlider(float value)
        {
            return (Remap(value, -80f, 0f, 0f, 1f));
        }

        /// <summary>
        /// Map a mixer audio volume to a float range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min1"></param>
        /// <param name="max1"></param>
        /// <param name="min2"></param>
        /// <param name="max2"></param>
        /// <returns></returns>
        float Remap(float value, float min1, float max1, float min2, float max2)
        {
            return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }
    }
}