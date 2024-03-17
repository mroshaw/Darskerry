using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LodGroupAutoEditorSettings", menuName = "Settings/Scene Tools/LodGroupAutoEditor", order = 1)]
    public class LodGroupAutoEditorSettings : BaseAutoEditorSettings
    {
        [Header("Lod Group Editor Settings")]
        public float[] lodWeights = new float[] { 0.6f, 0.3f, 0.1f };
        public LODFadeMode fadeMode = LODFadeMode.CrossFade;
        public float cullRatio = 0.01f;
    }
}