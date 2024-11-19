using DaftAppleGames.Darskerry.Core.CharController.AiController;
using System;
using Unity.Behavior;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "Target Detected", story: "[Detector] sees [Target] with tag [Tag]", category: "Conditions",
        id: "8726cdf612c610ebfe8addaa580d1b4b")]
    public partial class DetectTargetCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<FovDetector> Detector;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<string> Tag;

        public override bool IsTrue()
        {
            if (Detector.Value == null)
            {
                return false;
            }
            GameObject target = Detector.Value.GetClosestTargetWithTag(Tag.Value);
            if (target)
            {
                Target.Value = target.transform;
                return true;
            }
            return false;
        }
    }
}