#if ENVIRO_3
using Enviro;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.CameraTools
{
    public class AddCameraToEnviro : MonoBehaviour
    {
        private void Start()
        {
            if (EnviroManager.instance)
            {
                EnviroCameras newCamera = new EnviroCameras();
                newCamera.camera = GetComponent<Camera>();
                EnviroManager.instance.Cameras.Add(newCamera);
            }
        }
    }
}
#endif