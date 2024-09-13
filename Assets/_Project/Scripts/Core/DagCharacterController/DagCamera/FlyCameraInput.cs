using DaftAppleGames.Darskerry.Core.DagCharacterController.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = DaftAppleGames.Darskerry.Core.DagCharacterController.Input.PlayerInputManager;

namespace DaftAppleGames.Darskerry.Core.DagCamera
{
    public class FlyCameraInput : MonoBehaviour, PlayerControls.IFlyCamMapActions
    {
        public bool ForwardPressed { get; private set; }
        public bool BackwardPressed { get; private set; }
        public bool LeftPressed { get; private set; }
        public bool RightPressed { get; private set; }
        public bool UpPressed { get; private set; }
        public bool DownPressed { get; private set; }
        public bool ToggleFocusPressed { get; private set; }
        public bool SpeedUpPressed { get; private set; }

        public Vector2 MouseDelta { get; private set; }

        private void OnEnable()
        {
            PlayerInputManager.Instance.PlayerControls.FlyCamMap.Enable();
            PlayerInputManager.Instance.PlayerControls.FlyCamMap.SetCallbacks(this);
        }

        private void LateUpdate()
        {
            ToggleFocusPressed = false;
        }

        public void OnFlyForward(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                ForwardPressed = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                ForwardPressed = false;
            }
        }

        public void OnFlyBackward(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                BackwardPressed = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                BackwardPressed = false;
            }
        }

        public void OnFlyLeft(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                LeftPressed = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                LeftPressed = false;
            }
        }

        public void OnFlyRight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                RightPressed = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                RightPressed = false;
            }
        }

        public void OnFlyUp(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                UpPressed = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UpPressed = false;
            }
        }

        public void OnFlyDown(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                DownPressed = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                DownPressed = false;
            }
        }

        public void OnFlyToggleFocus(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ToggleFocusPressed = true;
            }
        }

        public void OnFlySpeedUp(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                SpeedUpPressed = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                SpeedUpPressed = false;
            }
        }

        public void OnFlyLook(InputAction.CallbackContext context)
        {
            MouseDelta = context.ReadValue<Vector2>();
        }
    }
}