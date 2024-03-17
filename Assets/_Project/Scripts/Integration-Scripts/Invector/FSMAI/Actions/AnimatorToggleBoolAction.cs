#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Actions
{
#if UNITY_EDITOR
    [vFSMHelpbox("This is a Animator Toggle Bool Action", UnityEditor.MessageType.Info)]
#endif
    
    // StateAction class to allow FSM AIs to toggle boolean animator parameters
    public class AnimatorToggleBoolAction : vStateAction
    {
        /// <summary>
        /// Return the Action Category
        /// </summary>
        public override string categoryName => "Animator/";

        /// <summary>
        /// Return the Action name
        /// </summary>
        public override string defaultName => "Toggle Bool";
        
        // State to set the animator parameter
        public string animBool;
        
        // Name of the animator parameter
        public bool toggleValue;

        /// <summary>
        /// Sets the value on the given animator parameter, through the fsmBehaviour and associated animator
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <param name="fsmExecutionType"></param>
        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType fsmExecutionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            fsmBehaviour.aiController.animator.SetBool(animBool, toggleValue);
        }
    }
}
#endif