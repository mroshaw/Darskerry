#if GPU_INSTANCER
using System.Collections;
using GPUInstancer;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CameraTools
{
    [RequireComponent(typeof(Camera))]
    public class RegisterCameraGpuInstancer : MonoBehaviour
    {

        private void Awake()
        {
            StartCoroutine(SetCameraAsync());
        }

        private void Start()
        {
            // GPUInstancerAPI.SetCamera(GetComponent<Camera>());
        }

        private IEnumerator SetCameraAsync()
        {
            while (GPUInstancerManager.activeManagerList == null)
            {
                yield return null;
            }
            GPUInstancerAPI.SetCamera(GetComponent<Camera>());
            Debug.Log("GPU Instance camera set");
        }
    }
}
#endif