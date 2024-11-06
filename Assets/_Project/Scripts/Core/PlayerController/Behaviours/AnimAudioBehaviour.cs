using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController.Behaviours
{
    [RequireComponent(typeof(AudioSource))]
    public class AnimAudioBehaviour : CharacterBehaviour
    {
        [BoxGroup("Settings")] public float delayBeforeStart = 0.1f;
        [BoxGroup("AudioClips")] public AudioClip[] audioClips;

        #region State events
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (audioClips.Length == 0)
            {
                return;
            }

            Character.StartCoroutine(PlayClipAsync());
        }

        private IEnumerator PlayClipAsync()
        {
            yield return new WaitForSeconds(delayBeforeStart);
            AudioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length - 1)]);
        }
        #endregion
    }
}