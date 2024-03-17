using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;

namespace DaftAppleGames.Common.Audio
{
    /// <summary>
    /// MonoBehaviour to provide Ambient Audio capability
    /// </summary>
    public class AmbientAudioManager : MonoBehaviour
    {
        public enum AmbientAudioType { Calm, LightWind, StrongWind, LightRain, HeavyRain, Storm, LightSnow, SnowStorm }

        [BoxGroup("General Settings")]
        public AudioMixerGroup mixerGroup;
        [BoxGroup("General Settings")]
        public float fadeTime;

        [FoldoutGroup("Ambient Audio Clips")]
        public AmbientClip[] ambientAudioClips;

        private AudioSource[] _audioSources;


        // Singleton static instance
        private static AmbientAudioManager _instance;
        public static AmbientAudioManager Instance => _instance;

        /// <summary>
        /// Set up singleton
        /// </summary>
        public void Awake()
        {
            // Set up singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        /// <summary>
        /// Initialise the component
        /// </summary>
        public void Start()
        {
            CreateAudioSources();
        }

        /// <summary>
        /// Start the ambient audio
        /// </summary>
        /// <param name="ambientAudioType"></param>
        public void StartAmbientAudio(AmbientAudioType ambientAudioType)
        {
            AmbientClip ambientClip = GetAmbientClipByType(ambientAudioType);
            if (ambientClip != null)
            {
                Debug.Log($"AmbientAudioManage: Starting {ambientClip.AudioType.ToString()}");
                ambientClip.Play();
            }
        }

        /// <summary>
        /// Stop the ambient audio
        /// </summary>
        /// <param name="ambientAudioType"></param>
        public void StopAmbientAudio(AmbientAudioType ambientAudioType)
        {
            AmbientClip ambientClip = GetAmbientClipByType(ambientAudioType);
            if (ambientClip != null)
            {
                ambientClip.Stop();
            }
        }

        /// <summary>
        /// Finds an AudioClip by type
        /// </summary>
        /// <param name="ambientAudioType"></param>
        /// <returns></returns>
        private AmbientClip GetAmbientClipByType(AmbientAudioType ambientAudioType)
        {
            foreach (AmbientClip ambientClip in ambientAudioClips)
            {
                if (ambientClip.AudioType == ambientAudioType)
                {
                    return ambientClip;
                }
            }

            return null;
        }

        /// <summary>
        /// Create an audio source for each Ambient sound type
        /// </summary>
        private void CreateAudioSources()
        {
            foreach (AmbientClip ambientClip in ambientAudioClips)
            {
                AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
                newAudioSource.playOnAwake = false;
                newAudioSource.loop = true;
                newAudioSource.outputAudioMixerGroup = mixerGroup;
                newAudioSource.clip = ambientClip.AudioClip;
                newAudioSource.volume = 0.0f;
                ambientClip.AmbientAudioSource = newAudioSource;
                ambientClip.FadeTime = fadeTime;
            }
        }

        /// <summary>
        /// Class for mapping AudioType to Clips
        /// </summary>
        [Serializable]
        public class AmbientClip
        {
            public AmbientAudioType  AudioType;
            public AudioClip AudioClip;
            private float _fadeTime;
            private AudioSource _audioSource;

            /// <summary>
            /// Getter and Setter for FadeTime
            /// </summary>
            internal float FadeTime
            {
                set => _fadeTime = value;
                get => _fadeTime;
            }

            /// <summary>
            /// Getter and Setter for AudioSource
            /// </summary>
            internal AudioSource AmbientAudioSource
            {
                set => _audioSource = value;
                get => _audioSource;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="audioType"></param>
            /// <param name="audioClip"></param>
            /// <param name="fadeTime"></param>
            private AmbientClip(AmbientAudioType audioType, AudioClip audioClip, float fadeTime)
            {
                AudioType = audioType;
                AudioClip = audioClip;
                _fadeTime = fadeTime;
            }


            /// <summary>
            /// Stop playing
            /// </summary>
            [Button("Play")]
            public void Play()
            {
                Instance.StartCoroutine(FadeAudioSource( 1.0f));
            }

            /// <summary>
            /// Start playing
            /// </summary>
            [Button("Stop")]
            public void Stop()
            {
                Instance.StartCoroutine(FadeAudioSource(0.0f));
            }

            /// <summary>
            /// Fades a source to a target volume over time
            /// </summary>
            /// <param name="source"></param>
            /// <param name="endVol"></param>
            /// <returns></returns>
            private IEnumerator FadeAudioSource(float endVol)
            {
                if (!AmbientAudioSource.isPlaying)
                {
                    AmbientAudioSource.Play();
                }

                float startVol = AmbientAudioSource.volume;
                float currTime = 0.0f;

                while (AmbientAudioSource.volume - endVol > 0.01f || AmbientAudioSource.volume - endVol < -0.01f)
                {
                    AmbientAudioSource.volume = Mathf.Lerp(startVol, endVol, currTime / FadeTime);
                    currTime += Time.deltaTime;
                    yield return null;
                }

                AmbientAudioSource.volume = endVol;
            }
        }
    }
}
