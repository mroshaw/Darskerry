using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Spawning
{
    public class ProximityEnabler : MonoBehaviour
    {
        [Header("General Settings")]
        public bool enabledOnStart = false;
        public GameObject[] gameObjects;
        public string playerTag = "Player";

        [Header("Events")]
        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        /// <summary>
        /// Configure the component
        /// </summary>
        public void Start()
        {
            // Check for Collider
            if(!GetComponent<Collider>())
            {
                Debug.LogError("ProximityEnabler: need a Collider component!");
            }

            if(enabledOnStart)
            {
                EnableObjects();
            }
            else
            {
                DisableObjects();
            }
        }

        /// <summary>
        /// Handle player entering collider range
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag(playerTag))
            {
                EnableObjects();
                OnEnter.Invoke();
            }
        }

        /// <summary>
        /// Handle player leaving the collider range
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(playerTag))
            {
                DisableObjects();
                OnExit.Invoke();
            }
        }

        /// <summary>
        /// Enable all associated objects
        /// </summary>
        private void EnableObjects()
        {
            foreach(GameObject obj in gameObjects)
            {
                obj.SetActive(true);
            }
        }

        /// <summary>
        /// Disable all associated objects
        /// </summary>
        private void DisableObjects()
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(false);
            }
        }
    }
}