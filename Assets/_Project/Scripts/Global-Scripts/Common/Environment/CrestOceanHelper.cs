#if CREST
using Crest;
using DaftAppleGames.Common.GameControllers;
#endif
using UnityEngine;

namespace DaftAppleGames.Common.Environment
{
    public class CrestOceanHelper : MonoBehaviour
    {
        #if CREST
        private OceanRenderer _crest; 
        
        /// <summary>
        /// Configure Crest at runtime
        /// </summary>
        private void Awake()
        {
            _crest = GetComponent<OceanRenderer>();
        }

        /// <summary>
        /// Set the Crest camera
        /// </summary>
        private void Start()
        {
            SetCamera();
        }
        
        /// <summary>
        /// Sets the Crest camera to the current Main Camera
        /// </summary>
        public void SetCamera()
        {
            _crest.ViewCamera = PlayerCameraManager.Instance.MainCamera;
        }
        
        /// <summary>
        /// Sets the Crest camera to the specified Camera
        /// </summary>
        /// <param name="camera"></param>
        public void SetCamera(Camera camera)
        {
            _crest.ViewCamera = camera;
        }
        #endif
    }
}
