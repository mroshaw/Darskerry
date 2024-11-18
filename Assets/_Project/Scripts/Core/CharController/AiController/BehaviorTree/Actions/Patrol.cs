using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{

    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Patrol", story: "Patrol [PatrolRoute] waypoints using [AiBrain]", category: "Action", id: "8016e055e781d0a76aedfc22d3146316")]
    public partial class PatrolAction : Action
    {
    [SerializeReference] public BlackboardVariable<PatrolRoute> PatrolRoute;
    [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;
        protected override Status OnStart()
        {
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}