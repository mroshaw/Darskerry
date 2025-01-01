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

        [BoxGroup("Events")] [SerializeField] private bool triggerOnEnable;
        [BoxGroup("Events")] [EnableIf("triggerOnEnable")] public UnityEvent enableTriggeredEvent;
        [BoxGroup("Events")] [SerializeField] private bool triggerOnAwake;
        [BoxGroup("Events")] [EnableIf("triggerOnAwake")] public UnityEvent awakeTriggeredEvent;
        [BoxGroup("Events")] [SerializeField] private bool triggerOnStart;
        [BoxGroup("Events")] [EnableIf("triggerOnStart")] public UnityEvent startTriggeredEvent;

        #endregion

        #region Startup
        /// <summary>
        /// Subscribe to events
        /// </summary>   
        private void OnEnable()
        {
            if (triggerOnEnable)
            {
                StartCoroutine(DelayedTriggerAsync(enableTriggeredEvent));
            }
        }

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            if (triggerOnAwake)
            {
                StartCoroutine(DelayedTriggerAsync(awakeTriggeredEvent));
            }
        }
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            if (triggerOnStart)
            {
                StartCoroutine(DelayedTriggerAsync(startTriggeredEvent));
            }
        }
        #endregion


        #region Class Methods

        private IEnumerator DelayedTriggerAsync(UnityEvent unityEvent)
        {
            yield return new WaitForSeconds(delayBeforeTrigger);
            Debug.Log("DelayedTriggerAsync Event Triggered");
            unityEvent?.Invoke();
        }
        #endregion
    }
}