using MalbersAnimations;
using MalbersAnimations.Scriptables;
using RenownedGames.AITree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Malbers.Integration.AITree.Core.Tasks
{
    [NodeContent("Patrol Waypoints", "Animal Controller/Movement/Patrol Waypoints", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MPatrolWaypointsTask : MTaskNode
    {
        [Space]
        public TaskTargetType targetType = TaskTargetType.TransformKey;

        [ShowIf("targetType",TaskTargetType.TransformVar)][RequiredField] public TransformVar targetTransformVar;
        [ShowIf("targetType",TaskTargetType.TransformKey)][RequiredField] public TransformKey targetTransformKey;
        [ShowIf("targetType",TaskTargetType.GameObjectVar)][RequiredField] public GameObjectVar targetGameObjectVar;
        [ShowIf("targetType",TaskTargetType.GameObjectName)] public string targetGameObjectName;
        [ShowIf("targetType",TaskTargetType.RuntimeGameObjects)] [RequiredField] public RuntimeGameObjects targetRuntimeGameObject;
        [ShowIf("targetType",TaskTargetType.RuntimeGameObjects)] RuntimeSetTypeGameObject runTimeSetType = RuntimeSetTypeGameObject.Random;
        [ShowIf("targetType",TaskTargetType.RuntimeGameObjects)] public IntReference rtIntRef = new();
        [ShowIf("targetType",TaskTargetType.RuntimeGameObjects)] public StringReference rtStringRef = new();

        private string _description;
        private bool _isMoveStarted;

        private Transform _firstWaypoint;

        protected override void OnEntry()
        {
            base.OnEntry();

            AIBrain.AIControl.UpdateDestinationPosition = true;

            // Stop if the animal is already moving
            if (AIBrain.AIControl.IsMoving)
            {
                AIBrain.AIControl.Stop();
            }

            Transform targetTransform;

            if (targetType == TaskTargetType.RuntimeGameObjects)
            {
                targetTransform = GetRuntimeGameObject(targetRuntimeGameObject, runTimeSetType, rtIntRef, rtStringRef, out _description).transform;
            }
            else
            {
                targetTransform = GetTargetTransform(targetType, targetTransformVar, targetTransformKey, targetGameObjectVar, targetGameObjectName, out _description);
            }
            AIBrain.AIControl.SetTarget(targetTransform, true);
            _firstWaypoint = targetTransform;
            _isMoveStarted = true;
        }

        /// <summary>
        /// Check to see if the AI has arrived at the target
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            // If we've not yet started moving, or we're moving and not arrived, or we still have waypoints to go,
            // we're still running the task
            if (!_isMoveStarted || !AIBrain.AIControl.HasArrived || AIBrain.AIControl.NextTarget)
            {
                return State.Running;
            }

            // If we're at the end of our patrol and bool is set, restart the patrol
            if (!AIBrain.AIControl.NextTarget && AIBrain.isLoopPatrol)
            {
                AIBrain.AIControl.SetTarget(_firstWaypoint, true);
                return State.Running;
            }

            return State.Success;
        }

        /// <summary>
        /// Stop the AI once the task is complete
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();
            AIBrain.AIControl.Stop();
        }

        /// <summary>
        /// Return the description of the task
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return $"{base.GetDescription()} {_description}";
        }
    }
}