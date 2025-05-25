using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.Mesh
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LodGroupSettings", menuName = "Daft Apple Games/Meshes/LOD group settings", order = 1)]
    public class LodGroupSettingsSO : ScriptableObject
    {
        [SerializeField] private LodGroupSettings lodGroupSettings;

        public void ConfigureLodGroup(LODGroup lodGroup)
        {
            lodGroup.fadeMode = lodGroupSettings.fadeMode;
            int numberOfLods = lodGroup.lodCount;
            Debug.Log($"Number of LODs in {lodGroup.name}: {numberOfLods}");

            if (numberOfLods > 6)
            {
                Debug.LogError($"{lodGroup.name} has {numberOfLods} LODs. Maximum of 6 is supported.");
                return;
            }

            LOD[] lods = lodGroup.GetLODs();

            if (numberOfLods == 0)
            {
                Debug.Log($"LODGroup has no LODs on {lodGroup.name}");
                return;
            }

            float[] lodWeights = new[] { 0.0f };

            switch (numberOfLods)
            {
                case 2:
                    lodWeights = lodGroupSettings.twoLodWeights;
                    break;
                case 3:
                    lodWeights = lodGroupSettings.threeLodWeights;
                    break;
                case 4:
                    lodWeights = lodGroupSettings.fourLodWeights;
                    break;
                case 5:
                    lodWeights = lodGroupSettings.fiveLodWeights;
                    break;
                case 6:
                    lodWeights = lodGroupSettings.sixLodWeights;
                    break;
            }

            float weightSum = 0;
            for (int k = 0; k < lods.Length; k++)
            {

                if (k >= lodWeights.Length)
                {
                    weightSum += lodWeights[lodWeights.Length - 1];
                }
                else
                {
                    weightSum += lodWeights[k];
                }
            }

            Debug.Log($"Total LOD weights: {weightSum}");

            float maxLength = 1 - lodGroupSettings.cullRatio;
            float curLodPos = 1;
            for (int j = 0; j < lods.Length; j++)
            {
                float weight = j < lodWeights.Length ? lodWeights[j] : lodWeights[lodWeights.Length - 1];

                float lengthRatio = weightSum != 0 ? weight / weightSum : 1;

                float lodLength = maxLength * lengthRatio;
                curLodPos = curLodPos - lodLength;

                lods[j].screenRelativeTransitionHeight = curLodPos;
            }

            for (int i = 0; i < lods.Length; i++)
            {
                Debug.Log($"Lod {i}: {lods[i].screenRelativeTransitionHeight}");
            }

            lodGroup.SetLODs(lods);

            // Recalculate bounds
            lodGroup.RecalculateBounds();
        }
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