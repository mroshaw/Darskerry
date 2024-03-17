#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Actions
{
    public enum RotateType {WorkIdle, WorkBusy, HomeIdle, HomeBusy}
    
#if UNITY_EDITOR
    [vFSMHelpbox("This action causes the NPC to rotate to face the rotation the given transform", UnityEditor.MessageType.Info)]
#endif
    
    public class RotateToPositionAction : vStateAction
    {
        /// <summary>
        /// Return the Action Category
        /// </summary>
        public override string categoryName => "NPC/";

        /// <summary>
        /// Return the Action name
        /// </summary>
        public override string defaultName => "Rotate to position";

        /// <summary>
        /// Target rotation type
        /// </summary>
        public RotateType rotateType;
        
        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <param name="fsmExecutionType"></param>
        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType fsmExecutionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            fsmBehaviour.aiController.RotateToPosition(rotateType);
        }
    }
}
#endif