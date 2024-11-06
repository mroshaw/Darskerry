using ECM2;
using UnityEngine;


namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class HumanCharacter : Character
    {
        [SerializeField] private float rollSpeed = 1.0f;
        [SerializeField] private float maxSprintSpeed = 10.0f;

        private Vector3 _rollMovementDirection = Vector3.zero;

        private bool _isRolling;
        private bool _rollInputPressed;

        private bool _isSprinting;
        private bool _sprintInputPressed;

        private bool _isAttacking;
        private bool _attackInputPressed;

        #region Startup
        #endregion
        #region Update
        protected override void OnBeforeSimulationUpdate(float deltaTime)
        {
            // Call base method implementation
            base.OnBeforeSimulationUpdate(deltaTime);

            // Handle Sprinting
            CheckSprintInput();

            // Handle rolling
            CheckRollInput();
            HandlingRolling();

            // Handle attacking
            CheckAttackInput();
        }

        #endregion

        #region Attack methods
        public bool IsAttacking()
        {
            return _isAttacking;
        }

        public bool CanAttack()
        {
            return IsWalking() && !IsCrouched() && !IsRolling();
        }

        public void Attack()
        {
            _attackInputPressed = true;
        }

        public void StopAttacking()
        {
            _attackInputPressed = false;
        }

        public void CheckAttackInput()
        {
            if (!_isAttacking && _attackInputPressed && CanAttack())
            {
                _isAttacking = true;
            }
            if (_isAttacking && (!_attackInputPressed || !CanAttack()))
            {
                _isAttacking = false;
            }
        }
        #endregion
        #region Sprint methods
        public bool IsSprinting()
        {
            return _isSprinting;
        }

        private bool CanSprint()
        {
            return IsWalking() && !IsCrouched() && !IsRolling();
        }

        public void Sprint()
        {
            _sprintInputPressed = true;
        }

        public void StopSprinting()
        {
            _sprintInputPressed = false;
        }

        private void CheckSprintInput()
        {
            if (!_isSprinting && _sprintInputPressed && CanSprint())
            {
                _isSprinting = true;
            }
            else if (_isSprinting && (!_sprintInputPressed || !CanSprint()))
            {
                _isSprinting = false;
            }
        }
        #endregion

        #region Roll methods
        public bool IsRolling()
        {
            return _isRolling;
        }
        private bool CanRoll()
        {
            return IsWalking() || IsCrouched();
        }

        public void Roll()
        {
            _rollInputPressed = true;
        }

        public void StopRolling()
        {
            _isRolling = false;
        }

        public void StopRollingInput()
        {
            _rollInputPressed = false;
        }

        protected void CheckRollInput()
        {
            if (!_rollInputPressed || _isRolling)
                return;

            if (CanRoll())
            {
                _rollMovementDirection = GetMovementDirection();
                if (_rollMovementDirection == Vector3.zero)
                {
                    _rollMovementDirection = transform.forward * rollSpeed;
                }
                _isRolling = true;
            }
        }

        private void HandlingRolling()
        {
            if (!_isRolling)
            {
                return;
            }
            SetMovementDirection(_rollMovementDirection);
        }
        #endregion

        #region Character Overrides
        public override float GetMaxSpeed()
        {
            return _isSprinting ? maxSprintSpeed : base.GetMaxSpeed();
        }

        public float GetMaxAttainableSpeed()
        {
            return maxSprintSpeed > GetMaxSpeed() ? maxSprintSpeed : GetMaxSpeed();
        }
        #endregion
    }
}