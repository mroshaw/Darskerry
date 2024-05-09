using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.AI
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "AiConfigSettings", menuName = "Daft Apple Games/AI/Ai config", order = 1)]
    public class AiConfigSettings : ScriptableObject
    {
        [BoxGroup("Movement")] public int wanderSpeedIndex;
        [BoxGroup("Movement")] public int fleeSpeedIndex;
        [BoxGroup("Movement")] public int pursueSpeedIndex;
        [BoxGroup("Movement")] public float wanderRange;
    }
}