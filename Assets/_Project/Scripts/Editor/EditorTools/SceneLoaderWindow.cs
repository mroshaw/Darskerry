using System.Collections;
using DaftAppleGames.Darskerry.Core.Scenes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DaftAppleGames.Editor.BuildTool
{
    public class SceneLoaderWindow : OdinEditorWindow
    {
        [HideIf("@mainMenuSceneAsset != null")] [SerializeField]
        private SceneAsset mainMenuSceneAsset;

        [HideIf("@gameSceneAsset != null")] [SerializeField]
        private SceneAsset gameSceneAsset;

        // Display Editor Window
        [MenuItem("Daft Apple Games/Editor/Scene Loader")]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(SceneLoaderWindow));
            editorWindow.titleContent = new GUIContent("Scene Loader");
            editorWindow.Show();
        }

        [BoxGroup("Game Scenes")]
        [Button("Main Menu Scene", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void LoadMainMenuScenes()
        {
            SaveAllScenes();
            OpenScene(mainMenuSceneAsset);
            CallAdditiveLoader();
        }

        [BoxGroup("Game Scenes")]
        [Button("Game Scene", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void LoadGameScenes()
        {
            SaveAllScenes();
            OpenScene(gameSceneAsset);
            CallAdditiveLoader();
        }

        private void OpenScene(SceneAsset sceneAsset)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset), OpenSceneMode.Single);
        }

        private void CallAdditiveLoader()
        {
            // Find the additive scene loader and load all scenes
            AdditiveSceneLoader additiveSceneLoader = Object.FindAnyObjectByType<AdditiveSceneLoader>();

            if (additiveSceneLoader)
            {
                additiveSceneLoader.LoadAllScenes();
            }
        }

        /// <summary>
        /// Saves all open scenes
        /// </summary>
        private void SaveAllScenes()
        {
            int numScenes = EditorSceneManager.sceneCount;
            for (int currSceneIndex = 0; currSceneIndex < numScenes; currSceneIndex++)
            {
                EditorSceneManager.SaveScene(SceneManager.GetSceneAt(currSceneIndex));
            }
        }
    }
}