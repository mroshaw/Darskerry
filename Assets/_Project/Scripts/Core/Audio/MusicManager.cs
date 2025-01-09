using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;

namespace DaftAppleGames.Darskerry.Core.Audio
{
    public class MusicManager : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Music Settings")] [SerializeField] private MusicPlaylist musicPlaylist;
        [BoxGroup("Music Settings")] [SerializeField] private AudioMixerGroup mixerGroup;
        [BoxGroup("Music Settings")] [SerializeField] private bool playOnStart;
        [BoxGroup("Music Settings")] [SerializeField] private float delayBeforePlayOnStart = 0.0f;
        [BoxGroup("Music Settings")] [SerializeField] private AudioSource mainAudioSource;
        [BoxGroup("Music Settings")] [SerializeField] private AudioSource fadeTempAudioSource;
        [BoxGroup("Blend Settings")] [SerializeField] private float fadeInTime = 5.0f;
        [BoxGroup("Blend Settings")] [SerializeField] private float fadeOutTime = 3.0f;
        [BoxGroup("Blend Settings")] [SerializeField] private float fadeBufferTime = 6.0f;
        #endregion

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
            PlayFirstTrackAfterDelay();
        }
        #endregion

        #region Class methods

        private void PlayFirstTrackAfterDelay()
        {
            StartCoroutine(PlayFirstTrackAfterDelayAsync());
        }

        private IEnumerator PlayFirstTrackAfterDelayAsync()
        {
            yield return new WaitForSeconds(delayBeforePlayOnStart);
            PlayFirstTrack();
        }

        private void PlayFirstTrack()
        {
            FadeInTrack(musicPlaylist.musicClips[0]);
        }

        private void FadeInTrack(MusicClip musicClip)
        {
            StartCoroutine(FadeInTrackAsync(musicClip));
        }

        private IEnumerator FadeInTrackAsync(MusicClip musicClip)
        {
            mainAudioSource.volume = musicClip.volume;
            mainAudioSource.loop = musicClip.loop;
            mainAudioSource.clip = musicClip.clip;
            float time = 0;
            float startValue = 0;
            float endValue = 1.0f;

            mainAudioSource.volume = startValue;
            mainAudioSource.Play();

            while (time < fadeInTime)
            {
                mainAudioSource.volume = Mathf.Lerp(startValue, endValue, time / fadeInTime);
                time += Time.deltaTime;
                yield return null;
            }
            mainAudioSource.volume = endValue;
        }
        #endregion
    }
}