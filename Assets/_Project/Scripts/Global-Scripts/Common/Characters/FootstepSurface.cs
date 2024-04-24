using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Characters
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "FootstepSurface", menuName = "Daft Apple Games/Characters/Footstep Surface", order = 1)]
    public class FootstepSurface : ScriptableObject
    {
        [BoxGroup("Surface Details")] public string[] surfaceStrings;
        [BoxGroup("Surface Details")] public AudioClip surfaceSounds;
        [BoxGroup("Step Behaviour")] public bool spawnFootstep;
        [BoxGroup("Step Behaviour")] public LayerMask footstepDecalLayer;
        [BoxGroup("Step Behaviour")] public bool spawnParticle;
    }
}