using System.Collections;
using UnityEngine;

namespace DaftAppleGames.Common.Audio
{
    public class AudioSourceFader : MonoBehaviour
    {
        // Public serializable properties
        [Header("General Settings")]
        public bool fadeInOnAwake = false;
        public bool fadeInOnStart = true;
        public float fadeInDuration = 3.0f;
        public float delayBeforeFade = 2.0f;
        
        // Private fields
        private AudioSource _audioSource;
        private float _targetVolume;
        
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _targetVolume = _audioSource.volume;
            
            _audioSource.playOnAwake = false;
            _audioSource.volume = 0.0f;
            
            if (fadeInOnAwake)
            {
                FadeInAudioSource();
            }
        }
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            if (fadeInOnStart)
            {
                FadeInAudioSource();
            }
        }

        /// <summary>
        /// Fade in AudiSource
        /// </summary>
        private void FadeInAudioSource()
        {
            StartCoroutine(FadeInAsync());
        }

        /// <summary>
        /// Async Coroutine to fade over time
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeInAsync()
        {
            // Wait for delay if given
            yield return new WaitForSeconds(delayBeforeFade);
            
            float time = 0;
            float startValue = 0;
            while (time < fadeInDuration)
            {
                _audioSource.volume = Mathf.Lerp(startValue, _targetVolume, time / fadeInDuration);
                time += Time.deltaTime;
                yield return null;
            }
            _audioSource.volume = _targetVolume;
        }
    }
}
