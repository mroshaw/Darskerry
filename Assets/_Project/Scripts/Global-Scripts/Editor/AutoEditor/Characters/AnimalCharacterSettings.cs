using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor.Characters
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "AnimalCharacterSettings", menuName = "Settings/Characters/Animal Settings", order = 1)]
    public class AnimalCharacterConfig : ScriptableObject
    {
        public string characterName;
    }
}