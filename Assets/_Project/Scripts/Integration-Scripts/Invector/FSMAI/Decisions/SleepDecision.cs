#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;
#if HDRPTIMEOFDAY
using ProceduralWorlds.HDRPTOD;
#endif
#if ENVIRO_3

#endif

namespace DaftAppleGames.Common.AI.Invector.Decisions
{
#if UNITY_EDITOR
    [vFSMHelpbox("Decide whether it's time for the NPC to go to sleep or wake up", UnityEditor.MessageType.Info)]
#endif
    public class SleepDecision : vStateDecision
    {
        /// <summary>
        /// Return the Decision Category
        /// </summary>
		public override string categoryName => "NPC/";

        /// <summary>
        /// Return the Decision name
        /// </summary>
        public override string defaultName => "Time to sleep?";

        /// <summary>
        /// Determine if it's time to sleep
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>
        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
            int sleepHour = fsmBehaviour.aiController.sleepHour;
            int wakeHour = fsmBehaviour.aiController.wakeHour;

            // Enviro
#if ENVIRO_3
            int enviroHour = Enviro.EnviroManager.instance.Time.hours;
            return (enviroHour >= sleepHour || enviroHour <= wakeHour);
#endif

            // HDRP Time of Day
#if HDRPTIMEOFDAY
            return true;
#endif
        }
    }
}
#endif