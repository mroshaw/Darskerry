using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "InteriorSwapperEditorSettings", menuName = "Settings/Buildings/InteriorSwapperEditor", order = 1)]
    public class InteriorSwapperEditorSettings : ScriptableObject
    {
        [BoxGroup("Material Settings")]
        public string[] sourceMaterials;
        public string[] targetMaterials;
    }
}