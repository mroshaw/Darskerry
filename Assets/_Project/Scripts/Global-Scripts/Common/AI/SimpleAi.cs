using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.AI
{
    public class SimpleAi : MonoBehaviour
    {
        [BoxGroup("Actions")]
        [SerializeField]
        private AiAction[] _actions;

        [BoxGroup("Debug")]
        [SerializeField]
        private AiAction _currentAction;

        [BoxGroup("Debug")]
        [SerializeField]
        private AiAction _nextAction;

        [BoxGroup("Debug")]
        [SerializeField]
        private AiAction _previousAction;

        /// <summary>
        /// Initialise the AI
        /// </summary>
        public void Start()
        {
            RefreshActions();
            _currentAction = PickNextAction();
        }

        /// <summary>
        /// Refresh the available actions
        /// </summary>
        private void RefreshActions()
        {
            _actions = GetComponents<AiAction>();
        }

        /// <summary>
        /// Pick the next action to perform
        /// </summary>
        /// <returns></returns>
        private AiAction PickNextAction()
        {
            return _actions[0];
        }

        /// <summary>
        /// Perform the current action
        /// </summary>
        private void DoCurrentAction()
        {
            _currentAction.DoAction();
        }

        /// <summary>
        /// Update the state and perform the action
        /// every frame
        /// </summary>
        private void Update()
        {
            DoCurrentAction();
        }
    }
}
