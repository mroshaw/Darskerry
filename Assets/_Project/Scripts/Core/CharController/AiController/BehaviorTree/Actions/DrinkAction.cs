using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Drink", story: "Agent drinks using [AiBrain]", category: "Action",
        id: "e0244217a628d45716b10b2f14860411")]
    public partial class DrinkAction : Action
    {
        [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;

        protected override Status OnStart()
        {
            if (AiBrain.Value == null)
            {
                LogFailure("No AIBrain assigned.");
                return Status.Failure;
            }

            AiBrain.Value.Drink(100.0f);
            return Status.Success;
        }
    }
}