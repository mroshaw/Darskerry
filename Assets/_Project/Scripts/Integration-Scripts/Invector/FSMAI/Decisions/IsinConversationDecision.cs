#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Decisions
{
#if UNITY_EDITOR
    [vFSMHelpbox("This is an IsinConversation decision", UnityEditor.MessageType.Info)]
#endif
    public class IsinConversationDecision : vStateDecision
    {
        /// <summary>
        /// Return the Decision Category
        /// </summary>
		public override string categoryName => "Dialog and Quests/";

        /// <summary>
        /// Return the Decision name
        /// </summary>
        public override string defaultName => "IsinConversation";

        /// <summary>
        /// Determine whether NPC is having a conversation
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>
        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
            if (fsmBehaviour.aiController != null)
            {
                return fsmBehaviour.aiController.IsInConversation;
            }
            return false;
        }
    }
}
#endif