using UnityEngine;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    [RequireComponent(typeof(PlayerCamera))]
    public class PlayerCameraInput : MonoBehaviour, PlayerControls.ICameraControlsActions
    {
        private PlayerCamera _playerCamera;

        public Vector2 LookDebug;

        #region Startup

        private void Awake()
        {
            _playerCamera =GetComponent<PlayerCamera>();
        }

        private void OnEnable()
        {
            if (PlayerCharacterInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot enable");
                return;
            }

            PlayerCharacterInputManager.Instance.PlayerControls.CameraControls.Enable();
            PlayerCharacterInputManager.Instance.PlayerControls.CameraControls.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerCharacterInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot disable");
                return;
            }

            PlayerCharacterInputManager.Instance.PlayerControls.CameraControls.Disable();
            PlayerCharacterInputManager.Instance.PlayerControls.CameraControls.RemoveCallbacks(this);
        }
  
        #endregion


        #region Input Callbacks

        public void OnLookGamepad(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            LookDebug = lookInput;
            _playerCamera.LookGamePad(lookInput);
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            float scrollInput = context.ReadValue<Vector2>().y;
            _playerCamera.Scroll(scrollInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            LookDebug = lookInput;
            _playerCamera.Look(lookInput);
        }
        #endregion
    }
}