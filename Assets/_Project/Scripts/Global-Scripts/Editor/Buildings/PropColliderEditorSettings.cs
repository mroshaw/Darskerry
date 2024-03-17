using DaftAppleGames.Editor.Common;
using Sirenix.OdinInspector;
using UnityEditor.EditorTools;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "PropColliderEditorSettings", menuName = "Settings/Buildings/PropColliderEditor", order = 1)]
    public class PropColliderEditorSettings : EditorToolSettings
    {
        [BoxGroup("Prefab Assets")] public string assetPath = "Assets/3DForge";
        [BoxGroup("Unwanted Colliders")] public string shaderToFindPath = "Legacy Shaders/Transparent/Diffuse";
        [BoxGroup("Prop Colliders")] public string[] boxSearchStrings;
        [BoxGroup("Prop Colliders")] public string[] capsuleSearchStrings;
        [BoxGroup("Prop Colliders")] public string[] sphereSearchStrings;
        [BoxGroup("Prop Colliders")] public string[] meshSearchStrings;
    }
}