using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Move To", story: "Move to [Transform] using [AiBrain]", category: "Action",
        id: "704c056a6c235a2d3dd8e4e731f7a37d")]
    public partial class MoveToAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Transform;
        [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;

        private bool _hasMoved;

        protected override Status OnStart()
        {
            if (AiBrain.Value == null)
            {
                LogFailure("No AIBrain assigned.");
                return Status.Failure;
            }

            AiBrain.Value.MoveTo(Transform.Value.position, DestinationReached);
            _hasMoved = false;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return _hasMoved ? Status.Success : Status.Running;
        }

        private void DestinationReached()
        {
            _hasMoved = true;
        }
    }

}