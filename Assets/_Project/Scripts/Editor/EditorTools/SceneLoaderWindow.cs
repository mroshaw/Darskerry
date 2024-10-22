using System.Collections;
using DaftAppleGames.Darskerry.Core.Scenes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DaftAppleGames.Editor.BuildTool
{
    public class SceneLoaderWindow : OdinEditorWindow
    {
        [HideIf("@mainMenuSceneAsset != null")] [SerializeField] private SceneAsset mainMenuSceneAsset;
        [SerializeField] private Vector3 mainMenuSceneCameraPosition;
        [SerializeField] private Quaternion mainMenuSceneCameraRotation;
        [HideIf("@gameSceneAsset != null")] [SerializeField] private SceneAsset gameSceneAsset;
        [SerializeField] private Vector3 gameSceneCameraPosition;
        [SerializeField] private Quaternion gameSceneCameraRotation;

        // Display Editor Window
        [MenuItem("Daft Apple Games/Editor/Scene Loader")]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(SceneLoaderWindow));
            editorWindow.titleContent = new GUIContent("Scene Loader");
            editorWindow.Show();
        }
        [BoxGroup("Game Scenes")] [Button("Main Menu Scene", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void LoadMainMenuScenes()
        {
            OpenScene(mainMenuSceneAsset);
            CallAdditiveLoader(true);
        }

        [BoxGroup("Game Scenes")] [Button("Game Scene", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void LoadGameScenes()
        {
            OpenScene(gameSceneAsset);
            CallAdditiveLoader(false);
        }

        private void OpenScene(SceneAsset sceneAsset)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset), OpenSceneMode.Single);
        }

        private void CallAdditiveLoader(bool isMainMenu)
        {
            // Find the additive scene loader and load all scenes
            AdditiveSceneLoader additiveSceneLoader = Object.FindAnyObjectByType<AdditiveSceneLoader>();


            if (additiveSceneLoader)
            {
                /*
                    additiveSceneLoader.allScenesActivatedEvent.RemoveListener(PlaceGameSceneCamera);
                    additiveSceneLoader.allScenesActivatedEvent.RemoveListener(PlaceMainMenuSceneCamera);

                    if (isMainMenu)
                    {
                        additiveSceneLoader.allScenesActivatedEvent.AddListener(PlaceMainMenuSceneCamera);
                    }
                    else
                    {
                        additiveSceneLoader.allScenesActivatedEvent.AddListener(PlaceGameSceneCamera);
                    }
                */
                additiveSceneLoader.LoadAllScenes();
            }

        }

        private void PlaceMainMenuSceneCamera()
        {
            EditorCoroutineUtility.StartCoroutine(PlaceSceneCameraAsync(mainMenuSceneCameraPosition, mainMenuSceneCameraRotation), this);
        }

        private void PlaceGameSceneCamera()
        {
            EditorCoroutineUtility.StartCoroutine(PlaceSceneCameraAsync(gameSceneCameraPosition, gameSceneCameraRotation), this);
        }

        private IEnumerator PlaceSceneCameraAsync(Vector3 cameraPosition, Quaternion cameraRotation)
        {
            while (SceneView.lastActiveSceneView == null)
            {
                yield return null;
            }

            while (SceneView.lastActiveSceneView.camera == null)
            {
                yield return null;
            }

            Camera sceneCamera = SceneView.lastActiveSceneView.camera;
            if (sceneCamera)
            {
                sceneCamera.transform.position = cameraPosition;
                sceneCamera.transform.rotation = cameraRotation;
                Debug.Log("Scene View Camera moved!");
            }

        }

    }
}