#if HAP
using DaftAppleGames.Common.Spawning;
using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    public class Horse : Character
    {
        [Header("Spawner Settings")]
        public HorseSpawner spawner;
    }
}
#endif