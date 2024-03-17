using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor.Buildings
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LightAutoEditorSettings", menuName = "Settings/Buildings/LightAutoEditor", order = 1)]
    public class LightAutoEditorSettings : BaseAutoEditorSettings
    {
        [Header("Light Settings")]
        public float range;
        public float intensity;
#if HDPipeline
        public float radius;
#else
        public float indirectMultiplier;
#endif
    }
}