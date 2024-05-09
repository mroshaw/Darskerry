using MalbersAnimations;
using MalbersAnimations.Controller.AI;
using MalbersAnimations.Scriptables;
using RenownedGames.AITree;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Malbers.Integration.AITree.Core.Tasks
{

    [NodeContent("Set Destination", "Animal Controller/Movement/Set Destination", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MSetDestinationNode : MTaskNode
    {

        [Tooltip("Slow multiplier to set on the Destination")]
        public float SlowMultiplier = 0;
        [Space]
        public TaskPositionType positionType = TaskPositionType.Vector3Key;
        [ShowIf("positionType",TaskPositionType.TransformVar)] [RequiredField] public TransformVar positionTransformVar;
        [ShowIf("positionType",TaskPositionType.Vector3Var)] [RequiredField] public Vector3Var positionVector3Var;
        [ShowIf("positionType",TaskPositionType.TransformKey)] [RequiredField] public TransformKey positionTransformKey;
        [ShowIf("positionType",TaskPositionType.Vector3Key)] [RequiredField] public Vector3Key positionVector3Key;
        [ShowIf("positionType",TaskPositionType.GameObjectVar)] [RequiredField] public GameObjectVar positionGameObjectVar;
        [ShowIf("positionType",TaskPositionType.GameObjectName)] public string positionGameObjectName;

        [ShowIf("positionType",TaskPositionType.GameObjectVar)] [RequiredField] public RuntimeGameObjects positionRuntimeGameObject;
        [ShowIf("positionType",TaskPositionType.GameObjectVar)] public RuntimeSetTypeGameObject runTimeSetType = RuntimeSetTypeGameObject.Random;
        [ShowIf("positionType",TaskPositionType.GameObjectVar)] public IntReference rtIntRef = new();
        [ShowIf("positionType",TaskPositionType.GameObjectVar)] public StringReference rtStringRef = new();

        [Tooltip("When a new target is assigned it also sets that the Animal should move to that target")]
        public bool moveToTarget = true;

        private string _description = "";

        /// <summary>
        /// Utility method to support showing properties based on the selected Position Type
        /// </summary>
        /// <param name="positionTypeToCompare"></param>
        /// <returns></returns>
        public bool IsTaskPositionType(TaskPositionType positionTypeToCompare)
        {
            return positionType == positionTypeToCompare;
        }

        protected override void OnEntry()
        {
            base.OnEntry();
            AIBrain.AIControl.ClearTarget();
            AIBrain.AIControl.CurrentSlowingDistance = AIBrain.AIControl.StoppingDistance * SlowMultiplier;
            if (positionType == TaskPositionType.RuntimeGameObjects)
            {
                AIBrain.AIControl.SetDestination(GetRuntimeGameObject(positionRuntimeGameObject, runTimeSetType, rtIntRef, rtStringRef, out _description).transform.position, moveToTarget);
            }
            else
            {
                AIBrain.AIControl.SetDestination(GetTargetPosition(positionType, positionTransformVar, positionVector3Var, positionTransformKey, positionVector3Key, positionGameObjectVar, positionGameObjectName, out _description), moveToTarget);
            }
            AIBrain.TasksDone = true;
        }

        protected override State OnUpdate()
        {
            if ((AIBrain.TasksDone && AIBrain.AIControl.HasArrived && moveToTarget) || AIBrain.TasksDone && !moveToTarget)
            {
                return State.Success;
            }
            return State.Running;
        }

        public override string GetDescription()
        {
            return $"{base.GetDescription()} {_description}";
        }
    }
}