using MalbersAnimations;
using MalbersAnimations.Controller;
using RenownedGames.AITree;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Malbers.Integration.AITree.Core
{
    public enum EffectedTarget { Self, Target }
    public enum ExecuteTask { OnStart, OnUpdate, OnExit }

    public class AIBrain : MonoBehaviour, IAnimatorListener
    {
        private const string MoveTargetBlackboardKeyName = "MoveTarget";
        private const string FreeToWanderKeyName = "FreeToWander";
        private const string LoopPatrolKeyName = "LoopPatrol";
        private const string PatrolTransformBlackboardKeyName = "PatrolTransform";

        [BoxGroup("Movement")] public Transform defaultPatrolTransform;
        [BoxGroup("Movement")] public bool isFreeToWander = false;
        [BoxGroup("Movement")] public bool isLoopPatrol = true;

        [BoxGroup("Other Settings")] [Tooltip("Removes all AI Components when the Animal Dies. (Brain, AiControl, Agent)")]
        bool disableAIOnDeath = true;

        /// <summary>
        /// AI Control is used to direct the AI and integrate with the NavMesh
        /// </summary>
        public IAIControl AIControl { get; private set; }

        /// <summary>
        /// MAnimal is used to change state, change speed groups, etc.
        /// </summary>
        public MAnimal Animal { get; private set; }

        /// <summary>
        /// Animal Stats, to be exposed via the Blackboard
        /// </summary>
        public Stats AnimalStats { get; private set; }

        // Saves on the a Task that it has finish is stuff
        internal bool TasksDone;
        private BehaviourRunner _behaviourRunner;
        private Blackboard _blackboard;

        /// <summary>
        /// Find the Malbers AI components and set up the AI Brain properties
        /// </summary>
        private void Awake()
        {
            // Get references to key Malbers components
            Animal = gameObject.FindComponent<MAnimal>();
            AIControl = gameObject.FindInterface<IAIControl>();
            AnimalStats = Animal.FindComponent<Stats>();

            // Get a reference to the AI Tree runner and Blackboard, so we can sync and manage Malbers
            // specific keys and values
            _behaviourRunner = GetComponent<BehaviourRunner>();
            _blackboard = _behaviourRunner.GetBlackboard();

            // If a default target is set, update the blackboard
            if (defaultPatrolTransform)
            {
                SetBlackboardKey(PatrolTransformBlackboardKeyName, defaultPatrolTransform);
            }

            SetBlackboardKey(FreeToWanderKeyName, isFreeToWander);
            SetBlackboardKey(LoopPatrolKeyName, isLoopPatrol);
        }

        /// <summary>
        /// Add Event listeners to the Malbers animal.
        /// OnStateChange - capture the "Death" state, so we can update and disable the AI Tree components.
        /// </summary>
        public void OnEnable()
        {
            Animal.OnStateChange.AddListener(OnAnimalStateChange);
        }

        /// <summary>
        /// Remove Event Listeners and stop the AI
        /// </summary>
        public void OnDisable()
        {
            Animal.OnStateChange.RemoveListener(OnAnimalStateChange);
            AIControl.Stop();
            StopAllCoroutines();                      
        }

        /// <summary>
        /// Respond to the Animator triggering a behaviour
        /// </summary>
        /// <param name="message"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool OnAnimatorBehaviourMessage(string message, object value) => this.InvokeWithParams(message, value);

        /// <summary>
        /// Handle OnAnimalStateChange event from the Animal AI.
        /// </summary>
        /// <param name="state"></param>
        private void OnAnimalStateChange(int state)
        {
            if (state == StateEnum.Death)
            {
                StopAllCoroutines();
                _behaviourRunner.SetEnable(false);
                AIControl.ClearTarget();
                
                enabled = false;

                if (disableAIOnDeath)
                {
                    AIControl.SetActive(false);
                    this.SetEnable(false);
                }
            }
        }

        private void SetBlackboardKey(string keyName, Transform keyValue)
        {
            if (_blackboard.TryGetKey(keyName, out TransformKey transformKey))
            {
                transformKey.SetValue(keyValue);
            }
        }

        private void SetBlackboardKey(string keyName, bool keyValue)
        {
            if (_blackboard.TryGetKey(keyName, out BoolKey boolKey))
            {
                boolKey.SetValue(keyValue);
            }
        }

        private void SetBlackboardKey(string keyName, int keyValue)
        {
            if (_blackboard.TryGetKey(keyName, out IntKey intKey))
            {
                intKey.SetValue(keyValue);
            }
        }

        private void SetBlackboardKey(string keyName, float keyValue)
        {
            if (_blackboard.TryGetKey(keyName, out FloatKey floatKey))
            {
                floatKey.SetValue(keyValue);
            }
        }

        private Transform GetBlackboardTransformKey(string keyName)
        {
            return _blackboard.TryGetKey(keyName, out TransformKey transformKey) ? transformKey.GetValue() : null;
        }

        private bool GetBlackboardBoolKey(string keyName)
        {
            return _blackboard.TryGetKey(keyName, out BoolKey boolKey) && boolKey.GetValue();
        }

        private int GetBlackboardIntKey(string keyName)
        {
            return _blackboard.TryGetKey(keyName, out IntKey intKey) ? intKey.GetValue() : 0;
        }

        private float GetBlackboardFloatKey(string keyName)
        {
            return _blackboard.TryGetKey(keyName, out FloatKey floatKey) ? floatKey.GetValue() : 0.0f;
        }
    }
}