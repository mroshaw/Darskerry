using UnityEngine;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.DagCharacterController.Input
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        #region Class Variables

        [Header("Input Settings")]
        public bool holdToSprint = true;
        public bool invertLook = false;

        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool SprintToggledOn { get; private set; }
        public bool WalkToggledOn { get; private set; }
        public bool RollPressed { get; private set; }
        public bool CrouchToggledOn { get; private set;  }
        public InputDevice ActiveDevice { get; private set; }

        #endregion

        private bool _isPaused = false;

        #region Startup
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot enable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Enable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.SetCallbacks(this);

            ActiveDevice = Keyboard.current;

        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot disable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Disable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }
        #endregion

        #region Late Update Logic
        private void LateUpdate()
        {
            JumpPressed = false;
 }
        #endregion

        public void PauseInput()
        {
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Disable();
        }

        public void UnpauseInput()
        {
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Enable();
        }

        #region Input Callbacks
        public void OnMovement(InputAction.CallbackContext context)
        {
            ActiveDevice = context.control.device;
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            ActiveDevice = context.control.device;
            if (ActiveDevice is Gamepad && invertLook)
            {
                // Invert the Y axis, if invertLook is selected
                LookInput = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y * -1);
                return;
            }
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnToggleSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintToggledOn = holdToSprint || !SprintToggledOn;
            }
            else if (context.canceled)
            {
                SprintToggledOn = !holdToSprint && SprintToggledOn;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            JumpPressed = true;
        }

        public void OnToggleWalk(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            WalkToggledOn = !WalkToggledOn;
        }

        public void OnToggleCrouch(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            CrouchToggledOn = !CrouchToggledOn;
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            RollPressed = true;
        }
        #endregion

        #region Animation Event methods
        public void SetRollPressedFalse()
        {
            RollPressed = false;
        }

        public void SetCrouchToggleToFalse()
        {
            CrouchToggledOn = false;
        }
        #endregion
    }
}