using DaftAppleGames.Darskerry.Core.DagCharacterController.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.DagCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        #region Class Variables

        [Header("Components")]
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private Camera _playerCamera;
        public float RotationMismatch { get; private set; } = 0f;
        public bool IsRotatingToTarget { get; private set; } = false;

        [Header("Base Movement")]
        public float walkAcceleration = 25f;

        public float walkSpeed = 2f;
        public float runAcceleration = 35f;
        public float runSpeed = 4f;
        public float sprintAcceleration = 50f;
        public float sprintSpeed = 7f;
        public float crouchAcceleration = 15f;
        public float crouchSpeed = 1.2f;
        public float rollingAcceleration = 25f;
        public float rollingSpeed = 2.5f;
        public float inAirAcceleration = 25f;
        public float drag = 20f;
        public float inAirDrag = 5f;
        public float gravity = 25f;
        public float terminalVelocity = 50f;
        public float jumpSpeed = 0.8f;
        public float movingThreshold = 0.01f;

        [Header("Animation")]
        public float playerModelRotationSpeed = 10f;

        public float rotateToTargetTime = 0.67f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;

        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;

        [Header("Gamepad Settings")]
        public float gamepadLookXMultiplier = 5.0f;

        public float gamepadLookYMultiplier = 5.0f;
        public float gamepadMoveRunThreshold = 0.5f;

        [Header("Environment Details")]
        [SerializeField] private LayerMask groundLayers;

        public float ForwardSpeed { get; private set; }
        public float LateralSpeed { get; private set; }
        public float VerticalSpeed { get; private set; }

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerActionsInput _playerActionInput;
        private PlayerState _playerState;

        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private bool _jumpedLastFrame = false;
        private bool _isRotatingClockwise = false;
        private float _rotatingToTargetTimer = 0f;
        private float _verticalVelocity = 0f;
        private float _antiBump;
        private float _stepOffset;

        private PlayerMovementState _lastMovementState = PlayerMovementState.Falling;

        #endregion

        #region Startup

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerActionInput = GetComponent<PlayerActionsInput>();
            _playerState = GetComponent<PlayerState>();

            _antiBump = sprintSpeed;
            _stepOffset = _characterController.stepOffset;
        }

        #endregion

        #region Update Logic

        private void Update()
        {
            UpdateMovementState();
            HandleVerticalMovement();
            HandleLateralMovement();
            UpdateCharacterVelocities();
        }

        private void UpdateCharacterVelocities()
        {
            ForwardSpeed = transform.InverseTransformDirection(_characterController.velocity).z;
            LateralSpeed = transform.InverseTransformDirection(_characterController.velocity).x;
            VerticalSpeed = transform.InverseTransformDirection(_characterController.velocity).y;
        }

        private void UpdateMovementState()
        {
            _lastMovementState = _playerState.CurrentPlayerMovementState;

            // Cancel roll if not able to do so
            if (_playerLocomotionInput.RollPressed && !CanRoll())
            {
                _playerLocomotionInput.SetRollPressedFalse();
            }

            bool canRun = CanRun();
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero; //order
            bool isMovingLaterally = IsMovingLaterally(); //matters
            bool isSprinting = isMovingLaterally && (!canRun || _playerLocomotionInput.SprintToggledOn);
            bool isCrouching = _playerLocomotionInput.CrouchToggledOn;
            bool isRolling = _playerLocomotionInput.RollPressed;
            bool isWalking;
            if (_playerLocomotionInput.ActiveDevice is Gamepad)
            {
                isWalking = _playerLocomotionInput.MovementInput.y < gamepadMoveRunThreshold;
            }
            else
            {
                isWalking = isMovingLaterally && (!canRun || _playerLocomotionInput.WalkToggledOn);
            }

            bool isGrounded = IsGrounded();

            PlayerMovementState lateralState = isRolling ? PlayerMovementState.Rolling :
                isCrouching ? PlayerMovementState.Crouching :
                isWalking ? PlayerMovementState.Walking :
                isSprinting ? PlayerMovementState.Sprinting :
                isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;

            _playerState.SetPlayerMovementState(lateralState);

            // Control Airborne State
            if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y > 0.0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y <= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else
            {
                _characterController.stepOffset = _stepOffset;
            }
        }

        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerState.InGroundedState();
            bool isRolling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Rolling;

            _verticalVelocity -= gravity * Time.deltaTime;

            if (isGrounded && _verticalVelocity < 0)
                _verticalVelocity = -_antiBump;

            if (_playerLocomotionInput.JumpPressed && isGrounded && !isRolling)
            {
                // Cancel crouch
                _playerLocomotionInput.SetCrouchToggleToFalse();
                _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3 * gravity);
                _jumpedLastFrame = true;
            }

            if (_playerState.IsStateGroundedState(_lastMovementState) && !isGrounded)
            {
                _verticalVelocity += _antiBump;
            }

            // Clamp at terminal velocity
            if (Mathf.Abs(_verticalVelocity) > Mathf.Abs(terminalVelocity))
            {
                _verticalVelocity = -1f * Mathf.Abs(terminalVelocity);
            }
        }

        private void HandleLateralMovement()
        {
            // Create quick references for current state
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isGrounded = _playerState.InGroundedState();
            bool isWalking = _playerState.CurrentPlayerMovementState == PlayerMovementState.Walking;
            bool isCrouching = _playerState.CurrentPlayerMovementState == PlayerMovementState.Crouching;
            bool isRolling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Rolling;

            // State dependent acceleration and speed
            float lateralAcceleration = !isGrounded ? inAirAcceleration :
                isRolling ? rollingAcceleration :
                isCrouching ? crouchAcceleration :
                isWalking ? walkAcceleration :
                isSprinting ? sprintAcceleration : runAcceleration;

            float clampLateralMagnitude = !isGrounded ? sprintSpeed :
                isRolling ? rollingSpeed :
                isCrouching ? crouchSpeed :
                isWalking ? walkSpeed :
                isSprinting ? sprintSpeed : runSpeed;

            Vector3 cameraForwardXZ =
                new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z)
                .normalized;

            // If rolling, then ignore the input and move forward
            Vector3 movementDirection = !isRolling ? (cameraRightXZ *  _playerLocomotionInput.MovementInput.x +
                                                      cameraForwardXZ * _playerLocomotionInput.MovementInput.y) : transform.forward;

            Vector3 movementDelta = movementDirection * (lateralAcceleration * Time.deltaTime);

            Vector3 newVelocity = _characterController.velocity + movementDelta;

            // Add drag to player
            float dragMagnitude = isGrounded ? drag : inAirDrag;
            Vector3 currentDrag = newVelocity.normalized * (dragMagnitude * Time.deltaTime);
            newVelocity = (newVelocity.magnitude > dragMagnitude * Time.deltaTime)
                ? newVelocity - currentDrag
                : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), clampLateralMagnitude);
            newVelocity.y += _verticalVelocity;
            newVelocity = !isGrounded ? HandleSteepWalls(newVelocity) : newVelocity;

            // Move character (Unity suggests only calling this once per tick)
            _characterController.Move(newVelocity * Time.deltaTime);
        }

        private Vector3 HandleSteepWalls(Vector3 velocity)
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, groundLayers);
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= _characterController.slopeLimit;

            if (!validAngle && _verticalVelocity < 0f)
                velocity = Vector3.ProjectOnPlane(velocity, normal);

            return velocity;
        }

        #endregion

        #region Late Update Logic

        private void LateUpdate()
        {
            UpdateCameraRotation();
        }

        private void UpdateCameraRotation()
        {
            float lookInputX = _playerLocomotionInput.LookInput.x;
            float lookInputY = _playerLocomotionInput.LookInput.y;

            if (_playerLocomotionInput.ActiveDevice is Gamepad)
            {
                lookInputX *= gamepadLookXMultiplier;
                lookInputY *= gamepadLookYMultiplier;
            }

            _cameraRotation.x += lookSenseH * lookInputX;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * lookInputY, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * lookInputX;

            float rotationTolerance = 90f;
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            IsRotatingToTarget = _rotatingToTargetTimer > 0;

            // ROTATE if we're not idling
            if (!isIdling)
            {
                RotatePlayerToTarget();
            }
            // If rotation mismatch not within tolerance, or rotate to target is active, ROTATE
            else if (Mathf.Abs(RotationMismatch) > rotationTolerance || IsRotatingToTarget)
            {
                UpdateIdleRotation(rotationTolerance);
            }

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);

            // Get angle between camera and player
            Vector3 camForwardProjectedXZ =
                new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 crossProduct = Vector3.Cross(transform.forward, camForwardProjectedXZ);
            float sign = Mathf.Sign(Vector3.Dot(crossProduct, transform.up));
            RotationMismatch = sign * Vector3.Angle(transform.forward, camForwardProjectedXZ);
        }

        private void UpdateIdleRotation(float rotationTolerance)
        {
            // Initiate new rotation direction
            if (Mathf.Abs(RotationMismatch) > rotationTolerance)
            {
                _rotatingToTargetTimer = rotateToTargetTime;
                _isRotatingClockwise = RotationMismatch > rotationTolerance;
            }

            _rotatingToTargetTimer -= Time.deltaTime;

            // Rotate player
            if (_isRotatingClockwise && RotationMismatch > 0f ||
                !_isRotatingClockwise && RotationMismatch < 0f)
            {
                RotatePlayerToTarget();
            }
        }

        private void RotatePlayerToTarget()
        {
            Quaternion targetRotationX = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX,
                playerModelRotationSpeed * Time.deltaTime);
        }

        #endregion

        #region State Checks

        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreshold;
        }

        private bool IsGrounded()
        {
            bool grounded = _playerState.InGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirborne();

            return grounded;
        }

        private bool IsGroundedWhileGrounded()
        {
            Vector3 spherePosition = new Vector3(transform.position.x,
                transform.position.y - _characterController.radius, transform.position.z);

            bool grounded = Physics.CheckSphere(spherePosition, _characterController.radius, groundLayers,
                QueryTriggerInteraction.Ignore);

            return grounded;
        }

        private bool IsGroundedWhileAirborne()
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, groundLayers);
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= _characterController.slopeLimit;

            return _characterController.isGrounded && validAngle;
        }

        private bool CanRun()
        {
            // This means player is moving diagonally at 45 degrees or forward, if so, we can run
            return _playerLocomotionInput.MovementInput.y >= Mathf.Abs(_playerLocomotionInput.MovementInput.x);
        }

        private bool CanRoll()
        {
            return _playerState.CurrentPlayerMovementState != PlayerMovementState.Jumping &&
                   _playerState.CurrentPlayerMovementState != PlayerMovementState.Falling;
        }

        #endregion
    }
}