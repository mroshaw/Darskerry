using RenownedGames.AITree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Malbers.Integration.AITree.Core.Decisions
{
    /// <summary>
    /// AI Tree decision that returns true if the AI Animal has arrived at its target destination and false if it
    /// has not.
    /// </summary>
    [NodeContent("Arrive Decision", "Animal Controller/Movement/Arrive Decision", IconPath = "Icons/AIDecision_Icon.png")]
    public class MArriveDecision : MDecisionNode
    {
        [BoxGroup("Node")] [Tooltip("(OPTIONAL) Use this if you want to know if we have arrived to a specific Target")]
        public string targetName = string.Empty;

        private bool _hasArrived;
        private float _remainingDistance;

        /// <summary>
        /// Determines the result of the decision by checking that a valid target has been set, and then checking
        /// if the AIControl reports that the animal has arrived at it's destination.
        /// </summary>
        /// <returns></returns>
        protected override bool CalculateResult()
        {
            _remainingDistance = AIBrain.AIControl.RemainingDistance;
            _hasArrived = string.IsNullOrEmpty(targetName) ? AIBrain.AIControl.HasArrived :
                AIBrain.AIControl.HasArrived && (AIBrain.AIControl.Target.name == targetName || AIBrain.AIControl.Target.root.name == targetName);
            return _hasArrived;
        }

        /// <summary>
        /// Returns the description of the node
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return !string.IsNullOrEmpty(targetName)
                ? $"Specific Target: {targetName}\n"
                : $"Specific Target: {targetName}\nRemaining Distance: {_remainingDistance}\nHas Arrived: {_hasArrived}\n";
        }
    }
}