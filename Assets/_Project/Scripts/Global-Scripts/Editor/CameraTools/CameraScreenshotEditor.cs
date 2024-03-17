using DaftAppleGames.Common.Utils;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.CameraTools
{
    [CustomEditor(typeof(CameraScreenshot))]
    public class CameraScreenshotEditor : OdinEditor
    {
        public CameraScreenshot cameraScreenshot;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            cameraScreenshot = target as CameraScreenshot;
            if (GUILayout.Button("Save Screenshot"))
            {
                cameraScreenshot.SaveCameraToPng();
            }
        }
    }
}