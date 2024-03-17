#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Actions
{
#if UNITY_EDITOR
    [vFSMHelpbox("Used to toggle an NPC working animation on and off", UnityEditor.MessageType.Info)]
#endif
    
    // StateAction class to toggle an NPC work status
    public class ToggleWorkAction : vStateAction
    {       
        /// <summary>
        /// Return the Action Category
        /// </summary>
        public override string categoryName => "NPC/";

        /// <summary>
        /// Return the Action name
        /// </summary>
        public override string defaultName => "Toggle Work";
        
        // Public property, value to set the toggle
        public bool toggleValue;
        
        /// <summary>
        /// Set the animator parameter toggle value, via the FSM AI animator
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <param name="fsmExecutionType"></param>
        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType fsmExecutionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            fsmBehaviour.aiController.animator.SetInteger("WorkingMoveSet_ID", (int)fsmBehaviour.aiController.workType);
            fsmBehaviour.aiController.animator.SetBool("IsWorking", toggleValue);
        }
    }
}
#endif