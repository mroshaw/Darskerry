using System.Collections;
using UnityEngine;

namespace DaftAppleGames.Common.MainMenu
{
    public class CameraAnimAfterDelay : MonoBehaviour
    {
        [Header("Config")]
        public float delayInSecs;
        public string animTriggerName;

        private Animator _animator;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(StartAnimAfterDelay());
        }

        /// <summary>
        /// Async method to set animator trigger after given delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartAnimAfterDelay()
        {
            yield return new WaitForSeconds(delayInSecs);
            _animator.SetBool(animTriggerName, true);
        }

    }
}