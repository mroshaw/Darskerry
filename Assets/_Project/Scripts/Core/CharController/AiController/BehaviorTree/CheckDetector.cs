using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "CheckDetector", story: "Check if [FovDetector] has a Target tagged [Tag]", category: "Action", id: "b728d27c0a1f72cf322961b6b939b7a4")]
    public partial class CheckDetectorAction : Action
    {
        [SerializeReference] public BlackboardVariable<FovDetector> FovDetector;
        [SerializeReference] public BlackboardVariable<string> Tag;
        private FovDetector fovDetector;

        protected override Status OnStart()
        {
            fovDetector = FovDetector.Value;
            if (fovDetector == null)
            {
                LogFailure("No Detector assigned.");
                return Status.Failure;
            }
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (fovDetector.CanSeeTargetsWithTag(Tag.Value))
            {
                return Status.Success;
            }
            return Status.Failure;
        }

        protected override void OnEnd()
        {
        }
    }

}