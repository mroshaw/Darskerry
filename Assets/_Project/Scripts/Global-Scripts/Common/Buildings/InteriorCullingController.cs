using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    /// <summary>
    /// MonoBehaviour to enable and disable interior
    /// when player is within proximity of building
    /// </summary>
    public class InteriorCullingController : MonoBehaviour
    {
        [Header("Settings")]
        public List<GameObject> interiorObjects;
        public bool startDisabled = true;

        /// <summary>
        /// Configure the component
        /// </summary>
        private void Start()
        {
            if (startDisabled)
            {
                SetAllObjectState(false);
            }
        }

        /// <summary>
        /// Sets all objects to the given active state
        /// </summary>
        /// <param name="isActive"></param>
        private void SetAllObjectState(bool isActive)
        {
            foreach (GameObject interiorObject in interiorObjects)
            {
                interiorObject.SetActive(isActive);
            }
        }
        
        /// <summary>
        /// Enable interior
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SetAllObjectState(true);
            }
        }

        /// <summary>
        /// Disable interior
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SetAllObjectState(false);
            }
        }
    }
}