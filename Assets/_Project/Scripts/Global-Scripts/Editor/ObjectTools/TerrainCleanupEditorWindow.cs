using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.ObjectTools
{
    public class TerrainCleanupEditorWindow : OdinEditorWindow
    {
        [MenuItem("Daft Apple Games/Tools/Objects/Terrain cleanup")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TerrainCleanupEditorWindow));
        }

        [Button("Sync Terrain Data")]
        private void SyncTerrainData()
        {
            if (!IsSelectionValidTerrain())
            {
                return;
            }

            Terrain selectedTerrain = Selection.gameObjects[0].GetComponent<Terrain>();
            TerrainData terrainData = selectedTerrain.terrainData;

            terrainData.SyncTexture(TerrainData.HolesTextureName);
            Debug.Log("Terrain data sync'd!");
        }

        [Button("List terrain holes")]
        private void ListTerrainHoles()
        {
            if (!IsSelectionValidTerrain())
            {
                return;
            }

            Terrain selectedTerrain = Selection.gameObjects[0].GetComponent<Terrain>();
            TerrainData terrainData = selectedTerrain.terrainData;

            bool holeFound = false;

            bool[,] holes = terrainData.GetHoles(0, 0, terrainData.heightmapResolution-1, terrainData.heightmapResolution-1);

            foreach (bool hole in holes)
            {
                // List holes
                if (!hole)
                {
                    Debug.Log("Found hole: ");
                    holeFound = true;
                }
            }

            if (!holeFound)
            {
                Debug.Log("No holes found in terrain!");
            }
        }

        /// <summary>
        /// Check if the selected object is a Terrain
        /// </summary>
        /// <returns></returns>
        private bool IsSelectionValidTerrain()
        {
            if (Selection.gameObjects.Length == 0)
            {
                Debug.Log("Please select a terrain gameobject in the scene!");
                return false;
            }

            if (!Selection.gameObjects[0].GetComponent<Terrain>())
            {
                Debug.Log("Please select a terrain gameobject in the scene!");
                return false;
            }
            return true;
        }
    }
}