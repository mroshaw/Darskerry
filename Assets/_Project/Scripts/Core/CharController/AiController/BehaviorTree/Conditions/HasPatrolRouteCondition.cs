using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "Has Patrol Route", story: "[AiBrain] has a Patrol Route", category: "Conditions",
        id: "7d590140f7684dd8c4607439cb520623")]
    public partial class HasPatrolRouteCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;

        public override bool IsTrue()
        {
            if (AiBrain.Value == null)
            {
                return false;
            }

            return AiBrain.Value.HasPatrolRoute();
        }
    }
}