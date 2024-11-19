using ECM2;
using System;
using System.Collections;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Wander", story: "Wander using an [AiBrain]", category: "Action/Navigation", id: "79530ae655e4c01296bec8f3939d187e")]
    public partial class WanderAction : Action
    {
    [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;

        protected override Status OnStart()
        {
            base.OnStart();

            if (AiBrain.Value == null)
            {
                LogFailure("No Ai Brain assigned.");
                return Status.Failure;
            }

            AiBrain.Value.Wander();
            return Status.Success;
        }
    }
}