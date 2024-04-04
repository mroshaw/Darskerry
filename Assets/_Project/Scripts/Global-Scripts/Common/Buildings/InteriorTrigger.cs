using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Buildings
{
    /// <summary>
    /// MonoBehaviour to manage entering and existing a building
    /// </summary>
    public class InteriorTrigger : MonoBehaviour
    {
        [BoxGroup("Interior Events")] public UnityEvent onInteriorEnterEvent;
        [BoxGroup("Exterior Events")] public UnityEvent onInteriorExitEvent;
        
        
        /// <summary>
        /// Enable interior
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
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
                onInteriorExitEvent.Invoke();
            }
        }
    }
}