using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Characters
{
    public class Character : MonoBehaviour
    {
        [BoxGroup("Character Settings")]
        public string characterName;

        [BoxGroup("Debug")]
        public bool enableDebug = false;

        [BoxGroup("Audio FX")]
        public AudioSource audioSource;
        [BoxGroup("Audio FX")]
        public AudioClip[] deathAudioClips;
        [BoxGroup("Audio FX")]
        public AudioClip[] hitAudioClips;

        private int _hitClips;
        private int _deathClips;
        
        /// <summary>
        /// Initialise the animal
        /// </summary>
        public virtual void Start()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }

            _hitClips = hitAudioClips.Length;
            _deathClips = deathAudioClips.Length;
        }

        /// <summary>
        /// Play a random hit audio clip
        /// </summary>
        public void PlayHitAudio()
        {
            System.Random random = new System.Random();
            int randomClipNum = random.Next(0, _hitClips);
            audioSource.PlayOneShot(hitAudioClips[randomClipNum]);
        }

        /// <summary>
        /// Play a random death audio clip
        /// </summary>
        public void PlayDeathAudio()
        {
            System.Random random = new System.Random();
            int randomClipNum = random.Next(0, _deathClips);
            audioSource.PlayOneShot(deathAudioClips[randomClipNum]);
        }
        
        // Update is called once per frame
        public virtual void Update()
        {
        }
    }
}