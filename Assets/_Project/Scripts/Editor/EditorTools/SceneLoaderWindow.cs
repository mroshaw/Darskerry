using DaftAppleGames.Darskerry.Core.Scenes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildTool
{
    public class SceneLoaderWindow : OdinEditorWindow
    {
        [HideIf("@mainMenuSceneAsset != null")][SerializeField] private SceneAsset mainMenuSceneAsset;

        [HideIf("@gameSceneAsset != null")][SerializeField] private SceneAsset gameSceneAsset;

        [HideIf("@gameWorldSceneAsset != null")][SerializeField] private SceneAsset gameWorldSceneAsset;

        [HideIf("@emptySceneAsset != null")][SerializeField] private SceneAsset emptySceneAsset;

        // Display Editor Window
        [MenuItem("Daft Apple Games/Editor/Scene Loader")]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(SceneLoaderWindow));
            editorWindow.titleContent = new GUIContent("Scene Loader");
            editorWindow.Show();
        }

        [BoxGroup("Game Scenes")]
        [Button("Main Menu", ButtonSizes.Medium), GUIColor(0, 1, 0)]
        private void LoadMainMenuScenes()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(mainMenuSceneAsset);
            CallAdditiveLoader();
        }

        [BoxGroup("Game Scenes")]
        [Button("Game", ButtonSizes.Medium), GUIColor(0, 1, 0)]
        private void LoadGameScenes()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(gameSceneAsset);
            CallAdditiveLoader();
        }

        [BoxGroup("Editor Scenes")]
        [Button("World Editor", ButtonSizes.Medium)]
        private void LoadGameWorldScenes()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(gameWorldSceneAsset);
            CallAdditiveLoader();
        }

        [BoxGroup("Editor Scenes")]
        [Button("Empty Scene", ButtonSizes.Medium)]
        private void LoadEmptyScene()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(emptySceneAsset);
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
    }
}