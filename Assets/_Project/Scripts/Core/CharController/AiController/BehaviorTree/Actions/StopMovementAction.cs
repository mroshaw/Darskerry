using System;
using DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions;
using Unity.Behavior;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AI Stop Movement", story: "[Agent] stops moving", category: "Action/Navigation", id: "eaa0047d484bd43d5b0f1f88c3fcb4ba")]
public partial class StopMovementAction : AiBrainAction
{
    protected override Status OnStart()
    {
        if(!Init())
        {
            return Status.Failure;
        }
        AiBrain.NavMeshCharacter.StopMovement();
        return Status.Running;
    }
}