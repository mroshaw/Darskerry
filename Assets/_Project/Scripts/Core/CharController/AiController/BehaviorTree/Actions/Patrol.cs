using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{

    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Patrol", story: "Patrol waypoints using [AiBrain]", category: "Action", id: "8016e055e781d0a76aedfc22d3146316")]
    public partial class PatrolAction : Action
    {
    [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;
    protected override Status OnStart()
        {
            if (AiBrain.Value == null)
            {
                LogFailure("No AIBrain assigned.");
                return Status.Failure;
            }
            AiBrain.Value.Patrol();
            return Status.Success;
        }
    }
}