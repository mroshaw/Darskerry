using DaftAppleGames.Common.GameControllers;
#if ASMDEF
#if INVECTOR_SHOOTER
using Invector.vCamera;
#endif
#endif
using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    public class AddMainCamera : MonoBehaviour
    {
#if ASMDEF
#if INVECTOR_SHOOTER    
        private vThirdPersonCamera vCamera;
#endif
#endif
        private GameObject _mainCameraGameObject;
        /// <summary>
        /// Add the Main Camera game object as a child
        /// </summary>
        private void Start()
        {
            _mainCameraGameObject = PlayerCameraManager.Instance.MainCamera.gameObject;
            if (!_mainCameraGameObject)
            {
                Debug.LogError("AddMainCamera: Can't find 'MainCamera' game object!!!");
                return;
            }
#if ASMDEF
#if INVECTOR_SHOOTER
            vCamera = GetComponent<vThirdPersonCamera>();

            if (!vCamera)
            {
                Debug.LogError("AddMainCamera: Can't find 'vThirdPersonCamera' component!!!");
                return;
            }
#endif
#endif
            // Update the main camera
            UpdateMainCamera();
        }

        /// <summary>
        /// Set the Main Camera as child
        /// </summary>
        public void UpdateMainCamera()
        {
            _mainCameraGameObject.transform.SetParent(transform, false);
            _mainCameraGameObject.transform.localPosition = new Vector3(0, 0, 0);
            _mainCameraGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
