using DaftAppleGames.Darskerry.Core.PlayerController;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AiPatrolAction", story: "Moves a GameObject along way points (transform children of a GameObject) using an [AiBrain] component.", category: "Action", id: "ea0709ab0d42d49eda6bed6af42082f9")]
public partial class AiPatrolActionAction : Action
{
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

