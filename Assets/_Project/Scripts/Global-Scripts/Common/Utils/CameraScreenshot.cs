using UnityEngine;

namespace DaftAppleGames.Common.Utils
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class CameraScreenshot : MonoBehaviour
    {
        // Public properties
        [Header("Image Properties")] public int imageWidth;
        public int imageHeight;

        [Header("Output Settings")] public string outputPath;
        public string outputFileName;

        private Camera _camera;
        private RenderTexture _screenshotTexture;
        private int _screenshotCount = 1;
        
        /// <summary>
        /// Save the current Camera view to PNG
        /// </summary>
        public void SaveCameraToPng()
        {
            UpdateSettings();
            
            byte[] bytes = RenderTextureToTexture2D(_screenshotTexture).EncodeToPNG();
            string fullPath = $"{outputPath}\\{outputFileName}_{_screenshotCount.ToString()}.png";
            Debug.Log($"Writing file: {fullPath}");
            System.IO.File.WriteAllBytes($"{fullPath}", bytes);
            _screenshotCount++;
        }

        /// <summary>
        /// Convert RenderTexture to Texture2D
        /// </summary>
        /// <param name="renderTexture"></param>
        /// <returns></returns>
        private Texture2D RenderTextureToTexture2D(RenderTexture renderTexture)
        {
            // renderTexture.height = imageHeight;
            // renderTexture.width = imageWidth;

            Texture2D texture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Refresh the settings
        /// </summary>
        private void UpdateSettings()
        {
            _camera = GetComponent<Camera>();
            _screenshotTexture = _camera.targetTexture;
            outputFileName = _camera.gameObject.name;
        }

    }
}