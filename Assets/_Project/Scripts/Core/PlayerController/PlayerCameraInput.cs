using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    [RequireComponent(typeof(PlayerCamera))]
    public class PlayerCameraInput : MonoBehaviour, PlayerControls.ICameraControlsActions
    {
        #region Debug values
        [BoxGroup("DEBUG")] public Vector2 LookDebug;
        [BoxGroup("DEBUG")] public Vector2 GamepadLookDebug;
        [BoxGroup("DEBUG")] public float ScrollInputDebug;
        #endregion

        #region Properties
        private PlayerCamera _playerCamera;
        private Vector2 _lookInputValue = Vector2.zero;
        private Vector2 _gamepadLookInputValue = Vector2.zero;
        #endregion

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

        #region Updates
        private void Update()
        {
            _playerCamera.Look(_lookInputValue);
            _playerCamera.LookGamePad(_gamepadLookInputValue);
        }
        #endregion

        #region Input Callbacks

        public void OnLookGamepad(InputAction.CallbackContext context)
        {
            _gamepadLookInputValue = context.ReadValue<Vector2>();
            GamepadLookDebug = _gamepadLookInputValue;
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            float scrollInput = context.ReadValue<Vector2>().y;
            ScrollInputDebug = scrollInput;
            _playerCamera.Scroll(scrollInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInputValue = context.ReadValue<Vector2>();
            LookDebug = _lookInputValue;
        }
        #endregion
    }
}