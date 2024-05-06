using MalbersAnimations.Scriptables;
using RenownedGames.AITree;
using UnityEngine;

namespace Malbers.Integration.AITree.Core
{
    public enum TaskTargetType { TransformKey, TransformVar, GameObjectVar, GameObjectName, RuntimeGameObjects }
    public enum TaskPositionType { Vector3Key, TransformKey, TransformVar, Vector3Var, GameObjectVar, GameObjectName, RuntimeGameObjects }

    /// <summary>
    /// Base class for Malbers Animal AI Task nodes
    /// Provides shared and abstract definitions for all Malbers Animal AI nodes designed for us
    /// with the AI Tree asset and Malbers Animal Controller.
    /// </summary>
    public abstract class MTaskNode : TaskNode
    {
        /// <summary>
        /// Exposes the AIBrain to all inheriting classes. This is a wrapper for Malbers components
        /// including AIControl and any other components we may need in the future.
        /// </summary>
        protected AIBrain AIBrain;

        /// <summary>
        /// Find the AIBrain component. This must be on the same GameObject as the BehaviourRunner component
        /// </summary>
        protected override void OnInitialize()
        {
            if (!AIBrain)
            {
                AIBrain = GetOwner().GetComponent<AIBrain>();

                if (!AIBrain)
                {
                    Debug.LogError("The AIBrain component must be present on the same GameObject as BehaviourRunner!");
                }
            }
        }

        /// <summary>
        /// Helper function to retrieve a GameObject pointer from a Malbers RunTimeGameObjects component
        /// </summary>
        /// <param name="runtimeGameObjects"></param>
        /// <param name="setType"></param>
        /// <param name="intRef"></param>
        /// <param name="strRef"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public GameObject GetRuntimeGameObject(RuntimeGameObjects runtimeGameObjects, RuntimeSetTypeGameObject setType,  IntReference intRef, StringReference strRef, out string description)
        {
            description = $"Target set to: RuntimeGameObjects {runtimeGameObjects.name}";
            return runtimeGameObjects.GetItem(setType, intRef, strRef, AIBrain.Animal.gameObject);
        }

        /// <summary>
        /// Helper function to retrieve a Transform from a number of possible types that can be captured in the
        /// AI Tree UI.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="targetTransformVar"></param>
        /// <param name="targetTransformKey"></param>
        /// <param name="targetGameObjectVar"></param>
        /// <param name="targetGameObjectName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        protected Transform GetTargetTransform(TaskTargetType targetType, TransformVar targetTransformVar,
            TransformKey targetTransformKey, GameObjectVar targetGameObjectVar, string targetGameObjectName, out string description)
        {
            description = "Target set to: ";
            Transform returnTransform = null;

            switch (targetType)
            {
                case TaskTargetType.TransformKey:
                    returnTransform =  targetTransformKey.GetValue();
                    break;
                case TaskTargetType.TransformVar:
                    returnTransform = targetTransformVar.Value;
                    break;
                case TaskTargetType.GameObjectVar:
                    returnTransform = targetGameObjectVar.Value.transform;
                    break;
                case TaskTargetType.GameObjectName:
                    GameObject findGameObject = GameObject.Find(targetGameObjectName);
                    if (findGameObject)
                    {
                        returnTransform = findGameObject.transform;
                    }
                    break;

                case TaskTargetType.RuntimeGameObjects:

                    break;
            }
             description += (returnTransform!=null? $"{targetType.ToString()} - {returnTransform.name}" : "null");
             return returnTransform;
        }

        /// <summary>
        /// Helper function to retrieve a Position from a number of possible types that can be captured in the
        /// AI Tree UI.
        /// </summary>
        /// <param name="positionType"></param>
        /// <param name="positionTransformVar"></param>
        /// <param name="positionVector3Var"></param>
        /// <param name="positionTransformKey"></param>
        /// <param name="positionVector3Key"></param>
        /// <param name="positionGameObjectVar"></param>
        /// <param name="positionGameObjectName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        protected Vector3 GetTargetPosition(TaskPositionType positionType, TransformVar positionTransformVar,
            Vector3Var positionVector3Var, TransformKey positionTransformKey, Vector3Key positionVector3Key,
            GameObjectVar positionGameObjectVar, string positionGameObjectName, out string description)
        {
            description = "Position set to: ";
            Vector3 returnPosition = Vector3.zero;

            switch (positionType)
            {
                case TaskPositionType.Vector3Key:
                    returnPosition = positionVector3Key.GetValue();
                    break;
                case TaskPositionType.Vector3Var:
                    returnPosition = positionVector3Var.Value;
                    break;
                case TaskPositionType.TransformKey:
                    returnPosition =  positionTransformKey.GetValue().position;
                    break;
                case TaskPositionType.TransformVar:
                    returnPosition =  positionTransformVar.Value.position;
                    break;
                case TaskPositionType.GameObjectVar:
                    returnPosition =  positionGameObjectVar.Value.transform.position;
                    break;
                case TaskPositionType.GameObjectName:
                    GameObject findGameObject = GameObject.Find(positionGameObjectName);
                    if (findGameObject)
                    {
                        returnPosition = findGameObject.transform.position;
                    }
                    break;
            }
            description += (returnPosition!=Vector3.zero? $"{positionType.ToString()} - ({returnPosition.x}, {returnPosition.y}, {returnPosition.z})" : "null");
            return returnPosition;
        }
    }
}