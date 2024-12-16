using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.Mesh
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LodGroupSettings", menuName = "Daft Apple Games/Objects/LOD group settings", order = 1)]
    public class LodGroupSettingsSO : ScriptableObject
    {
        public LodGroupSettings LodGroupSettings;
    }

    [Serializable]
    public class LodGroupSettings
    {
        [BoxGroup("LOD Group Settings")] public float[] twoLodWeights = new float[] { 0.9f, 0.1f };
        [BoxGroup("LOD Group Settings")] public float[] threeLodWeights = new float[] { 0.6f, 0.2f, 0.2f };
        [BoxGroup("LOD Group Settings")] public float[] fourLodWeights = new float[] { 0.6f, 0.2f, 0.1f, 0.1f };
        [BoxGroup("LOD Group Settings")] public float[] fiveLodWeights = new float[] { 0.6f, 0.1f, 0.1f, 0.1f, 0.1f };
        [BoxGroup("LOD Group Settings")] public float[] sixLodWeights = new float[] { 0.5f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f };
        [BoxGroup("LOD Group Settings")] public LODFadeMode fadeMode = LODFadeMode.CrossFade;
        [BoxGroup("LOD Group Settings")] public float cullRatio = 0.0f;
    }
}