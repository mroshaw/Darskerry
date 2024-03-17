using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor.Characters
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "BeastCharacterSettings", menuName = "Settings/Characters/Beast Settings", order = 1)]
    public class BeastCharacterConfig : ScriptableObject
    {
        public string characterName;
    }
}