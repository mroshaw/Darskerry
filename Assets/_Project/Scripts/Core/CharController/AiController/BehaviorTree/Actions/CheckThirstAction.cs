using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "CheckThirst", story: "Check if thirsty using [AiBrain]", category: "Action",
        id: "6ffd2f629ff808fe74370f92714345ed")]
    public partial class CheckThirstAction : Action
    {
        [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;

        protected override Status OnStart()
        {
            if (AiBrain.Value == null)
            {
                LogFailure("No AIBrain assigned.");
                return Status.Failure;
            }
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