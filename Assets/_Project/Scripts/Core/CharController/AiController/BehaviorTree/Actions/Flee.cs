using System;
using ECM2;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Flee", story: "Flee from [Target] using [AiBrain]", category: "Action", id: "d9102898acb44b193285ab3326751e54")]
    public partial class FleeAction : Action
    {
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;

        private NavMeshCharacter _navMeshCharacter;

        private bool _hasArrived;

        protected override Status OnStart()
        {
            if (AiBrain.Value == null)
            {
                LogFailure("No Ai Brain assigned.");
                return Status.Failure;
            }

            AiBrain.Value.Flee(Target.Value);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (AiBrain.Value.State == AiState.Fleeing)
            {
                return Status.Running;
            }
            return Status.Success;
        }

        protected override void OnEnd()
        {
            Target.Value = null;
        }
    }
}