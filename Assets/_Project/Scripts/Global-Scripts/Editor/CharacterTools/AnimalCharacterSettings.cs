using UnityEngine;

namespace DaftAppleGames.Editor.CharacterTools
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "AnimalCharacterSettings", menuName = "Daft Apple Games/Characters/Animal settings", order = 1)]
    public class AnimalCharacterConfig : ScriptableObject
    {
        public string characterName;
    }
}