using ECM2;
using Sirenix.OdinInspector;
using UnityEngine;


namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class GameCharacter : Character
    {
        [BoxGroup("Movement Settings")][SerializeField] private float maxSprintSpeed = 10.0f;

        private bool _isAttacking;
        private bool _attackInputPressed;

        private bool _isSprinting;
        private bool _sprintInputPressed;

        #region Startup
        #endregion
        #region Update
        protected override void OnBeforeSimulationUpdate(float deltaTime)
        {
            // Call base method implementation
            base.OnBeforeSimulationUpdate(deltaTime);

            // Handle attacking
            CheckAttackInput();

            // Handle Sprinting
            CheckSprintInput();
        }

        #endregion
        #region Sprint methods
        public bool IsSprinting()
        {
            return _isSprinting;
        }

        protected virtual bool CanSprint()
        {
            return IsWalking() && !IsCrouched();
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
        public float GetMaxAttainableSpeed()
        {
            return maxSprintSpeed > GetMaxSpeed() ? maxSprintSpeed : GetMaxSpeed();
        }
        #endregion
        #region Attack methods
        public bool IsAttacking()
        {
            return _isAttacking;
        }

        protected virtual bool CanAttack()
        {
            return IsWalking() && !IsCrouched();
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
        #region Character Overrides
        public override float GetMaxSpeed()
        {
            return _isSprinting ? maxSprintSpeed : base.GetMaxSpeed();
        }

        #endregion
    }
}