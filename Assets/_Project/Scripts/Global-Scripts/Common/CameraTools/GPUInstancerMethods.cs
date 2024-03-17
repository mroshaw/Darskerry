#if GPU_INSTANCER
using DaftAppleGames.Common.GameControllers;
using GPUInstancer;
using UnityEngine;

namespace DaftApplesGames.Common.CameraTools
{
    public class GPUInstancerMethods : MonoBehaviour
    {
        public void SetCamera()
        {
            Debug.Log("Setting GPU Instancer Main Camera");
            GPUInstancerAPI.SetCamera(PlayerCameraManager.Instance.MainCamera);
            Debug.Log("Setting GPU Instancer Main Camera. Done.");
        }
    }
}
#endif