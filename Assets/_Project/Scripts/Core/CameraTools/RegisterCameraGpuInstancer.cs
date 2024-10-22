#if GPU_INSTANCER
using GPUInstancer;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CameraTools
{
    [RequireComponent(typeof(Camera))]
    public class RegisterCameraGpuInstancer : MonoBehaviour
    {
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            GPUInstancerAPI.SetCamera(GetComponent<Camera>());
        }
    }
}
#endif