using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    public class BaseAutoEditorSettings : ScriptableObject
    {
        [Header("Object Identifiers")]
        public string[] objectNameStrings;

        [Header("Basic Settings")]
        public string settingsName;
    }
}