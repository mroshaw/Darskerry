using System;
using Unity.Behavior;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "AI Target Is Dead", story: "[Target] of [Agent] is dead", category: "Conditions/Detection", id: "d5c47986fce649512068b39ccfe2acf5")]
    public partial class TargetIsDeadCondition : AiBrainCondition
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;

        public override bool IsTrue()
        {
            return false;
        }
    }
}