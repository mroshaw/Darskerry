#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Decisions
{
#if UNITY_EDITOR
    [vFSMHelpbox("Decide whether it's time for the NPC to go to work", UnityEditor.MessageType.Info)]
#endif
    public class GoToWorkDecision : vStateDecision
    {
        /// <summary>
        /// Return the Decision Category
        /// </summary>
		public override string categoryName => "NPC/";

        /// <summary>
        /// Return the Decision name
        /// </summary>
        public override string defaultName => "Time to go to work";

        /// <summary>
        /// Decide if it's time for the NPC to go to work
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>
        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
#if ENVIRO_3
            if (!fsmBehaviour.aiController.workplace || !Enviro.EnviroManager.instance)
            {
                return false;
            }

            int startHour = fsmBehaviour.aiController.workStartHour;
            int endHour = fsmBehaviour.aiController.workEndHour;
            int enviroHour = Enviro.EnviroManager.instance.Time.hours;
            return (enviroHour >= startHour && enviroHour <= endHour);
#else
            return false;
#endif
        }
    }
}
#endif