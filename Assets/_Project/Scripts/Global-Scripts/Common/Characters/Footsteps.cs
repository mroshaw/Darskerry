using Sirenix.OdinInspector;
using UnityEngine;
namespace DaftAppleGames.Common.Characters
{
    public class Footsteps : MonoBehaviour
    {
        [BoxGroup("Footstep Surfaces")] public FootstepSurface defaultSurface;
        [BoxGroup("Footstep Surfaces")] public FootstepSurface[] customSurfaces;
        [BoxGroup("Footstep Pools")] public FootstepPool audioPool;
        [BoxGroup("Footstep Pools")] public FootstepPool stepPool;
        [BoxGroup("Footstep Pools")] public FootstepPool particlePool;
    }
}