using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.AI 
{
    /// <summary>
    /// Class defining properties and behaviours of an NPC workspace
    /// That being defined transforms and properties within a workplace
    /// where NPCs can perform work actions.
    /// </summary>
    public class NpcWorkSpace : MonoBehaviour
    {
        [BoxGroup("Work Space Settings")]
        public Transform idleTransform;
        [BoxGroup("Work Space Settings")]
        public Transform busyTransform;
        [BoxGroup("Work Space Settings")]
        public NpcChair allocatedChair;
    }
}
