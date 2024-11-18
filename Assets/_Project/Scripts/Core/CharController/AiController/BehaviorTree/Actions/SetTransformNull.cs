using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Set Transform Null", story: "Set [Transform] to null", category: "Action", id: "70a5b810f073de3d71270e9bd8f21278")]
    public partial class SetTransformNullAction : Action
    {
    [SerializeReference] public BlackboardVariable<Transform> Transform;
        protected override Status OnStart()
        {
            Transform.Value = null;
            return Status.Success;
        }

    }
}