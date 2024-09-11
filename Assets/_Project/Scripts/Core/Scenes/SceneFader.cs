using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Scenes
{
    public enum SceneFadeDirection { FadeFromBlack, FadeToBlack }

    public class SceneFader : MonoBehaviour
    {
        [Header("Control")]
        public bool fadeInOnStart = false;
        public bool blackBackgroundOnEnable = true;

        [Header("Canvas Fade")]
        public float delayBeforeFade = 2.0f;
        public float fadeDuration = 5.0f;
        public CanvasGroup fadeCanvasGroup;

        [Header("Audio Fade")]
        public bool fadeAudio = true;
        public AudioMixer audioMixer;

        [Header("Events")]
        public UnityEvent fadeStartEvent;
        public UnityEvent fadeEndEvent;
        
        private bool _isFading;

        private void OnEnable()
        {
            if (blackBackgroundOnEnable)
            {
                EnableBlackCanvas();
            }
        }

        public void Awake()
        {
            // Get the Canvas group for fading
            fadeCanvasGroup = GetComponentInChildren<CanvasGroup>(true);
        }

        /// <summary>
        /// Begin fade, if enabled
        /// </summary>
        private void Start()
        {
            if (fadeInOnStart)
            {
                FadeIn();
            }
        }

        /// <summary>
        /// Hide the scene behind a black canvas
        /// </summary>
        private void EnableBlackCanvas()
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            fadeCanvasGroup.alpha = 1.0f;
        }

        /// <summary>
        /// Show the scene, remove the black canvas
        /// </summary>
        private void DisableBlackCanvas()
        {
            fadeCanvasGroup.gameObject.SetActive(false);
            fadeCanvasGroup.alpha = 1.0f;
        }

        /// <summary>
        /// Can be called by an event to mute audio, prior to fading.
        /// For example, prior to loading new scenes.
        /// </summary>
        public void MuteAudio()
        {
            audioMixer.SetFloat("MasterVolume", -80.0f);
        }

        public void FadeIn()
        {
            StartCoroutine(FadeASync(SceneFadeDirection.FadeFromBlack));
        }
        
        public void FadeOut()
        {
            StartCoroutine(FadeASync(SceneFadeDirection.FadeToBlack));
        }

        private IEnumerator FadeASync(SceneFadeDirection fadeDirection)
        {
            // Call any event listeners
            fadeStartEvent.Invoke();

            // Determine start and end values
            float startAudioVolume;
            float endAudioVolume;
            float startScreenAlpha;
            float endScreenAlpha;

            if (fadeDirection == SceneFadeDirection.FadeFromBlack)
            {
                startAudioVolume = -80.0f;
                endAudioVolume = 0.0f;
                startScreenAlpha = 1.0f;
                endScreenAlpha = 0.0f;
            }
            else
            {
                startAudioVolume = 0f;
                endAudioVolume = -80.0f;
                startScreenAlpha = 0f;
                endScreenAlpha = 1.0f;
            }

            // Wait for the delay period
            yield return new WaitForSecondsRealtime(delayBeforeFade);
            float currTime = 0.0f;
            while (currTime < fadeDuration)
            {
                if (fadeAudio)
                {
                    // Calculate and set Audio mixer volume
                    float newVolume = Mathf.Lerp(startAudioVolume, endAudioVolume, currTime / fadeDuration);
                    audioMixer.SetFloat("MasterVolume", newVolume);
                }

                // Calculate and set Panel alpha
                float alphaValue = Mathf.Lerp(startScreenAlpha, endScreenAlpha, currTime / fadeDuration);
                fadeCanvasGroup.alpha = alphaValue;

                currTime += Time.unscaledDeltaTime;
                yield return null;
            }

            // Set final values
            if (fadeAudio)
            {
                audioMixer.SetFloat("MasterVolume", endAudioVolume);
            }
            fadeCanvasGroup.alpha = endScreenAlpha;

            // Call event listeners
            fadeEndEvent.Invoke();
        }
    }
}