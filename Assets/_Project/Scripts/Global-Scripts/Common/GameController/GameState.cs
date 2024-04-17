using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.GameControllers
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "GameState", menuName = "Daft Apple Games/Game/Game state", order = 1)]
    public class GameState : ScriptableObject
    {
        // Public serializable properties
        [BoxGroup("Game State")] public CharSelection selectedCharacter;
        [BoxGroup("Game State")] public bool isLoadingFromSave;


    }
}