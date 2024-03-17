#if GPU_INSTANCER
using UnityEngine;
using DaftAppleGames.Common.GameControllers;
using GPUInstancer;

namespace DaftAppleGames.Common.CameraTools
{
    public class AddCameraToGPUIManager : MonoBehaviour
    {
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Start()
        {
            Debug.Log("Setting GPUI Manager Camera...");
            GPUInstancerManager manager = GetComponent<GPUInstancerManager>();
            GPUInstancerAPI.SetCamera(manager, PlayerCameraManager.Instance.MainCamera);
            Debug.Log("Setting GPUI Manager Camera... Done.");
        }
    }
}
#endif