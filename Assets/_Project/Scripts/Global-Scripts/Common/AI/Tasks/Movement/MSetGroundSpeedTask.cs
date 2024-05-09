using RenownedGames.AITree;
using Sirenix.OdinInspector;
using State = RenownedGames.AITree.State;

namespace Malbers.Integration.AITree.Core.Tasks
{
    public enum GroundSpeed { Walk=1, Trot=2, Run=3 }

    [NodeContent("Set Ground Speed", "Animal Controller/Movement/Set Ground Speed", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MSetGroundSpeedTask : MTaskNode
    {
        private const string GroundSpeedSetName = "Ground";

        [BoxGroup("Settings")] public GroundSpeed groundSpeed = GroundSpeed.Walk;

        /// <summary>
        /// Set the ground speed in the first tick and return success.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            AIBrain.Animal.SpeedSet_Set_Active(GroundSpeedSetName, (int)groundSpeed);
            return State.Success;
        }

        /// <summary>
        /// Return the task description
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return $"{base.GetDescription()}\nGround speed: {groundSpeed.ToString()}\n";
        }
    }
}