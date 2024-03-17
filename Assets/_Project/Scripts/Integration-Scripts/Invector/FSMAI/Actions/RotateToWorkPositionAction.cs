#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;
using UnityEngine;

namespace DaftAppleGames.Common.AI.Invector.Actions
{
#if UNITY_EDITOR
    [vFSMHelpbox("This action causes the NPC to rotate to face the rotation of the work busy transform", UnityEditor.MessageType.Info)]
#endif
    public class RotateToWorkPositionAction : vStateAction
    {
        /// <summary>
        /// Return the Action Category
        /// </summary>
        public override string categoryName => "NPC/";

        /// <summary>
        /// Return the Action name
        /// </summary>
        public override string defaultName => "Rotate to work position";

        /// <summary>
        /// Tell the FSM AI to rotate the NPC to the end home position
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <param name="executionType"></param>
        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType executionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            fsmBehaviour.aiController.RotateToWorkPosition();
        }
    }
}
#endif