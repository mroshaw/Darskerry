using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "AI Rotate To Target", story: "[Agent] rotates to [Target]", category: "Action/Navigation", id: "0a25791c3fc13c4d19bf2c45479771e8")]
    public partial class RotateToTargetAction : AiBrainAction
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        protected override Status OnStart()
        {
            if(!Init())
            {
                return Status.Failure;
            }
            AiBrain.GameCharacter.RotateTowards(Target.Value.position, Time.deltaTime);
            return Status.Running;
        }
    }
}