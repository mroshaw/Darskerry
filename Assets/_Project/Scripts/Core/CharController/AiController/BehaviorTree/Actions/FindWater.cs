using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Object = UnityEngine.Object;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Find Water", story: "Find nearest [WaterSource]", category: "Action", id: "cfbeaaac6039bc4e972763f2f0fd128e")]
    public partial class FindWaterAction : Action
    {
    [SerializeReference] public BlackboardVariable<Transform> WaterSource;
        protected override Status OnStart()
        {
            WaterSource[] water = Object.FindObjectsByType<WaterSource>(FindObjectsSortMode.None);
            WaterSource.Value = water[0].gameObject.transform;
            return Status.Success;
        }

    }
}