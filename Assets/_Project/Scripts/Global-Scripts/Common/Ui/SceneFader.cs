using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace DaftAppleGames.Common.UI
{
    public class SceneFader : MonoBehaviour
    {
        [Header("Control")]
        public bool fadeInOnStart = true;

        [Header("Canvas Fade")]
        public float delayBeforeScreenFadeIn = 2.0f;
        public float delayBeforeScreenFadeOut = 0.0f;
        public float sceneFadeInDuration = 5.0f;
        public float sceneFadeOutDuration = 5.0f;
        public GameObject fadeCanvasGameObject;
        public CanvasGroup fadeCanvasGroup;

        [Header("Audio Fade")]
        public bool fadeAudioIn = true;
        public bool fadeAudioOut = true;
        public float delayBeforeAudioFadeIn = 0.0f;
        public float delayBeforeAudioFadeOut = 0.0f;
        public float audioFadeInDuration = 5.0f;
        public float audioFadeOutDuration = 5.0f;
        public AudioMixer audioMixer;

        [Header("Events")]
        public UnityEvent onBeforeFadeStartEvent;
        public UnityEvent onFadeStartEvent;
        public UnityEvent onFadeEndEvent;
        
        private bool _isFading = false;
        public bool IsFading { get { return _isFading; } }
        
        
        /// <summary>
        /// Register with OnSceneLoad
        /// </summary>
        public void Awake()
        {
            // Enable the black canvas
            if(fadeInOnStart)
            {
                fadeCanvasGameObject.SetActive(true);
                BlackFadeCanvas();
            }

            // Get the Canvas group for fading
            fadeCanvasGroup = GetComponentInChildren<CanvasGroup>(true);
        }

        /// <summary>
        /// Begin fade, if enabled
        /// </summary>
        private void Start()
        {
            if (fadeAudioIn)
            {
                audioMixer.SetFloat("MasterVolume", -80.0f);
            }
            else
            {
                audioMixer.SetFloat("MasterVolume", 0.0f);
            }
            
            if (fadeInOnStart)
            {
                FadeIn();
            }
        }

        /// <summary>
        /// Hide the scene behind a black canvas
        /// </summary>
        public void BlackFadeCanvas()
        {
            fadeCanvasGameObject.SetActive(true);
            fadeCanvasGroup.alpha = 0.0f;
        }

        /// <summary>
        /// Show the scene, remove the black canvas
        /// </summary>
        public void ClearFadeCanvas()
        {
            fadeCanvasGameObject.SetActive(false);
            fadeCanvasGroup.alpha = 1.0f;
        }

        /// <summary>
        /// Required to allow selection in Events
        /// </summary>
        public void FadeInProxy()
        {
            FadeIn();
        }
        
        /// <summary>
        /// Required to allow selection in Events
        /// </summary>
        public void FadeOutProxy()
        {
            FadeOut();
        }
        
        /// <summary>
        /// Public method to fade in
        /// </summary>
        public void FadeIn(Action callback=null)
        {
            onBeforeFadeStartEvent.Invoke();
            fadeCanvasGameObject.SetActive(true);
            fadeCanvasGroup.alpha = 1.0f;
            _isFading = true;
            FadeInAudio();
            FadeInCanvas(callback);
        }
        
        /// <summary>
        /// Public method to fade out
        /// </summary>
        public void FadeOut(Action callback=null)
        {
            fadeCanvasGameObject.SetActive(true);
            fadeCanvasGroup.alpha = 0.0f;
            _isFading = true;
            FadeOutAudio();
            FadeOutCanvas(callback);
        }

        /// <summary>
        /// Fade in audio over time
        /// </summary>
        private void FadeInAudio()
        {
            if (!fadeAudioIn)
            {
                return;
            }
            StartCoroutine(FadeAudioWithDelay(-80.0f, 0.0f, audioFadeInDuration, delayBeforeAudioFadeIn));
        }

        /// <summary>
        /// Fade out audio over time
        /// </summary>
        private void FadeOutAudio()
        {
            if (!fadeAudioOut)
            {
                return;
            }
            StartCoroutine(FadeAudioWithDelay( 0.0f, -80.0f, audioFadeOutDuration, delayBeforeAudioFadeOut));
        }

        /// <summary>
        /// Fade in the Audio Mixer over time
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeAudioWithDelay(float startingVolume, float targetVolume, float fadeDuration, float fadeDelay)
        {
            // Wait for the delay period
            yield return new WaitForSecondsRealtime(fadeDelay);
            float currTime = 0.0f;
            while (currTime < fadeDuration)
            {
                currTime += Time.unscaledDeltaTime;
                float newVolume = Mathf.Lerp(startingVolume, targetVolume, currTime / fadeDuration);
                audioMixer.SetFloat("MasterVolume", newVolume);
                yield return null;
            }
            audioMixer.SetFloat("MasterVolume", targetVolume);
        }

        /// <summary>
        /// Fade out canvas with callback invoked once done
        /// </summary>
        /// <param name="callback"></param>
        private void FadeOutCanvas(Action callback=null)
        {
            StartCoroutine(FadeCanvasAsync(0.0f, 1.0f, sceneFadeOutDuration, delayBeforeScreenFadeOut, true,
                callback));
        }
        
        /// <summary>
        /// Fade in with a callback once complete
        /// </summary>
        /// <param name="callback"></param>
        private void FadeInCanvas(Action callback=null)
        {
            StartCoroutine(FadeCanvasAsync(1.0f, 0.0f, sceneFadeInDuration, delayBeforeScreenFadeIn, false,
                callback));
        }
        
        /// <summary>
        /// Fade the Canvcas to Black over time
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeCanvasAsync(float start, float target, float fadeDuration, float fadeDelay, bool endCanvasState,
            Action callback)
        {
            onFadeStartEvent.Invoke();
            // Wait for the delay period
            yield return new WaitForSecondsRealtime(fadeDelay);

            float timeElapsed = 0;
            float alphaValue;

            while (timeElapsed < fadeDuration)
            {
                alphaValue = Mathf.Lerp(start, target, timeElapsed / fadeDuration);
                fadeCanvasGroup.alpha = alphaValue;
                timeElapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            fadeCanvasGroup.alpha = target;
            fadeCanvasGameObject.SetActive(endCanvasState);
            _isFading = false;
            callback?.Invoke();
            onFadeEndEvent.Invoke();
        }
    }
}