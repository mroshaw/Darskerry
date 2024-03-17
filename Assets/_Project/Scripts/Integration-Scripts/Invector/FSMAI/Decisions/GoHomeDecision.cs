#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Decisions
{
#if UNITY_EDITOR
    [vFSMHelpbox("Decide whether it's time for the NPC to go home", UnityEditor.MessageType.Info)]
#endif
    public class GoHomeDecision : vStateDecision
    {
        /// <summary>
        /// Return the Decision Category
        /// </summary>
		public override string categoryName => "NPC/";

        /// <summary>
        /// Return the Decision name
        /// </summary>
        public override string defaultName { get; } = "Time to go home";

        /// <summary>
        /// Decide if it's time for the NPC to go home
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>
        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
#if ENVIRO_3
            if (!fsmBehaviour.aiController.home || !Enviro.EnviroManager.instance)
            {
                return false;
            }

            int startHour = fsmBehaviour.aiController.workStartHour;
            int endHour = fsmBehaviour.aiController.workEndHour;
            int enviroHour = Enviro.EnviroManager.instance.Time.hours;
            return (enviroHour < startHour || enviroHour >= endHour);
#else
            return false;
#endif
        }
    }
}
#endif