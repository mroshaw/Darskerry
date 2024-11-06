using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController.Behaviours
{
    public class RollBehaviour : CharacterBehaviour
    {
        #region State events
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            Character.StopRolling();
        }
        #endregion
    }
}