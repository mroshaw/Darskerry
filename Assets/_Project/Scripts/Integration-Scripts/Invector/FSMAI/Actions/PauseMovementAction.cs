#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Actions
{
#if UNITY_EDITOR
    [vFSMHelpbox("Pause or unpause a NavMeshAgent FSM AI movement", UnityEditor.MessageType.Info)]
#endif
    public class PauseMovementAction : vStateAction
    {
        /// <summary>
        /// Return the Action Category
        /// </summary>
        public override string categoryName => "Movement/";

        /// <summary>
        /// Return the Action name
        /// </summary>
        public override string defaultName => "Pause Movement";

        /// <summary>
        /// Movement state toggle
        /// </summary>
        public bool pausedState;
        
        /// <summary>
        /// Tells the FSM AI to stop movement
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <param name="executionType"></param>
        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType executionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            if (pausedState)
            {
                fsmBehaviour.aiController.StopMovement();
            }
        }
    }
}
#endif