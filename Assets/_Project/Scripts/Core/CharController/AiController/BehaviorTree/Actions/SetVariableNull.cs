using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Set Variable Null", story: "Set [Variable] to null", category: "Action")]
    public partial class SetVariableNullAction : Action
    {
        [SerializeReference] public BlackboardVariable Variable;
        protected override Status OnStart()
        {
            Variable.ObjectValue = null;
            return Status.Success;
        }
    }
}