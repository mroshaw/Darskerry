using DaftAppleGames.Common.Buildings;
using DaftAppleGames.Editor.AutoEditor.Buildings;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    [CustomEditor(typeof(InteriorCullingController))]
    public class InteriorCullingControllerEditor : OdinEditor
    {
        public InteriorCullingController controller;
        public BuildingPropertiesEditorSettings settings;
        
        public override void OnInspectorGUI()
        {
            controller = target as InteriorCullingController;
            if (GUILayout.Button("Auto Populate"))
            {
                
                BuildingTools.ConfigureLayersAndStaticFlags(controller.gameObject, settings);
                
                // Mark prefab as "dirty" to allow saving
                UpdatePrefab();
            }
            DrawDefaultInspector();
        }
        
        public static void UpdatePrefab()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }
    }
}
