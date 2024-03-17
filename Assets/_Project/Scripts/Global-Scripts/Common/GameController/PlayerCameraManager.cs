#if INVECTOR_SHOOTER
using Invector.vCamera;
#endif

using DaftAppleGames.Common.CameraTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.GameControllers
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [BoxGroup("Camera")] public Camera MainCamera;
#if INVECTOR_SHOOTER
        [BoxGroup("Camera")] public vThirdPersonCamera InvectorMainCamera;
#endif
        [BoxGroup("Lighting")] public Light MainDirectionalLight;
        [BoxGroup("Player")] [SerializeField] private GameObject _playerGameObject;

        /// <summary>
        /// Gets or sets the main player GameObject
        /// </summary>
        public GameObject PlayerGameObject
        {
            get { return _playerGameObject; }
            set
            {
                _playerGameObject = value;

#if INVECTOR_SHOOTER
                InvectorMainCamera = _playerGameObject.GetComponentInChildren<vThirdPersonCamera>();
#endif
                PlayerGameObjectUpdated();
            }
        }

        [FoldoutGroup("Events")] public UnityEvent OnPlayerChangedEvent;

        // Singleton static instance
        private static PlayerCameraManager _instance;
        public static PlayerCameraManager Instance { get { return _instance; } }

        /// <summary>
        /// Initialise the GameController Singleton instance
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        /// <summary>
        /// If camera is attached to a rotator, pause it
        /// </summary>
        public void PauseCameraRotation()
        {
            RotateCameraAroundObject rotator = MainCamera.GetComponent<RotateCameraAroundObject>();
            if (rotator)
            {
                rotator.Pause();
            }
        }

        /// <summary>
        /// If camera is attached to a rotator, unpause it
        /// </summary>
        public void ResumeCameraRotation()
        {
            RotateCameraAroundObject rotator = MainCamera.GetComponent<RotateCameraAroundObject>();
            if (rotator)
            {
                rotator.Resume();
            }
            
        }
        
        /// <summary>
        /// Invokves the UnityEvent when player game object is updated
        /// </summary>
        private void PlayerGameObjectUpdated()
        {
            if (OnPlayerChangedEvent != null)
            {
                // Debug.Log("Player GameObject has been updated!");
                OnPlayerChangedEvent.Invoke();
            }
        }
    }
}
