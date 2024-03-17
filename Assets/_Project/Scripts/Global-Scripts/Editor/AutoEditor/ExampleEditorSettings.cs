using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "ExampleAutoEditorSettings", menuName = "Settings/Example/ExampleAutoEditor", order = 1)]
    public class ExampleAutoEditorSettings : BaseAutoEditorSettings
    {
        [Header("Example Editor Settings")]
        public string sampleString;
        public int sampleInt;
        public float sampleFloat;
        public bool sampleBool;
    }
}