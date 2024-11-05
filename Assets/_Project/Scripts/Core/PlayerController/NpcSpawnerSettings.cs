using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    [CreateAssetMenu(fileName = "NPCSpawnerSettings", menuName = "Daft Apple Games/NPC Spawner Settings", order = 1)]
    public class NpcSpawnerSettings : CharacterSpawnerSettings
    {
        [BoxGroup("Prefabs")][AssetsOnly] public GameObject npcPrefab;
    }
}
