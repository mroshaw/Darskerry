#if INVECTOR_SHOOTER
using Invector.vCamera;
#endif
#if GPU_INSTANCER
using GPUInstancer;
#endif
using UnityEngine;

namespace DaftAppleGames.Common.Utils
{
    public static class GameUtils
    {

        /// <summary>
        /// Finds and returns the MainCamera component
        /// </summary>
        /// <returns></returns>
        public static Camera FindMainCamera()
        {
            GameObject cameraGameObject = GameObject.FindGameObjectWithTag("MainCamera");
            if (!cameraGameObject)
            {
                Debug.LogError("GameUtils.FindMainCamera: Can't find 'MainCamera' game object!");
                return null;
            }
            Camera cameraComponent = cameraGameObject.GetComponent<Camera>();
            if (!cameraComponent)
            {
                Debug.LogError("GameUtils.FindMainCamera: Can't find 'Camera' component on 'MainCamera' game object!");
                return null;
            }
            return cameraComponent;
        }

        #if GPU_INSTANCER
        
        /// <summary>
        /// Finds the GPU Instancer Detail Manager Game Object
        /// </summary>
        /// <returns></returns>
        public static GPUInstancerDetailManager FindGpuDetailManager()
        {
            GPUInstancerDetailManager gpuiDetailManager = GameObject.FindObjectOfType<GPUInstancerDetailManager>();
            if(!gpuiDetailManager)
            {
                Debug.Log("GameUtils.FindMainCamera: Can't find 'GPUI Detail Manager' game object!");
                return null;
            }
            return gpuiDetailManager;
        }
        #endif
        /// <summary>
        /// Finds the main Directional Light in the scene
        /// </summary>
        /// <returns></returns>
        public static Light FindMainDirectionalLight()
        {
            GameObject mainDirectionalLightGameObject = GameObject.Find("Sun and Moon Directional Light");
            if(!mainDirectionalLightGameObject)
            {
                Debug.Log("Couldn't find directional light Game Object!");
                return null;
            }
            Light mainDirectionalLight = mainDirectionalLightGameObject.GetComponent<Light>();
            if (!mainDirectionalLight)
            {
                Debug.Log("Couldn't find directional light!");
                return null;
            }
            return mainDirectionalLight;
        }

        /// <summary>
        /// Finds the Player GameObject in the scene
        /// </summary>
        /// <returns></returns>
        public static GameObject FindPlayerGameObject()
        {
            GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
            if (!playerGameObject)
            {
                Debug.LogError("GameUtils.FindPlayerGameObject: Can't find 'Player' game object!");
                return null;
            }

            return playerGameObject;
        }
#if INVECTOR_SHOOTER
        /// <summary>
        /// Finds the Invector Camera Game Object in the scene
        /// </summary>
        /// <returns></returns>
        public static vThirdPersonCamera FindInvectorCamera()
        {
            vThirdPersonCamera vCamera = GameObject.FindObjectOfType<vThirdPersonCamera>();
            return vCamera;
        }
#endif
    }
}
