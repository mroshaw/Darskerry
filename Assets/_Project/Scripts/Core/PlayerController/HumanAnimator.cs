using UnityEngine;
namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class HumanAnimator : MonoBehaviour
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
        private HumanCharacter _character;
        private Animator _animator;

        [SerializeField] private float forwardAmountDebug;
        [SerializeField] private float speedDebug;
        [SerializeField] private float maxSpeedDebug;

        protected virtual void Awake()
        {
            // Cache our Character
            _character = GetComponent<HumanCharacter>();


        }

        private void Start()
        {
            // Get Character animator
            _animator = _character.GetAnimator();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            // Compute input move vector in local space
            Vector3 move = transform.InverseTransformDirection(_character.GetMovementDirection());

            // Update the animator parameters
            float forwardAmount = _character.useRootMotion && _character.GetRootMotionController()
                ? move.z
                : Mathf.InverseLerp(0.0f, _character.GetMaxAttainableSpeed(), _character.GetSpeed());

            forwardAmountDebug = forwardAmount;
            speedDebug = _character.GetSpeed();
            maxSpeedDebug = _character.GetMaxSpeed();

            _animator.SetFloat(Forward, forwardAmount, 0.1f, deltaTime);
            _animator.SetFloat(Turn, Mathf.Atan2(move.x, move.z), 0.1f, deltaTime);

            _animator.SetBool(Ground, _character.IsGrounded());
            _animator.SetBool(Crouch, _character.IsCrouched());
            _animator.SetBool(Roll, _character.IsRolling());
            _animator.SetBool(Attack, _character.IsAttacking());

            if (_character.IsFalling())
            {
                _animator.SetFloat(Jump, _character.GetVelocity().y, 0.1f, deltaTime);
            }

            // Calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1.0f);
            float jumpLeg = (runCycle < 0.5f ? 1.0f : -1.0f) * forwardAmount;

            if (_character.IsGrounded())
            {
                _animator.SetFloat(JumpLeg, jumpLeg);
            }
        }
    }
}
