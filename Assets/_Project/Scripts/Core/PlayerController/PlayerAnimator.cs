using UnityEngine;
namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class PlayerAnimator : MonoBehaviour
    {
        // Cache Animator parameters
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int Turn = Animator.StringToHash("Turn");
        private static readonly int Ground = Animator.StringToHash("OnGround");
        private static readonly int Crouch = Animator.StringToHash("Crouch");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int JumpLeg = Animator.StringToHash("JumpLeg");
        private static readonly int Roll = Animator.StringToHash("Roll");
        private static readonly int Attack = Animator.StringToHash("Attack");

        // Cached Character
        private PlayerCharacter _playerCharacter;

        [SerializeField] private float forwardAmountDebug;
        [SerializeField] private float speedDebug;
        [SerializeField] private float maxSpeedDebug;

        protected virtual void Awake()
        {
            // Cache our Character
            _playerCharacter = GetComponentInParent<PlayerCharacter>();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            // Get Character animator
            Animator animator = _playerCharacter.GetAnimator();

            // Compute input move vector in local space
            Vector3 move = transform.InverseTransformDirection(_playerCharacter.GetMovementDirection());

            // Update the animator parameters
            float forwardAmount = _playerCharacter.useRootMotion && _playerCharacter.GetRootMotionController()
                ? move.z
                : Mathf.InverseLerp(0.0f, _playerCharacter.GetMaxAttainableSpeed(), _playerCharacter.GetSpeed());

            forwardAmountDebug = forwardAmount;
            speedDebug = _playerCharacter.GetSpeed();
            maxSpeedDebug = _playerCharacter.GetMaxSpeed();

            animator.SetFloat(Forward, forwardAmount, 0.1f, deltaTime);
            animator.SetFloat(Turn, Mathf.Atan2(move.x, move.z), 0.1f, deltaTime);

            animator.SetBool(Ground, _playerCharacter.IsGrounded());
            animator.SetBool(Crouch, _playerCharacter.IsCrouched());
            animator.SetBool(Roll, _playerCharacter.IsRolling());
            animator.SetBool(Attack, _playerCharacter.IsAttacking());

            if (_playerCharacter.IsFalling())
            {
                animator.SetFloat(Jump, _playerCharacter.GetVelocity().y, 0.1f, deltaTime);
            }

            // Calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1.0f);
            float jumpLeg = (runCycle < 0.5f ? 1.0f : -1.0f) * forwardAmount;

            if (_playerCharacter.IsGrounded())
            {
                animator.SetFloat(JumpLeg, jumpLeg);
            }
        }

        /// <summary>
        /// Called by an Animation Event on the roll animation clip
        /// </summary>
        public void StopRollingAnim()
        {
            _playerCharacter.StopRolling();
        }
    }
}
