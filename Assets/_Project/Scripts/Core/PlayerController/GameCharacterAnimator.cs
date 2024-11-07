using UnityEngine;
namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public abstract class GameCharacterAnimator : MonoBehaviour
    {
        // Cache Animator parameters
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int Turn = Animator.StringToHash("Turn");
        private static readonly int Ground = Animator.StringToHash("OnGround");
        private static readonly int Crouch = Animator.StringToHash("Crouch");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Attack = Animator.StringToHash("Attack");

        [SerializeField] private float maxSpeedDebug;
        [SerializeField] private float currentSpeedDebug;
        [SerializeField] private float forwardAmountDebug;

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
                : Mathf.InverseLerp(0.0f, GetMaxSpeed(), GameCharacter.GetSpeed());

            maxSpeedDebug = GetMaxSpeed();
            currentSpeedDebug = GameCharacter.GetSpeed();
            forwardAmountDebug = ForwardAmount;

            Animator.SetFloat(Forward, ForwardAmount, 0.1f, deltaTime);
            Animator.SetFloat(Turn, Mathf.Atan2(MoveDirection.x, MoveDirection.z), 0.1f, deltaTime);

            Animator.SetBool(Ground, GameCharacter.IsGrounded());
            Animator.SetBool(Attack, GameCharacter.IsAttacking());


            if (GameCharacter.IsFalling())
            {
                Animator.SetFloat(Jump, GameCharacter.GetVelocity().y, 0.1f, deltaTime);
            }
        }

        protected virtual float GetMaxSpeed()
        {
            return GameCharacter.GetMaxAttainableSpeed();
        }
    }
}
