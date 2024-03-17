using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Audio
{
    public class UiAudioController : MonoBehaviour
    {
        [BoxGroup("UI Audio Settings")]
        public UiSoundSettings uiSoundSettings;

        private AudioSource _audioSource;
        
        // Singleton static instance
        private static UiAudioController _instance;
        public static UiAudioController Instance => _instance;

        /// <summary>
        /// Initialise the GameController Singleton instance
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                _audioSource = this.gameObject.AddComponent<AudioSource>();
                _audioSource.loop = false;
                _audioSource.playOnAwake = false;
                _audioSource.outputAudioMixerGroup = uiSoundSettings.mixerGroup;
            }
        }

        /// <summary>
        /// Play basic clip
        /// </summary>
        public static void PlayClick()
        {
            _instance._audioSource.PlayOneShot(_instance.uiSoundSettings.clickClip);
        }

        /// <summary>
        /// Play "big" clip
        /// </summary>
        public static void PlayBig()
        {
            _instance._audioSource.PlayOneShot(_instance.uiSoundSettings.bigClickClip);
        }
        
        /// <summary>
        /// Play back clip
        /// </summary>
        public static void PlayBack()
        {
            _instance._audioSource.PlayOneShot(_instance.uiSoundSettings.backClip);
        }

        /// <summary>
        /// Play cancel clip
        /// </summary>
        public static void PlayCancel()
        {
            _instance._audioSource.PlayOneShot(_instance.uiSoundSettings.cancelClip);
        }
        
        /// <summary>
        /// Play posiutive clip
        /// </summary>
        public static void PlayPositive()
        {
            _instance._audioSource.PlayOneShot(_instance.uiSoundSettings.positiveClip);
        }
    }
}
