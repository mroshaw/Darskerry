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
    [vFSMHelpbox("Determines whether it is night time", UnityEditor.MessageType.Info)]
#endif
    public class IsBetweenHoursDecision : vStateDecision
    {
        /// <summary>
        /// Return the Decision Category
        /// </summary>
        public override string categoryName => "Time and Weather/";

        /// <summary>
        /// Return the Decision name
        /// </summary>
        public override string defaultName => "Is Night?";

        /// <summary>
        /// Configurable temperament to compare
        /// </summary>
        public float startTime;
        public float endTime;

        /// <summary>
        /// Check to see if the AI is of the given temperament
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>
        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
            if (fsmBehaviour.aiController == null)
            {
                return false;
            }
            
            // HDRP Time of Day
#if HDRPTIMEOFDAY
            float time = HDRPTimeOfDay.Instance.TimeOfDay;
            if (endTime < startTime)
            {
                return (time > startTime && time <= 23.99) || (time > 0 && time < endTime);
            }
            else
            {
                return (time > startTime && time <= endTime) || (time > 0 && time < 5.99);
            }
#endif
            
            // Enviro
#if ENVIRO_3
            
#endif

            return false;

        }
    }
}
#endif