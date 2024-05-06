using MalbersAnimations;
using RenownedGames.AITree;
using UnityEngine;

namespace Malbers.Integration.AITree.Core.Tasks
{
    [NodeContent("Flee From Target", "Animal Controller/Flee From Target", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MFleeFromTargetTask : MTaskNode
    {
        [Tooltip("Distance to flee from the target")]
        public float fleeDistance;
        private bool _taskDone;

        protected override void OnEntry()
        {
            base.OnEntry();

            // Get the direction
            Vector3 direction = (GetOwner().transform.position - AIBrain.Target.position);
            direction.Normalize();
            Vector3 fleePosition = GetOwner().transform.position + (direction * fleeDistance);

            Debug.Log($"Fleeing to: {fleePosition.x}, {fleePosition.y}, {fleePosition.z}");

            AIBrain.AIControl.SetDestination(fleePosition, true);
            _taskDone = true;
        }

        protected override State OnUpdate()
        {
            if (!_taskDone || !AIBrain.AIControl.HasArrived)
            {
                return State.Running;
            }
            return State.Success;
        }

        /// <summary>
        /// Clear the Target, once we're done fleeing
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();
            AIBrain.AIControl.ClearTarget();
        }

        /// <summary>
        /// Return task description
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return $"{base.GetDescription()}\nFlee distance: {fleeDistance}";
        }
    }
}