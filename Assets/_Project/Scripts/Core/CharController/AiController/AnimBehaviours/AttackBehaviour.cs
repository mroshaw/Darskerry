using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.AnimBehaviours
{
    public class AttackBehaviour : CharacterBehaviour
    {
        #region State events
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            Character.StopAttacking();
        }
        #endregion
    }
}