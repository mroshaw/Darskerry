using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.CharacterTools
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "BeastCharacterSettings", menuName = "Daft Apple Games/Characters/Beast settings", order = 1)]
    public class BeastCharacterConfig : ScriptableObject
    {
        [BoxGroup("Beast Settings")] public string characterName;
    }
}