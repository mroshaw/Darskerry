using ECM2;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class PlayerCamera : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Components")] [SerializeField] private CinemachineCamera virtualCamera;

        [BoxGroup("Camera Settings")] [SerializeField]
        [BoxGroup("Camera Settings")] private Vector2 lookSensitivity = new(0.1f, 0.1f);
        [BoxGroup("Camera Settings")] private Vector2 gamepadlookSensitivity = new(1f, 1f);
        [BoxGroup("Camera Settings")][SerializeField] private float minPitch = -20.0f;
        [BoxGroup("Camera Settings")][SerializeField] private float maxPitch = 20.0f;

        [BoxGroup("Camera Settings")][SerializeField] private Transform followTarget;
        [BoxGroup("Camera Settings")][SerializeField] private float followDistance = 5.0f;
        [BoxGroup("Camera Settings")][SerializeField] private float followMinDistance = 2.0f;
        [BoxGroup("Camera Settings")][SerializeField] private float followMaxDistance = 10.0f;
        
        [BoxGroup("Input Settings")][SerializeField] private bool invertLook;
        [BoxGroup("Input Settings")][SerializeField] private bool gamepadInvertLook;

        private CinemachineThirdPersonFollow _thirdPersonFollow;

        private float _cameraTargetYaw;
        private float _cameraTargetPitch;
        private float _followDistanceSmoothVelocity;

        #endregion

        #region Debug values
        [BoxGroup("DEBUG")] public float CameraTargetYawDebug;
        [BoxGroup("DEBUG")] public float CameraTargetPitchDebug;

        #endregion

        #region Startup
        private void Awake()
        {
            _thirdPersonFollow = virtualCamera.GetComponent<CinemachineThirdPersonFollow>();
        }
        #endregion

        private void Update()
        {
            CameraTargetYawDebug = _cameraTargetYaw;
            CameraTargetPitchDebug = _cameraTargetPitch;
        }

        private void LateUpdate()
        {
            // Update cameraTarget rotation using our yaw and pitch values
            UpdateCamera();
        }

        public void Look(Vector2 lookInput)
        {
            AddControlYawInput(lookInput.x * lookSensitivity.x);
            AddControlPitchInput(lookInput.y * lookSensitivity.y, minPitch, maxPitch, invertLook);
        }

        public void LookGamePad(Vector2 lookInput)
        {
            AddControlYawInput(lookInput.x * gamepadlookSensitivity.x);
            AddControlPitchInput(lookInput.y * gamepadlookSensitivity.y, minPitch, maxPitch, gamepadInvertLook);
        }

        public void Scroll(float scrollInput)
        {
            AddControlZoomInput(scrollInput);
        }

        /// <summary>
        /// Add input (affecting Yaw). 
        /// This is applied to the followTarget's yaw rotation.
        /// </summary>

        public void AddControlYawInput(float value, float minValue = -180.0f, float maxValue = 180.0f)
        {
            if (value != 0.0f) _cameraTargetYaw = MathLib.ClampAngle(_cameraTargetYaw + value, minValue, maxValue);
        }

        /// <summary>
        /// Add input (affecting Pitch). 
        /// This is applied to the followTarget's pitch rotation.
        /// </summary>

        public void AddControlPitchInput(float value, float minValue, float maxValue, bool willInvertLook)
        {
            if (value == 0.0f)
                return;

            if (willInvertLook)
                value = -value;

            _cameraTargetPitch = MathLib.ClampAngle(_cameraTargetPitch + value, minValue, maxValue);
        }

        /// <summary>
        /// Adds input (affecting follow distance).
        /// </summary>
        public virtual void AddControlZoomInput(float value)
        {
            followDistance = Mathf.Clamp(followDistance - value, followMinDistance, followMaxDistance);
        }

        /// <summary>
        /// Update followTarget rotation using _cameraTargetYaw and _cameraTargetPitch values and its follow distance.
        /// </summary>
        private void UpdateCamera()
        {
            followTarget.transform.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0.0f);

            _thirdPersonFollow.CameraDistance =
                Mathf.SmoothDamp(_thirdPersonFollow.CameraDistance, followDistance, ref _followDistanceSmoothVelocity, 0.1f);
        }
    }
}