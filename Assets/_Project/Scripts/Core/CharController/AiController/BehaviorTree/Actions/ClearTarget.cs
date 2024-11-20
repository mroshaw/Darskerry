using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "AI Clear Target", story: "Clear [Target] on [Agent]", category: "Action/Detection", id: "253fdde0be26f2c53f8ab51ae4e1a63d")]
    public partial class ClearTargetAction : AiBrainAction
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        protected override Status OnStart()
        {
            Target.ObjectValue = null;
            return Status.Success;
        }
    }
}