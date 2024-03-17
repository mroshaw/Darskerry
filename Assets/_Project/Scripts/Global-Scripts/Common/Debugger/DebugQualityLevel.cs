using DaftAppleGames.Common.Utils;
using TMPro;
using UnityEngine;
#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif
namespace DaftAppleGames.Common.Debugger
{
    public class DebugQualityLevel : MonoBehaviour
    {
        [Header("UI Settings")]
        public TMP_Text qualityLevelText;
        public TMP_Text aaMode;
        public TMP_Text aaQuality;
        public TMP_Text textureQuality;

        private Camera _mainCamera;

        /// <summary>
        /// Update control
        /// </summary>
        public void Update()
        {
            qualityLevelText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
            _mainCamera = GameUtils.FindMainCamera();

            #if UNITY_POST_PROCESSING_STACK_V2
            if (_mainCamera)
            {
                aaMode.text = _mainCamera.GetComponent<PostProcessLayer>().antialiasingMode.ToString();
            }
            #endif
            aaQuality.text = QualitySettings.antiAliasing.ToString();

            textureQuality.text = QualitySettings.globalTextureMipmapLimit.ToString();
        }
    }
}
