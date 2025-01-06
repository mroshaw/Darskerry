using JBooth.MicroSplat;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

namespace Assets._Project.Scripts.Editor.EditorTools
{
    public class FavouritesWindow : OdinEditorWindow
    {
        // Display Editor Window
        [MenuItem("Daft Apple Games/Editor/Favourites")]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(FavouritesWindow));
            editorWindow.titleContent = new GUIContent("Favourites");
            editorWindow.Show();
        }

        [Button("Terrain")]
        private void SelectTerrain()
        {
            Selection.activeObject = FindFirstObjectByType<Terrain>();
        }

        [Button("MicroSplat")]
        private void SelectMicroSplat()
        {
            Terrain terrain = FindFirstObjectByType<Terrain>();
            if (!terrain)
            {
                return;
            }

            MicroSplatTerrain microSplat = terrain.GetComponent<MicroSplatTerrain>();
            if (!microSplat)
            {
                return;
            }

            Material templateMaterial = microSplat.templateMaterial;
            string microSplatAssetPath = AssetDatabase.GetAssetPath(templateMaterial);
            Selection.activeObject = templateMaterial;
        }

        [Button("Refresh NavMesh")]
        private void RefreshNavMesh()
        {
            Terrain terrain = FindFirstObjectByType<Terrain>();
            if (!terrain)
            {
                return;
            }

            foreach (NavMeshSurface surface in terrain.GetComponentsInChildren<NavMeshSurface>())
            {
                Debug.Log($"Refreshing NavMeshSurface: {surface.agentTypeID}");
                surface.BuildNavMesh();
            }
        }
    }
}