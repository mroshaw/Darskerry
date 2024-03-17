using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Audio
{
    public class BackgroundMusicTrigger : MonoBehaviour
    {
        // Public serializable properties
        [BoxGroup("Settings")]
        public bool destroyOnTrigger = true;
        [BoxGroup("Settings")]
        public float destroyDelay = 1.0f;

        [BoxGroup("Events")]
        public UnityEvent TriggerEnterEvent;
        [BoxGroup("Events")]
        public UnityEvent TriggerExitEvent;

        private bool _isTriggered = false;

        /// <summary>
        /// Triggered on entering the collider
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            if (_isTriggered || !other.transform.CompareTag("Player"))
            {
                return;
            }

            if (TriggerEnterEvent.GetPersistentEventCount() == 0)
            {
                return;
            }
            TriggerEnterEvent.Invoke();
            _isTriggered = true;
            if (destroyOnTrigger)
            {
                StartCoroutine(DestroyAfterDelayAsync());
            }
        }

        /// <summary>
        /// Triggered on exiting the collider
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit(Collider other)
        {
            if (_isTriggered || !other.transform.CompareTag("Player"))
            {
                return;
            }

            if (TriggerExitEvent.GetPersistentEventCount() == 0)
            {
                return;
            }
            TriggerExitEvent.Invoke();
            _isTriggered = true;
            if (destroyOnTrigger)
            {
                StartCoroutine(DestroyAfterDelayAsync());
            }
        }

        /// <summary>
        /// Destroy GameObject after delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator DestroyAfterDelayAsync()
        {
            yield return new WaitForSecondsRealtime(destroyDelay);
            Destroy(gameObject);
        }
    }
}
