using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "CheckDetector", story: "Check if [FovDetector] has a [Target] tagged [Tag]", category: "Action", id: "b728d27c0a1f72cf322961b6b939b7a4")]
    public partial class CheckDetectorAction : Action
    {
    [SerializeReference] public BlackboardVariable<FovDetector> FovDetector;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<string> Tag;
    private FovDetector _fovDetector;
    private GameObject _detectedTarget;

        protected override Status OnStart()
        {
            _fovDetector = FovDetector.Value;
            if (_fovDetector == null)
            {
                LogFailure("No Detector assigned.");
                return Status.Failure;
            }

            _detectedTarget = _fovDetector.GetClosestTargetWithTag(Tag.Value);

            if (_detectedTarget)
            {
                Target.Value = _detectedTarget.transform;
                return Status.Success;
            }
            return Status.Failure;
        }
    }
}