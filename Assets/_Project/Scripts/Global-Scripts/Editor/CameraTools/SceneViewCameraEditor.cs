using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.CameraTools
{
    public class SceneViewCameraEditor : OdinEditorWindow
    {
        
        [MenuItem("Window/Camera/Scene View Camera Tools")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SceneViewCameraEditor));
        }

        private Camera _mainCamera;
        private Camera _sceneCamera;
        
        /// <summary>
        /// Move the current Scene Camera to the position and rotation of the Main Camera
        /// </summary>
        [Button("Scene to Main")]
        private void MoveSceneCameraToMainCamera()
        {
            // Refresh Cameras
            GetMainCam();
            GetSceneCam();
            
            // Set SceneView Camera position
            _sceneCamera.transform.position = _mainCamera.transform.position;
            _sceneCamera.transform.rotation = _mainCamera.transform.rotation;

        }
        
        [Button("Main to Scene")]
        private void MoveMainCameraToSceneCamera()
        {
            // Refresh Cameras
            GetMainCam();
            GetSceneCam();
            
            // Set SceneView Camera position
            _mainCamera.transform.position = _sceneCamera.transform.position;
            _mainCamera.transform.rotation = _sceneCamera.transform.rotation;
        }

        /// <summary>
        /// Finds the current Scene View Camera
        /// </summary>
        private void GetSceneCam()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            _sceneCamera = sceneView.camera;

        }

        /// <summary>
        /// Finds the MainCamera tagged Camera
        /// </summary>
        private void GetMainCam()
        {
            // Find Main Camera
            _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
    }
}
