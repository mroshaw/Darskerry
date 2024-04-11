using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.ObjectTools
{
    public class LodGroupEditorWindow : OdinEditorWindow
    {
        [BoxGroup("Object Settings")] public LodGroupSettings lodGroupSettings;
        [BoxGroup("Selected Objects")] public bool prefabMode;

        [SerializeField]
        [BoxGroup("Selected Objects")] private GameObject[] _selectedGameObjects;

        [MenuItem("Daft Apple Games/Tools/Objects/LOD group editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LodGroupEditorWindow));
        }

        /// <summary>
        /// Update the list of selected objects
        /// </summary>
        private void OnSelectionChange()
        {
            _selectedGameObjects = Selection.gameObjects;
        }

        [Button("Configure Selected")]
        private void ConfigureSelected()
        {
            foreach (GameObject currGameObject in Selection.gameObjects)
            {
                if (prefabMode)
                {
                    string prefabPath = AssetDatabase.GetAssetPath(currGameObject);
                    Debug.Log($"Asset Path: {prefabPath}");
                    GameObject assetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                    ConfigureLodGroup(assetPrefab);
                }
                else
                {
                    ConfigureLodGroup(currGameObject);
                }
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
            lodGroup.fadeMode = lodGroupSettings.fadeMode;
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