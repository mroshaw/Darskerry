using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.MeshTools
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LodGroupSettings", menuName = "Daft Apple Games/Objects/LOD group settings", order = 1)]
    public class LodGroupSettings : ScriptableObject
    {
        [Header("Lod Group Editor Settings")]
        [BoxGroup("LOD Group Editor Settings")] public float[] twoLodWeights = new float[] { 0.1f, 0.8f };
        [BoxGroup("LOD Group Editor Settings")] public float[] threeLodWeights = new float[] { 0.1f, 0.4f, 0.4f };
        [BoxGroup("LOD Group Editor Settings")] public float[] fourLodWeights = new float[] {0.1f, 0.3f, 0.3f, 0.3f };
        [BoxGroup("LOD Group Editor Settings")] public float[] fiveLodWeights = new float[] { 0.1f, 0.2f, 0.2f, 0.2f, 0.2f };
        [BoxGroup("LOD Group Editor Settings")] public LODFadeMode fadeMode = LODFadeMode.CrossFade;
        [BoxGroup("LOD Group Editor Settings")] public float cullRatio = 0.1f;
    }
}