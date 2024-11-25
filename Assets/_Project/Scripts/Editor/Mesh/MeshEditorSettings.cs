using UnityEngine;

namespace DaftAppleGames.Editor.ObjectTools
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "MeshToolSettings", menuName = "Daft Apple Games/Buildings/Mesh Tool Settings", order = 1)]
    internal class MeshEditorSettings : ScriptableObject
    {
        internal MeshSettings meshSettings;
    }
}