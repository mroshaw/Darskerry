using DaftAppleGames.Scenes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets._Project.Scripts.Editor.EditorTools
{
    public class SceneLoaderWindow : OdinEditorWindow
    {
        [HideIf("@sceneLoaderSettings != null")] [SerializeField] private SceneLoaderSettings sceneLoaderSettings;

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
            OpenScene(sceneLoaderSettings.mainMenuSceneAsset);
            CallAdditiveLoader();
        }

        [BoxGroup("Game Scenes")]
        [Button("Game", ButtonSizes.Medium), GUIColor(0, 1, 0)]
        private void LoadGameScenes()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(sceneLoaderSettings.gameSceneAsset);
            CallAdditiveLoader();
        }

        [BoxGroup("Editor Scenes")]
        [Button("World Editor", ButtonSizes.Medium)]
        private void LoadGameWorldScenes()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(sceneLoaderSettings.gameWorldSceneAsset);
            CallAdditiveLoader();
        }

        [BoxGroup("Editor Scenes")]
        [Button("Empty Scene", ButtonSizes.Medium)]
        private void LoadEmptyScene()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(sceneLoaderSettings.emptySceneAsset);
            CallAdditiveLoader();
        }

        private void OpenScene(SceneAsset sceneAsset)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset), OpenSceneMode.Single);
        }

        private void CallAdditiveLoader()
        {
            // Find the additive scene loader and load all scenes
            AdditiveSceneLoader sceneLoader = Object.FindAnyObjectByType<AdditiveSceneLoader>();

            if (sceneLoader)
            {
                sceneLoader.LoadAndActivateAllScenes();
            }
        }
    }
}