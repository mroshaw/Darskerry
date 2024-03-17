using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.AI
{
    /// <summary>
    /// Class defining properties and behaviours of an NPC homespace. That being
    /// transforms and properties within a homespace where an NPC performs home based
    /// actions such as sitting and sleeping.
    /// </summary>
    public class NpcHomeSpace : MonoBehaviour
    {
        [BoxGroup("Home Space Settings")]
        public Transform idleTransform;
        [BoxGroup("Home Space Settings")]
        public Transform busyTransform;
        [BoxGroup("Home Space Settings")]
        public NpcChair allocatedChair;
        [BoxGroup("Home Space Settings")]
        public NpcBed allocatedBed;
    }
}
