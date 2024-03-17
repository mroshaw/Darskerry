#if PIXELCRUSHERS
using System.Linq;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames
{
    /// <summary>
    /// Simple wrapper to call Dialogue Barks when player enters NPC collider area
    /// </summary>
    public class ProximityBark : BarkStarter
    {
        [BoxGroup("Bark Targets")]
        [Tooltip("Target to whom bark is addressed. Leave unassigned to just bark into the air.")]
        public Transform target;
        [BoxGroup("Bark Targets")]
        [Tooltip("Tags used to identify eligible colliders to initiate a bark.")]
        public string[] targetTags;
        
        /// <summary>
        /// Bark when the collider enters the trigger
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (enabled && (!DialogueManager.isConversationActive || allowDuringConversations) && !DialogueTime.isPaused)
            {
                if (targetTags.Any(other.tag.Contains))
                {
                    TryBark(target);
                }
            }
        }
    }
}
#endif