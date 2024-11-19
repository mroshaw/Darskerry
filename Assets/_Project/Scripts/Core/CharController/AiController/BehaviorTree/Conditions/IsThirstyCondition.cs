using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "IsThirsty", story: "Is AI thirsty using [AiBrain]", category: "Conditions",
        id: "515c33877af94ec88d8cc0aa86e2cbaf")]
    public partial class IsThirstyCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;

        public override bool IsTrue()
        {
            if (AiBrain.Value == null)
            {
                return false;
            }
            return AiBrain.Value.IsThirsty();
        }
    }
}