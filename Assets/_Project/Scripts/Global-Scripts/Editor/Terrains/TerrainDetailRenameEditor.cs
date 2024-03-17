using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

using UnityEngine;

namespace DaftAppleGames.Editor.Terrains
{
    public class TerrainDetailRenameEditor : OdinEditorWindow
    {
        [MenuItem("Window/Terrains/TerrainDetailsRenamer")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TerrainDetailRenameEditor));
        }
    
        // UI layout
        [FoldoutGroup("Grass and Flower Settings")]
        public bool prefabMode = false;
        
        [Button("Rename Grass & Flowers", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void RenameFlowers()
        {

            foreach (GameObject currGameObject in Selection.gameObjects)
            {
                if (!prefabMode)
                {
                    currGameObject.name = GetNewName(currGameObject.name);
                }
                else
                {
                    // GameObject prefab = (GameObject) PrefabUtility.GetPrefabInstanceHandle(currGameObject);
                    string path = AssetDatabase.GetAssetPath(currGameObject);
                    string newName = GetNewName(currGameObject.name);
                    Debug.Log($"{path}, {newName}");
                    AssetDatabase.RenameAsset(path, newName);
                }
            }
        }

        private string GetNewName(string oldName)
        {
            string currName = oldName;
            string newName = currName;

            newName = newName.Replace("prefab", "");
            newName = newName.Replace("Prefab", "");
            newName = newName.Replace("flower", "");
            newName = newName.Replace("Flower", "");
            newName = newName.Replace("variant", "");
            newName = newName.Replace("Variant", "");
            newName = newName.Replace(" ", "");
            newName = newName.Replace("01", "");
            newName = newName.Replace("__", "_");
            newName = newName.Trim(new char[] { '_', ' ' });

            string flowerName = GetFlowerColour(currName);
            if (flowerName != "" && !newName.Contains(flowerName))
            {
                newName = newName + "_" + flowerName;
            }

            Debug.Log($"Changing {currName} to {newName}...");
            return newName;
        }

        /// <summary>
        /// Returns the colour given a flower prefab name
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private string GetFlowerColour(string objectName)
        {
            if (objectName.ToLower().Contains("bouncing")) return "white";
            if (objectName.ToLower().Contains("chamomile")) return "white";
            if (objectName.ToLower().Contains("chicory")) return "blue";
            if (objectName.ToLower().Contains("cornflower")) return "blue";
            if (objectName.ToLower().Contains("knapweed")) return "purple";
            if (objectName.ToLower().Contains("daisy")) return "white";
            if (objectName.ToLower().Contains("poppy")) return "red";
            if (objectName.ToLower().Contains("sunroot")) return "yellow";

            return "";
        }
    }
}
