using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterSpawnerSettings", menuName = "Daft Apple Games/Character Spawner Settings", order = 1)]
    public class CharacterSpawnerSettings : ScriptableObject
    {
        [BoxGroup("Prefabs")][AssetsOnly] public GameObject characterPrefab;
        [BoxGroup("Prefabs")][AssetsOnly] public GameObject footstepPoolsPrefab;
    }
}
