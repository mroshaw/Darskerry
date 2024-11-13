using DaftAppleGames.Darskerry.Core.CharController.AiController;
using UnityEngine;
using UnityEngine.InputSystem;


namespace DaftAppleGames.Darskerry.Core.CameraTools
{
    public class FlyCameraInput : MonoBehaviour, PlayerControls.IFlyCamControlsActions
    {
        public bool ForwardPressed { get; private set; }
        public bool BackwardPressed { get; private set; }
        public bool LeftPressed { get; private set; }
        public bool RightPressed { get; private set; }
        public bool UpPressed { get; private set; }
        public bool DownPressed { get; private set; }
        public bool ToggleFocusPressed { get; private set; }
        public bool SpeedUpPressed { get; private set; }

        public bool FlyLookEnabledPressed { get; private set; }

        public Vector2 MouseDelta { get; private set; }

        private void OnEnable()
        {
            PlayerCharacterInputManager.Instance.PlayerControls.FlyCamControls.Enable();
            PlayerCharacterInputManager.Instance.PlayerControls.FlyCamControls.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerCharacterInputManager.Instance.PlayerControls.FlyCamControls.Disable();
            PlayerCharacterInputManager.Instance.PlayerControls.FlyCamControls.RemoveCallbacks(this);
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
            if (FlyLookEnabledPressed)
            {
                MouseDelta = context.ReadValue<Vector2>();
            }
            else
            {
                MouseDelta = new Vector2(0, 0);
            }
        }

        public void OnFlyLookEnabled(InputAction.CallbackContext context)
        {

            if (context.phase == InputActionPhase.Started)
            {
                FlyLookEnabledPressed = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                FlyLookEnabledPressed = false;
            }
        }
    }
}