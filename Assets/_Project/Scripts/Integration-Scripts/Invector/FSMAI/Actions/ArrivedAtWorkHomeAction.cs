#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Actions
{
#if UNITY_EDITOR
    [vFSMHelpbox("This action tells the NPC that they have reached their home or workplace", UnityEditor.MessageType.Info)]
#endif
    
    // StateAction class to allow FSM AIs to toggle boolean animator parameters
    public class ArrivedAtWorkHomeAction : vStateAction
    { 
        /// <summary>
        /// Return the Action Category
        /// </summary>
       public override string categoryName => "NPC/";

        /// <summary>
        /// Return the Action name
        /// </summary>
       public override string defaultName => "Arrived at Work or Home";

       /// <summary>
        /// Updates the FSM AI to indicate that the NPC has arrived at home or work
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <param name="fsmExecutionType"></param>
        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType fsmExecutionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            fsmBehaviour.aiController.goHome = false;
            fsmBehaviour.aiController.goToWork = false;
        }
    }
}
#endif