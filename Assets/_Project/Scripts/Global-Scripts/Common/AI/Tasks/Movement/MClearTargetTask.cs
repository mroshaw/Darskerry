using RenownedGames.AITree;

namespace Malbers.Integration.AITree.Core.Tasks
{
    [NodeContent("Clear Target", "Animal Controller/Clear Target", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MClearTargetTask : MTaskNode
    {
        /// <summary>
        /// Clear the target on the AIControl component, via the AIBrain base property
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            AIBrain.AIControl.ClearTarget();
        }

        /// <summary>
        /// This task is a one off, so always return Success on update
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}