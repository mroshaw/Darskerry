using System;
using Unity.Behavior;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "Transform Is Null", story: "[Transform] is null", category: "Conditions",
        id: "038d1834187a5a0071a570738df76a93")]
    public partial class TransformIsNullCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Transform> Transform;

        public override bool IsTrue()
        {
            return Transform.Value == null;
        }

        public override void OnStart()
        {
        }

        public override void OnEnd()
        {
        }
    }
}