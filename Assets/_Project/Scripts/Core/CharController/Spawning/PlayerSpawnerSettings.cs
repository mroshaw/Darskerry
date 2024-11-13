using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerSpawnerSettings", menuName = "Daft Apple Games/Player Spawner Settings", order = 1)]
    public class PlayerSpawnerSettings : CharacterSpawnerSettings
    {
        [BoxGroup("Prefabs")][AssetsOnly] public GameObject cameraPrefab;
        [BoxGroup("Prefabs")][AssetsOnly] public GameObject cmCameraRigPrefab;
    }
}
