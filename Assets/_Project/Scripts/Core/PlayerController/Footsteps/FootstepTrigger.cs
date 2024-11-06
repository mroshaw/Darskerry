using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController.FootSteps
{
    public class FootstepTrigger : CharacterTrigger
    {
        #region Class Variables
        private AudioSource _audioSource;
        private float _cooldownCounter = 0.0f;
        public FootstepManager FootstepManager { get; set; }
        #endregion

        #region Startup
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            if (!_audioSource)
            {
                Debug.LogError($"FootstepTrigger: no AudioSource on this gameobject! {gameObject}");
            }
        }
        #endregion

        #region Class methods
        public override void TriggerEnter(Collider other)
        {
            if (_cooldownCounter > 0.0f)
            {
                return;
            }

            FootstepManager.GetSurfaceFromCollision(transform, other, out FootstepSurface footstepSurface,
                out Vector3 spawnPosition);

            // Spawn particles
            if (footstepSurface.spawnParticle)
            {
                FootstepManager.SpawnFootStepParticleFx(spawnPosition, FootstepManager.transform.rotation);
            }

            // Spawn decal
            if (footstepSurface.spawnDecal)
            {
                FootstepManager.SpawnFootStepDecal(spawnPosition, FootstepManager.transform.rotation);
            }

            // Play random audio
            System.Random randomAudio = new System.Random();
            int audioIndex = randomAudio.Next(0, footstepSurface.audioClips.Length);
            AudioClip audioClip = footstepSurface.audioClips[audioIndex];
            _audioSource.Stop();
            _audioSource.PlayOneShot(audioClip);

            _cooldownCounter = 0.5f;

        }

        private void Update()
        {
            _cooldownCounter -= Time.deltaTime;
        }

        public override void TriggerExit(Collider other)
        {
        }
        #endregion
    }
}