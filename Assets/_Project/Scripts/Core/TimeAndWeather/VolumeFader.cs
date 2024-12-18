using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace DaftAppleGames.Darskerry.Core.TimeAndWeather
{
    public class VolumeFader : MonoBehaviour
    {
        #region Class Variables

        [BoxGroup("Settings")] [SerializeField] private Volume[] sourceVolumes;
        [BoxGroup("Settings")] [SerializeField] private Volume[] targetVolumes;
        [BoxGroup("Settings")] [SerializeField] private float onTargetWeight = 1.0f;
        [BoxGroup("Settings")] [SerializeField] private float offTargetWeight = 0.0f;
        [BoxGroup("Settings")] [SerializeField] private float fadeDuration;
        [BoxGroup("Events")] public UnityEvent fadeStartedEvent;
        [BoxGroup("Events")] public UnityEvent fadeCompleteEvent;

        [BoxGroup("Debug")] [SerializeField] private bool _isFading;

        private int _numSourceVolumes;
        private int _numTargetVolumes;

        private float[] _currentSourceVolumeWeights;
        private float[] _currentTargetVolumeWeights;

        #endregion

        #region Startup

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            _isFading = false;
            _numSourceVolumes = sourceVolumes.Length;
            _numTargetVolumes = targetVolumes.Length;
            _currentSourceVolumeWeights = new float[_numSourceVolumes];
            _currentTargetVolumeWeights = new float[_numTargetVolumes];
            UpdateSourceVolumeWeights();
            UpdateTargetVolumeWeights();
        }

        #endregion

        #region Class Methods

        private void UpdateSourceVolumeWeights()
        {
            for (int currVol = 0; currVol < _numSourceVolumes; currVol++)
            {
                _currentSourceVolumeWeights[currVol] = sourceVolumes[currVol].weight;
            }
        }

        private void UpdateTargetVolumeWeights()
        {
            for (int currVol = 0; currVol < _numTargetVolumes; currVol++)
            {
                _currentTargetVolumeWeights[currVol] = targetVolumes[currVol].weight;
            }
        }


        [Button("Fade In First Source")]
        public void FadeInFirstSource()
        {
            UpdateSourceVolumeWeights();
            FadeSingleVolume(sourceVolumes[0], onTargetWeight);
        }

        [Button("Fade Out First Source")]
        public void FadeOutFirstSource()
        {
            FadeSingleVolume(sourceVolumes[0], offTargetWeight);
        }

        public void FadeFirstSourceToFirstTarget()
        {
        }

        public void FadeFirsTargetToFirstSource()
        {
 }

        protected void FadeSingleVolume(Volume volume, float targetWeight)
        {
            if (_isFading)
            {
                return;
            }
            StartCoroutine(FadeSingleVolumeAsync(volume, targetWeight));
        }

        protected void FadeMultipleVolumes(Volume[] volumes, float[] currentVolumeWeights, int numVolumes, float targetWeight)
        {
            if (_isFading)
            {
                return;
            }
            StartCoroutine(FadeMultipleVolumeAsync(volumes, currentVolumeWeights, numVolumes, targetWeight));
        }

        private IEnumerator FadeSingleVolumeAsync(Volume volume, float targetWeight)
        {
            StartFade();
            float elapsedTime = 0;
            float startWeight = volume.weight;

            while (elapsedTime < fadeDuration)
            {
                volume.weight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            volume.weight = targetWeight;
            CompleteFade();
        }

        private IEnumerator FadeMultipleVolumeAsync(Volume[] volumes, float[] currentVolumeWeights, int numVolumes, float targetWeight)
        {
            StartFade();
            float elapsedTime = 0;

            while (elapsedTime < fadeDuration)
            {
                for(int currVolume = 0; currVolume < numVolumes; currVolume++)
                {
                    volumes[currVolume].weight = Mathf.Lerp(currentVolumeWeights[currVolume], targetWeight, elapsedTime / fadeDuration);
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            for(int currVolume = 0; currVolume < numVolumes; currVolume++)
            {
                volumes[currVolume].weight = targetWeight;
            }

            CompleteFade();
        }

        private IEnumerator FadeBetweenSingleVolumesAsync(Volume startVolume, float startVolumeStartWeight, float startVolumeEndWeight,
                Volume endVolume ,float endVolumeStartWeight, float endVolumeEndWeight, float duration)
        {
            StartFade();
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                startVolume.weight = Mathf.Lerp(startVolumeStartWeight, startVolumeEndWeight, elapsedTime / duration);
                endVolume.weight = Mathf.Lerp(endVolumeStartWeight, endVolumeEndWeight, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            startVolume.weight = startVolumeEndWeight;
            endVolume.weight = endVolumeEndWeight;

            CompleteFade();
        }

        private IEnumerator FadeBetweenMultipleVolumeAsync(Volume[] startVolumes, float startVolumeStartWeight, float startVolumeEndWeight,
            Volume[] endVolumes ,float endVolumeStartWeight, float endVolumeEndWeight, float duration)
        {
            StartFade();
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                foreach (Volume startVolume in startVolumes)
                {
                    startVolume.weight = Mathf.Lerp(startVolumeStartWeight, startVolumeEndWeight, elapsedTime / duration);
                }

                foreach (Volume endVolume in endVolumes)
                {
                    endVolume.weight = Mathf.Lerp(endVolumeStartWeight, endVolumeEndWeight, elapsedTime / duration);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            foreach (Volume endVolume in endVolumes)
            {
                endVolume.weight = endVolumeEndWeight;
            }
            CompleteFade();
        }

        private void StartFade()
        {
            _isFading = true;
            fadeStartedEvent.Invoke();
        }

        private void CompleteFade()
        {
            _isFading = false;
            fadeCompleteEvent.Invoke();
        }

        #endregion
    }
}