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

        // Cached Character

        private PlayerCharacter _playerCharacter;

        private void Awake()
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
                : Mathf.InverseLerp(0.0f, _playerCharacter.GetMaxSpeed(), _playerCharacter.GetSpeed());

            animator.SetFloat(Forward, forwardAmount, 0.1f, deltaTime);
            animator.SetFloat(Turn, Mathf.Atan2(move.x, move.z), 0.1f, deltaTime);

            animator.SetBool(Ground, _playerCharacter.IsGrounded());
            animator.SetBool(Crouch, _playerCharacter.IsCrouched());

            if (_playerCharacter.IsFalling())
                animator.SetFloat(Jump, _playerCharacter.GetVelocity().y, 0.1f, deltaTime);

            // Calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)

            float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1.0f);
            float jumpLeg = (runCycle < 0.5f ? 1.0f : -1.0f) * forwardAmount;

            if (_playerCharacter.IsGrounded())
                animator.SetFloat(JumpLeg, jumpLeg);
        }
    }
}

    /*
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float locomotionBlendSpeed = 4f;

    public bool RollingAnimIsPlaying { get; set; }

    private PlayerCharacter _playerCharacter;

    // Locomotion
    private static readonly int LateralSpeedHash = Animator.StringToHash("LateralSpeed");
    private static readonly int ForwardSpeedHash = Animator.StringToHash("ForwardSpeed");
    private static readonly int VerticalSpeedHash = Animator.StringToHash("VerticalSpeed");
    private static readonly int IsIdlingHash = Animator.StringToHash("IsIdling");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int IsFallingHash = Animator.StringToHash("IsFalling");
    private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    private static readonly int IsRollingHash = Animator.StringToHash("IsRolling");
    private static readonly int IsCrouchedHash = Animator.StringToHash("IsCrouched");

    // Actions
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    private static readonly int IsGatheringHash = Animator.StringToHash("IsGathering");
    private static readonly int IsPlayingActionHash = Animator.StringToHash("IsPlayingAction");
    private int[] _actionHashes;

    // Camera/Rotation
    private static readonly int IsRotatingToTargetHash = Animator.StringToHash("IsRotatingToTarget");
    private static readonly int RotationMismatchHash = Animator.StringToHash("RotationMismatch");

    private Vector3 _currentBlendInput = Vector3.zero;

    private void Awake()
    {

        _playerCharacter = GetComponent<PlayerCharacter>();
        _actionHashes = new int[] { IsGatheringHash };
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
        bool isJumping = _playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping;
        bool isFalling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Falling;
        bool isCrouched = _playerState.CurrentPlayerMovementState == PlayerMovementState.Crouching;
        bool isRolling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Rolling;

        bool isGrounded = _playerState.InGroundedState();
        bool isPlayingAction = _actionHashes.Any(hash => animator.GetBool(hash));

        Vector3 inputTarget = new Vector3(_playerController.LateralSpeed, _playerController.VerticalSpeed, _playerController.ForwardSpeed);
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);

        animator.SetBool(IsGroundedHash, isGrounded);
        animator.SetBool(IsIdlingHash, isIdling);
        animator.SetBool(IsFallingHash, isFalling);
        animator.SetBool(IsJumpingHash, isJumping);
        animator.SetBool(IsRotatingToTargetHash, _playerController.IsRotatingToTarget);
        animator.SetBool(IsAttackingHash, _playerActionsInput.AttackPressed);
        animator.SetBool(IsGatheringHash, _playerActionsInput.GatherPressed);
        animator.SetBool(IsRollingHash, isRolling);
        animator.SetBool(IsCrouchedHash, isCrouched);
        animator.SetBool(IsPlayingActionHash, isPlayingAction);

        animator.SetFloat(LateralSpeedHash, _currentBlendInput.x);
        animator.SetFloat(ForwardSpeedHash, _currentBlendInput.z);
        animator.SetFloat(VerticalSpeedHash, _currentBlendInput.y);
        animator.SetFloat(RotationMismatchHash, _playerController.RotationMismatch);
        */
