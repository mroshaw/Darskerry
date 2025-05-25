using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;

namespace DaftAppleGames.Darskerry.Core.Audio
{
    public class MusicManager : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Setup")] [SerializeField] private AudioSource musicAudioSource;
        [BoxGroup("Music")] [SerializeField] private MusicPlaylist musicPlaylist;
        [BoxGroup("Music")] [SerializeField] private AudioMixerGroup mixerGroup;
        [BoxGroup("Music")] [SerializeField] private bool playOnStart;
        [BoxGroup("Music")] [SerializeField] private float delayBeforePlayOnStart;
        [BoxGroup("Blend")] [SerializeField] private float fadeInDuration = 5.0f;
        [BoxGroup("Blend")] [SerializeField] private float durationBetweenClips = 2.0f;
        [BoxGroup("Performance")] [SerializeField] private int checkEveryFrames = 5;

        private MusicClip _currentClip;
        private int _currentTrack;
        private int _totalTracks;
        private bool _isFading;
        private bool _isWaiting;
        #endregion

        private void Awake()
        {
            _currentTrack = 0;
            _isFading = false;
            _isWaiting = false;
            _totalTracks = musicPlaylist.TotalTracks;
        }

        #region Startup

        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            if (!playOnStart)
            {
                return;
            }

            StartCoroutine(PlayTrackAfterDelayAsync(_currentTrack, delayBeforePlayOnStart));
        }

        #endregion

        #region Update

        private void Update()
        {
            if (_isFading || _isWaiting || musicAudioSource.isPlaying || Time.frameCount % checkEveryFrames != 0)
            {
                return;
            }
            PlayNextTrack();
        }
        #endregion

        #region Class methods


        private IEnumerator PlayTrackAfterDelayAsync(int trackIndex, float delayDuration)
        {
            _isWaiting = true;
            yield return new WaitForSeconds(delayDuration);
            yield return FadeInTrackAsync(trackIndex);
            _isWaiting = false;
        }

        [Button("Next Track")]
        private void PlayNextTrack()
        {
            if (!_currentClip.loop)
            {
                _currentTrack = _currentTrack == _totalTracks ? 0 : _currentTrack + 1;
            }

            StartCoroutine(PlayTrackAfterDelayAsync(_currentTrack, durationBetweenClips));

        }

        private IEnumerator FadeInTrackAsync(int trackIndex)
        {
            _isFading = true;
            _currentClip = musicPlaylist.GetTrack(trackIndex);
            musicAudioSource.volume = 0.0f;
            musicAudioSource.clip = _currentClip.clip;
            musicAudioSource.Play();
            yield return FadeAudioSourceAsync(musicAudioSource, _currentClip.volume, fadeInDuration, true);
            _isFading = false;
        }

        private IEnumerator FadeAudioSourceAsync(AudioSource audioSource, float targetVolume, float fadeDuration, bool fadeIn)
        {
            float time = 0;

            float startValue = fadeIn ? 0.0f : audioSource.volume;
            audioSource.volume = startValue;

            while (time < fadeDuration)
            {
                audioSource.volume = Mathf.Lerp(startValue, targetVolume, time / fadeDuration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = targetVolume;
        }
        #endregion
    }
}