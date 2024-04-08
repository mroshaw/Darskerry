using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Core.Buildings
{
    /// <summary>
    /// MonoBehaviour to manage entering and existing a building
    /// </summary>
    public class InteriorTriggerVolume : MonoBehaviour
    {
        [BoxGroup("Trigger Settings")] public string[] triggerTags;
        [BoxGroup("Trigger Settings")] public LayerMask triggerLayerMask;
        
        [BoxGroup("Interior Events")] public UnityEvent<Collider> onInteriorEnterEvent;
        [BoxGroup("Exterior Events")] public UnityEvent<Collider> onInteriorExitEvent;
        
        /// <summary>
        /// Other collider has entered the interior trigger collider
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            string otherTag = other.tag;
            LayerMask otherLayer = other.gameObject.layer;
            
            // If the other collider matches any of the trigger tags or trigger layer mask, invoke the event
            if ((triggerTags.Length==0 || triggerTags.Any(otherTag.Contains)) && (triggerLayerMask==0 || ( triggerLayerMask & (1 << otherLayer)) != 0))
            {
                onInteriorEnterEvent.Invoke(other);
            }
        }

        /// <summary>
        /// Other collider has exited the interior trigger collider
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            string otherTag = other.tag;
            LayerMask otherLayer = other.gameObject.layer;
            
            if ((triggerTags.Length==0 || triggerTags.Any(otherTag.Contains)) && (triggerLayerMask==0 || ( triggerLayerMask & (1 << otherLayer)) != 0))
            {
                onInteriorExitEvent.Invoke(other);
            }
        }
    }
}