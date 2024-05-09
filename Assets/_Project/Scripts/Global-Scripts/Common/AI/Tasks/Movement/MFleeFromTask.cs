using MalbersAnimations;
using RenownedGames.AITree;
using UnityEngine;

namespace Malbers.Integration.AITree.Core.Tasks
{
    [NodeContent("Flee From", "Animal Controller/Movement/Flee From", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MFleeFromTask : MTaskNode
    {
        [RequiredField] public TransformKey fleeTargetKey;
        [Tooltip("Distance to flee from the target")]
        public float fleeDistance;
        private bool _isMoving;

        protected override void OnEntry()
        {
            base.OnEntry();

            // Get the direction
            Vector3 direction = (GetOwner().transform.position - fleeTargetKey.GetValue().position);
            direction.Normalize();
            Vector3 fleePosition = GetOwner().transform.position + (direction * fleeDistance);

            Debug.Log($"Fleeing to: {fleePosition.x}, {fleePosition.y}, {fleePosition.z}");

            AIBrain.AIControl.SetDestination(fleePosition, true);
            _isMoving = true;
        }

        /// <summary>
        /// Wait for the Animal to start moving, then check each frame until the AI Control
        /// reports that the Animal has reached it's destination.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (_isMoving && AIBrain.AIControl.HasArrived)
            {
                return State.Success;
            }
            return State.Running;
        }

        /// <summary>
        /// Clear the AI Target and set IsMoving to false
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();
            AIBrain.AIControl.ClearTarget();
            _isMoving = false;
        }

        /// <summary>
        /// Return a description to display in the AI Tree UI when the task is run
        /// </summary>
        /// <returns></returns>
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