using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Helpers
{
    public class DelayedStartEventTrigger : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Settings")] [SerializeField] private float delayBeforeTrigger;

        [BoxGroup("Events")] public UnityEvent enableTriggeredEvent;
        [BoxGroup("Events")] public UnityEvent awakeTriggeredEvent;
        [BoxGroup("Events")] public UnityEvent startTriggeredEvent;

        #endregion

        #region Startup
        /// <summary>
        /// Subscribe to events
        /// </summary>   
        private void OnEnable()
        {
            if (enableTriggeredEvent != null)
            {
                StartCoroutine(DelayedTriggerAsync(enableTriggeredEvent));
            }
        }

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            if (awakeTriggeredEvent != null)
            {
                StartCoroutine(DelayedTriggerAsync(awakeTriggeredEvent));
            }
        }
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            if (startTriggeredEvent != null)
            {
                StartCoroutine(DelayedTriggerAsync(startTriggeredEvent));
            }
        }
        #endregion


        #region Class Methods

        private IEnumerator DelayedTriggerAsync(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(delayBeforeTrigger);
            unityEvent?.Invoke();
        }
        #endregion
    }
}