using MalbersAnimations;
using MalbersAnimations.Scriptables;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

namespace Malbers.Integration.AITree.Core.Tasks
{
    [NodeContent("Set Target", "Animal Controller/Set Target", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MSetTargetNode : MTaskNode
    {
        [Space]
        public TaskTargetType targetType = TaskTargetType.TransformKey;

        [ShowIf("IsTaskTargetType",TaskTargetType.TransformVar)][RequiredField] public TransformVar targetTransformVar;
        [ShowIf("IsTaskTargetType",TaskTargetType.TransformKey)][RequiredField] public TransformKey targetTransformKey;
        [ShowIf("IsTaskTargetType",TaskTargetType.GameObjectVar)][RequiredField] public GameObjectVar targetGameObjectVar;
        [ShowIf("IsTaskTargetType",TaskTargetType.GameObjectName)] public string targetGameObjectName;
        [ShowIf("IsTaskTargetType",TaskTargetType.RuntimeGameObjects)] [RequiredField] public RuntimeGameObjects targetRuntimeGameObject;
        [ShowIf("IsTaskTargetType",TaskTargetType.RuntimeGameObjects)] RuntimeSetTypeGameObject runTimeSetType = RuntimeSetTypeGameObject.Random;
        [ShowIf("IsTaskTargetType",TaskTargetType.RuntimeGameObjects)] public IntReference rtIntRef = new();
        [ShowIf("IsTaskTargetType",TaskTargetType.RuntimeGameObjects)] public StringReference rtStringRef = new();

        private string _description;

        [Tooltip("Animal should move to that target")]
        public bool moveToTarget = true;

        private bool _taskDone;

        /// <summary>
        /// Utility method to support showing properties based on the selected Target Type
        /// </summary>
        /// <param name="targetTypeToCompare"></param>
        /// <returns></returns>
        public bool IsTaskTargetType(TaskTargetType targetTypeToCompare)
        {
            return targetType == targetTypeToCompare;
        }

        protected override void OnEntry()
        {
            base.OnEntry();

            if (moveToTarget)
            {
                AIBrain.AIControl.UpdateDestinationPosition = true;          //Check if the target has moved
            }
            else
            {
                // Stop if the animal is already moving
                if (AIBrain.AIControl.IsMoving)
                {
                    AIBrain.AIControl.Stop();
                }
            }

            if (targetType == TaskTargetType.RuntimeGameObjects)
            {
                AIBrain.AIControl.SetTarget(
                    GetRuntimeGameObject(targetRuntimeGameObject, runTimeSetType, rtIntRef, rtStringRef, out _description).transform, moveToTarget);
            }
            else
            {
                AIBrain.AIControl.SetTarget(GetTargetTransform(targetType, targetTransformVar,
                    targetTransformKey, targetGameObjectVar, targetGameObjectName, out _description), moveToTarget);
            }
            _taskDone = true;
        }

        /// <summary>
        /// Check to see if the AI has arrived at the target
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (!_taskDone || !AIBrain.AIControl.HasArrived)
            {
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
            if (!moveToTarget)
            {
                AIBrain.AIControl.Stop();
            }
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