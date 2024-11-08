using UnityEngine;
namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class GameCharacterAnimator : MonoBehaviour
    {
        // Cache Animator parameters
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int Turn = Animator.StringToHash("Turn");
        private static readonly int Ground = Animator.StringToHash("OnGround");
        private static readonly int JumpLeg = Animator.StringToHash("JumpLeg");
        private static readonly int Roll = Animator.StringToHash("Roll");
        private static readonly int Crouch = Animator.StringToHash("Crouch");

        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Attack = Animator.StringToHash("Attack");

        // Cached Character
        protected GameCharacter GameCharacter { get; private set; }
        protected Animator Animator { get; private set; }

        protected float ForwardAmount { get; private set; }
        protected Vector3 MoveDirection { get; private set; }

        protected virtual void Awake()
        {
            // Cache our Character
            GameCharacter = GetComponent<GameCharacter>();
        }

        protected virtual void Start()
        {
            // Get Character animator
            Animator = GameCharacter.GetAnimator();
        }

        protected virtual void Update()
        {
            float deltaTime = Time.deltaTime;
            // Compute input move vector in local space
            MoveDirection = transform.InverseTransformDirection(GameCharacter.GetMovementDirection());

            // Update the animator parameters
            ForwardAmount = GameCharacter.useRootMotion && GameCharacter.GetRootMotionController()
                ? MoveDirection.z
                : Mathf.InverseLerp(0.0f, GameCharacter.GetMaxSpeed(), GameCharacter.GetSpeed());

            Animator.SetFloat(Forward, ForwardAmount, 0.1f, deltaTime);
            Animator.SetFloat(Turn, Mathf.Atan2(MoveDirection.x, MoveDirection.z), 0.1f, deltaTime);
            Animator.SetBool(Ground, GameCharacter.IsGrounded());
            Animator.SetBool(Attack, GameCharacter.IsAttacking());
            Animator.SetBool(Roll, GameCharacter.IsRolling());
            Animator.SetBool(Crouch, GameCharacter.IsCrouched());

            // Calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1.0f);
            float jumpLeg = (runCycle < 0.5f ? 1.0f : -1.0f) * ForwardAmount;

            if (GameCharacter.IsGrounded())
            {
                Animator.SetFloat(JumpLeg, jumpLeg);
            }
            if (GameCharacter.IsFalling())
            {
                Animator.SetFloat(Jump, GameCharacter.GetVelocity().y, 0.1f, deltaTime);
            }
        }
    }
}
