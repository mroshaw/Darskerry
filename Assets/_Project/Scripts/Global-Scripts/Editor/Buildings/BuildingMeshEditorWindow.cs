using System;
using DaftAppleGames.Editor.Common;
using DaftAppleGames.Editor.Common.Performance;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    
    public class BuildingMeshEditorWindow : OdinEditorWindow
    {
        // Display Editor Window
        [MenuItem("Window/Buildings/Building Mesh Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BuildingMeshEditorWindow));
        }

        /// <summary>
        /// Refresh the selected list when scene object selection changes
        /// </summary>
        private void OnSelectionChange()
        {
            selectedGameObjects = Selection.gameObjects;
        }

        // UI layout
        // Source Objects
        [BoxGroup("Source Objects", centerLabel: true)]
        public GameObject[] selectedGameObjects;
        
        // Preset and settings
        [BoxGroup("Mesh Settings")]
        public MeshSettings meshSettings;
        
        [Button("Configure Meshes")]
        private void BuildingMeshesButton()
        {
            if (Selection.gameObjects.Length > 0)
            {
                UpdateMeshes(Selection.gameObjects, meshSettings);
            }
        }
        
        /// <summary>
        /// Applies Mesh changes across buildings
        /// </summary>
        private void UpdateMeshes(GameObject[] gameObjects, MeshSettings settings)
        {
            Debug.Log($"Update Meshes - Processing...");
            // Update selected meshes
            MeshRenderer[] meshs = gameObjects[0].GetComponentsInChildren<MeshRenderer>(true);
            MeshTools.UpdateMeshProperties(meshs, settings);

            Debug.Log($"Update Meshes - Done Processing.");
        }
    }
}
