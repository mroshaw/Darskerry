using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CameraTools
{
    [RequireComponent(typeof(Camera))]
    public class FlyCamera : MonoBehaviour
    {
        public float acceleration = 50; // how fast you accelerate
        public float accSprintMultiplier = 4; // how much faster you go when "sprinting"
        public float lookSensitivity = 0.6f; // mouse look sensitivity
        public float dampingCoefficient = 5; // how quickly you break to a halt after you stop your input
        public bool focusOnEnable = true; // whether or not to focus and lock cursor immediately on enable

        private Vector3 _velocity; // current velocity

        private FlyCameraInput _cameraInput;

        private static bool Focused
        {
            get => Cursor.lockState == CursorLockMode.Locked;
            set
            {
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = value == false;
            }
        }

        private void OnEnable()
        {
            if (focusOnEnable) Focused = true;
        }

        private void Awake()
        {
            _cameraInput = GetComponent<FlyCameraInput>();
        }

        private void OnDisable() => Focused = false;

        private void Update()
        {
            // Input
            if (Focused)
                UpdateInput();
            // else if (Input.GetMouseButtonDown(0))
            //    Focused = true;

            // Physics
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, dampingCoefficient * Time.deltaTime);
            transform.position += _velocity * Time.deltaTime;
        }

        private void UpdateInput()
        {
            // Position
            _velocity += GetAccelerationVector() * Time.deltaTime;

            // Rotation
            Vector2 mouseDelta = lookSensitivity * _cameraInput.MouseDelta;
            Quaternion rotation = transform.rotation;
            Quaternion horiz = Quaternion.AngleAxis(mouseDelta.x, Vector3.up);
            Quaternion vert = Quaternion.AngleAxis(mouseDelta.y, Vector3.right);
            transform.rotation = horiz * rotation * vert;

            // Leave cursor lock
            if (_cameraInput.ToggleFocusPressed)
                Focused = false;
        }

        private Vector3 GetAccelerationVector()
        {
            Vector3 moveInput = default;

            if (_cameraInput.ForwardPressed)
            {
                moveInput += Vector3.forward;
            }

            if (_cameraInput.BackwardPressed)
            {
                moveInput += Vector3.back;
            }

            if (_cameraInput.LeftPressed)
            {
                moveInput += Vector3.left;
            }

            if (_cameraInput.RightPressed)
            {
                moveInput += Vector3.right;
            }

            if (_cameraInput.UpPressed)
            {
                moveInput += Vector3.up;
            }

            if (_cameraInput.DownPressed)
            {
                moveInput += Vector3.down;
            }

            Vector3 direction = transform.TransformVector(moveInput.normalized);

            if (_cameraInput.SpeedUpPressed)
                return direction * (acceleration * accSprintMultiplier); // "sprinting"
            return direction * acceleration; // "walking"
        }
    }
}