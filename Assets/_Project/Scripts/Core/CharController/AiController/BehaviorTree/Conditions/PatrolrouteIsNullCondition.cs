using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "Patrolroute Is Null", story: "[Patrolroute] is null", category: "Conditions",
        id: "7b53f0646f69b0a9572af9e3902dc3f3")]
    public partial class PatrolrouteIsNullCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<PatrolRoute> Patrolroute;

        public override bool IsTrue()
        {
            return Patrolroute.Value == null;
        }

        public override void OnStart()
        {
        }

        public override void OnEnd()
        {
        }
    }
}