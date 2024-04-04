using Codice.Client.BaseCommands.Differences;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor
{
    public class LodGroupAutoEditor : OdinEditorWindow
    {
        [BoxGroup("LOD Group Editor Settings")] public float[] twoLodWeights = new float[] { 0.9f, 0.0f };
        [BoxGroup("LOD Group Editor Settings")] public float[] threeLodWeights = new float[] { 0.9f, 0.5f, 0.0f };
        [BoxGroup("LOD Group Editor Settings")] public float[] fourLodWeights = new float[] {0.9f, 0.5f, 0.3f, 0.0f };
        [BoxGroup("LOD Group Editor Settings")] public float[] fiveLodWeights = new float[] { 0.9f, 0.5f, 0.3f, 0.2f, 0.1f };
        [BoxGroup("LOD Group Editor Settings")] public LODFadeMode fadeMode = LODFadeMode.CrossFade;
        [BoxGroup("LOD Group Editor Settings")] public float cullRatio = 0.01f;

        [MenuItem("Window/Scene Tools/LOD Group Fixer")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LodGroupAutoEditor));
        }

        [Button("Configure Selected")]
        private void ConfigureSelected()
        {
            foreach (GameObject prefabItem in Selection.gameObjects)
            {
                string prefabPath = AssetDatabase.GetAssetPath(prefabItem);
                Debug.Log($"Asset Path: {prefabPath}");
                GameObject assetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                ConfigureLodGroup(assetPrefab);
            }
        }

        private void ConfigureLodGroup(GameObject prefabGameObject)
        {
            LODGroup lodGroup = prefabGameObject.GetComponent<LODGroup>();
            if (!lodGroup)
            {
                Debug.Log($"No LODGroup on {prefabGameObject.name}");
                return;
            }
            lodGroup.fadeMode = fadeMode;
            int numberOfLods = lodGroup.lodCount;
            Debug.Log($"Number of LODs in {prefabGameObject.name}: {numberOfLods}");
            LOD[] lods = lodGroup.GetLODs();

            if(numberOfLods == 0)
            {
                Debug.Log($"LODGroup has no LODs on {prefabGameObject.name}");
                return;
            }

            float[] lodWeights = new []{0.0f};

            switch (numberOfLods)
            {
                case 2:
                    lodWeights = twoLodWeights;
                    break;
                case 3:
                    lodWeights = threeLodWeights;
                    break;
                case 4:
                    lodWeights = fourLodWeights;
                    break;
                case 5:
                    lodWeights = fiveLodWeights;
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

            float maxLength = 1 - cullRatio;
            float curLodPos = 1;
            for (int j = 0; j < lods.Length; j++)
            {
                float weight = j < lodWeights.Length ? lodWeights[j] : lodWeights[lodWeights.Length - 1];

                float lengthRatio = weightSum != 0 ? weight / weightSum : 1;

                float lodLength = maxLength * lengthRatio;
                curLodPos = curLodPos - lodLength;

                lods[j].screenRelativeTransitionHeight = curLodPos;
            }

            for(int i=0; i < lods.Length; i++)
            {
                Debug.Log($"Lod {i}: {lods[i].screenRelativeTransitionHeight}");
            }

            lodGroup.SetLODs(lods);

            // Recalculate bounds
            lodGroup.RecalculateBounds();

            EditorUtility.SetDirty(prefabGameObject);

        }
    }
}
