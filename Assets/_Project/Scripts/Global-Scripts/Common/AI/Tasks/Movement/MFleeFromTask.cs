using MalbersAnimations;
using RenownedGames.AITree;
using UnityEngine;

namespace Malbers.Integration.AITree.Core.Tasks
{
    [NodeContent("Flee From", "Animal Controller/Flee From", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MFleeFromTask : MTaskNode
    {
        [RequiredField] public TransformKey fleeTargetKey;
        [Tooltip("Distance to flee from the target")]
        public float fleeDistance;
        private bool _taskDone;

        protected override void OnEntry()
        {
            base.OnEntry();

            // Get the direction
            Vector3 direction = (GetOwner().transform.position - fleeTargetKey.GetValue().position);
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

        protected override void OnExit()
        {
            base.OnExit();
            AIBrain.AIControl.ClearTarget();
        }

        public override string GetDescription()
        {
            string description = base.GetDescription();
            if (!string.IsNullOrEmpty(description))
            {
                description += "\n";
            }
            description += $"Flee distance: {fleeDistance}\n";
            return description;
        }
    }
}