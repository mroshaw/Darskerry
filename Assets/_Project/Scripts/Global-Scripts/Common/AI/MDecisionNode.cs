using RenownedGames.AITree;
using UnityEngine;

namespace Malbers.Integration.AITree.Core
{
    /// <summary>
    /// Base class for Malbers Animal AI nodes
    /// Provides shared and abstract definitions for all Malbers Animal AI Decision nodes designed for us
    /// with the AI Tree asset and Malbers Animal Controller.
    /// </summary>
    public abstract class MDecisionNode : ConditionDecorator
    {
        /// <summary>
        /// Exposes the AIBrain to all inheriting classes. This is a wrapper for Malbers components
        /// including AIControl and any other components we may need in the future.
        /// </summary>
        protected AIBrain AIBrain;

        /// <summary>
        /// Find the AIBrain component. This must be on the same GameObject as the BehaviourRunner component
        /// </summary>
        protected override void OnInitialize()
        {
            AIBrain = GetOwner().GetComponent<AIBrain>();

            if (!AIBrain)
            {
                Debug.LogError("The AIBrain component must be present on the same GameObject as BehaviourRunner!");
            }
        }
    }
}