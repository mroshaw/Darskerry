using UnityEngine;
namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class HumanAnimator : GameCharacterAnimator
    {
        // Cache Animator parameters
        private static readonly int JumpLeg = Animator.StringToHash("JumpLeg");
        private static readonly int Roll = Animator.StringToHash("Roll");

        private HumanCharacter _humanCharacter;

        protected override void Awake()
        {
            base.Awake();
            // Cache our Character
            _humanCharacter = GameCharacter as HumanCharacter;
        }

        protected override void Update()
        {
            base.Update();

            Animator.SetBool(Roll, _humanCharacter.IsRolling());

            // Calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1.0f);
            float jumpLeg = (runCycle < 0.5f ? 1.0f : -1.0f) * ForwardAmount;

            if (GameCharacter.IsGrounded())
            {
                Animator.SetFloat(JumpLeg, jumpLeg);
            }
        }
    }
}
