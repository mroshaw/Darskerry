using System;
using Unity.Behavior;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "Variable Is Null", story: "[Variable] is null", category: "Conditions")]
    public partial class VariableIsNullCondition : Condition
    {
        [SerializeReference] public BlackboardVariable Variable;

        public override bool IsTrue()
        {
            if (Variable.Type.IsValueType)
            {
                return false;
            }

            return Variable.ObjectValue is null || Variable.ObjectValue.Equals(null);
        }
    }
}