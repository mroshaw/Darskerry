using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Buildings
{
    /// <summary>
    /// MonoBehaviour to manage entering and existing a building
    /// </summary>
    public class InteriorTriggerController : MonoBehaviour
    {
        [FoldoutGroup("Enable On Enter")]
        public GameObject[] enableOnEnter;
        [FoldoutGroup("Disable On Exit")]
        public GameObject[] disableOnExit;
        [FoldoutGroup("Disable On Enter")]
        public GameObject[] disableOnEnter;
        [FoldoutGroup("Enable On Exit")]
        public GameObject[] enableOnExit;

        [FoldoutGroup("Events")]
        public UnityEvent onInteriorEnterEvent;
        [FoldoutGroup("Events")]
        public UnityEvent onInteriorExitEvent;
        
        /// <summary>
        /// Configure the component
        /// </summary>
        private void Start()
        {

        }
        
        /// <summary>
        /// Enable interior
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Enable on enter
                foreach (GameObject gameObj in enableOnEnter)
                {
                    gameObj.SetActive(true);
                }

                // Disable on Enter
                foreach (GameObject gameObj in disableOnEnter)
                {
                    gameObj.SetActive(false);
                }
                onInteriorEnterEvent.Invoke();
            }
        }

        /// <summary>
        /// Disable interior
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Enable on Exit
                foreach (GameObject gameObj in enableOnExit)
                {
                    gameObj.SetActive(true);
                }

                // Disable on Exit
                foreach (GameObject gameObj in disableOnExit)
                {
                    gameObj.SetActive(false);
                }
                onInteriorExitEvent.Invoke();
            }
        }
    }
}