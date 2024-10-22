using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.TerrainEditor
{
    public class TerrainObjectReplacerWindow : OdinEditorWindow
    {
        [BoxGroup("Terrain Settings")]
        [SerializeField] private Terrain selectedTerrain;

        const string ConvertedParentName = "Terrain Trees";
        private const string TreesGeneratedName = "TREES_GENERATED";

        [MenuItem("Daft Apple Games/Terrains/Terrain Object Replacer")]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(TerrainObjectReplacerWindow));
            editorWindow.titleContent = new GUIContent("Terrain Replacer");
            editorWindow.Show();
        }

        [BoxGroup("Chosen Terrain")] [Button("Convert Terrain Trees")]
        private void ReplaceTerrainTreesSelectedButton()
        {
            if (selectedTerrain)
            {
                Convert(selectedTerrain, ConvertedParentName);
            }
        }

        [BoxGroup("Chosen Terrain")]
        [Button("Clear Trees")]
        private void ClearTreesSelectedButton()
        {
            if (selectedTerrain)
            {
                ClearTerrainTrees(selectedTerrain);
            }
        }

        [BoxGroup("All Terrains")] [Button("Convert Terrain Trees")]
        private void ReplaceTerrainTreeAllButton()
        {
            Terrain[] allTerrains = GetAllTerrains();
            foreach (Terrain currTerrain in allTerrains)
            {
                Convert(currTerrain, $"{ConvertedParentName}_{currTerrain.name}");
            }
        }

        [BoxGroup("All Terrains")]
        [Button("Clear Trees")]
        private void ClearTreesAllButton()
        {
            Terrain[] allTerrains = GetAllTerrains();
            foreach (Terrain currTerrain in allTerrains)
            {
                ClearTerrainTrees(currTerrain);
            }
        }

        /// <summary>
        /// Gets all Terrain trees from the given terrain and created GameObject instances under a new GameObject
        /// called "parentName".
        /// </summary>
        private void Convert(Terrain convertTerrain, string parentName)
        {
            TerrainData data = convertTerrain.terrainData;
            float width = data.size.x;
            float height = data.size.z;
            float y = data.size.y;

            // Create parent
            GameObject parent = GameObject.Find(parentName);

            if (parent == null)
            {
                parent = new GameObject(parentName);
            }

            parent.transform.parent = convertTerrain.transform;
            parent.transform.localPosition = Vector3.zero;

            // Create trees
            for (int currTreeIndex = 0; currTreeIndex < data.treeInstances.Length; currTreeIndex++)
            {
                TreeInstance tree = data.treeInstances[currTreeIndex];
                GameObject _tree = data.treePrototypes[tree.prototypeIndex].prefab;

                //Vector3 position = new Vector3(tree.position.x * width, tree.position.y * y, tree.position.z * height);
                Vector3 position = Vector3.Scale(tree.position, data.size) + convertTerrain.transform.position;

                // Instantiate as Prefab if is one, if not, instantiate as normal
                GameObject newTreeGameObject = PrefabUtility.InstantiatePrefab(_tree) as GameObject;

                if (newTreeGameObject != null)
                {
                    newTreeGameObject.name = $"{newTreeGameObject.name} ({currTreeIndex})";
                    newTreeGameObject.transform.position = position;
                    newTreeGameObject.transform.parent = parent.transform;
                }
                else
                {
                    newTreeGameObject = Instantiate(_tree, position, Quaternion.identity, parent.transform);
                }

                Transform treeTransform = newTreeGameObject.transform;
                // set correct Scale of tree instance
                treeTransform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                // set random rotation
                RotateTrees(treeTransform.transform);
                // set correct slope of tree
                RotateToMatchTerrainSlope(treeTransform, convertTerrain);
            }
        }

        private Terrain[] GetAllTerrains()
        {
            return FindObjectsByType<Terrain>(FindObjectsSortMode.None);
        }

        public void ClearAll()
        {
            List<GameObject> allGeneratedTrees = new List<GameObject>();

            for (int i = GetAllTerrains().Length - 1; i >= 0; i--)
            {
                allGeneratedTrees.Add(GameObject.Find(TreesGeneratedName + i));
            }

            for (int i = allGeneratedTrees.Count - 1; i >= 0; i--)
            {
                GameObject parentObj = allGeneratedTrees[i];
                allGeneratedTrees.Remove(parentObj);
                DestroyImmediate(parentObj);
            }
        }

        private  void ClearTerrainTrees(Terrain convertTerrain)
        {
            convertTerrain.terrainData.treeInstances = new List<TreeInstance>().ToArray();
        }

        private void RotateTrees(Transform target)
        {
            float randomRotation = Random.Range(0, 360);

            target.transform.RotateAround(target.transform.position, target.transform.up, randomRotation);
        }

        private void RotateToMatchTerrainSlope(Transform target, Terrain terrain)
        {
            if (Physics.Raycast(target.position, Vector3.down, out var hit))
            {
                target.rotation = Quaternion.FromToRotation(terrain.transform.up, hit.normal) * target.rotation;
            }
        }
    }
}