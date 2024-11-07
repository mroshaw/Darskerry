using UnityEngine;


namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class HumanCharacter : GameCharacter
    {
        [SerializeField] private float rollSpeed = 1.0f;

        private Vector3 _rollMovementDirection = Vector3.zero;

        private bool _isRolling;
        private bool _rollInputPressed;
        #region Startup
        #endregion
        #region Update
        protected override void OnBeforeSimulationUpdate(float deltaTime)
        {
            // Call base method implementation
            base.OnBeforeSimulationUpdate(deltaTime);

            // Handle rolling
            CheckRollInput();
            HandlingRolling();
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
        protected override bool CanAttack()
        {
            return base.CanAttack() && !IsRolling();
        }

        protected override bool CanSprint()
        {
            return base.CanSprint() && !IsRolling();
        }
        #endregion
    }
}