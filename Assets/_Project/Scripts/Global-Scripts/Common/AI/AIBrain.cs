using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Controller.AI;
using MalbersAnimations.Scriptables;
using RenownedGames.AITree;
using System.Collections.Generic;
using UnityEngine;

namespace Malbers.Integration.AITree.Core
{
    public enum EffectedTarget { Self, Target }
    public enum ExecuteTask { OnStart, OnUpdate, OnExit }

    public class AIBrain : MonoBehaviour, IAnimatorListener
    {
        /// <summary>
        /// Reference for the Ai Control Movement
        /// </summary>
        public IAIControl AIControl { get; set; }

        // Time needed to make a new transition. Necesary to avoid Changing to multiple States in the same frame
        public FloatReference transitionCoolDown = new FloatReference(0.2f);

        [Tooltip("Removes all AI Components when the Animal Dies. (Brain, AiControl, Agent)")]
        public bool disableAIOnDeath = true;
        public bool debug = false;       

        // Last Time the Animal  started a transition
        public float StateLastTime { get; set; }

        // Tasks Local Vars (1 Int,1 Bool,1 Float)
        public BrainVars tasksVars;
        // Saves on the a Task that it has finish is stuff
        internal bool TasksDone;

        /// <summary>Reference for the Animal</summary>
        public MAnimal Animal { get; private set; }

        /// <summary>Reference for the AnimalStats</summary>
        public Dictionary<int, Stat> AnimalStats { get; private set; }

        #region Target References
        // Reference for the Current Target the Animal is using
        public Transform Target { get; private set; }

        // Reference for the Target the Animal Component
        public MAnimal TargetAnimal { get; private set; }

        // Current Animal AI position
        public Vector3 Position => AIControl.Transform.position;

        // Current Animal AI height
        public float AIHeight => Animal.transform.lossyScale.y * AIControl.StoppingDistance;

        // True if the Current Target has Stats
        public bool TargetHasStats { get; private set; }

        // Reference for the Target the Stats Component
        public Dictionary<int, Stat> TargetStats { get; private set; }
        #endregion

        // Reference for the Last WayPoint the Animal used
        public IWayPoint LastWayPoint { get; set; }

        // Time Elapsed for the State Decisions
        [HideInInspector] public float[] DecisionsTime;// { get; set; }
        BehaviourRunner behaviourRunner;

        #region Unity Callbakcs

        /// <summary>
        /// Find the Malbers AI components and set up the AI Brain properties
        /// </summary>
        private void Awake()
        {
            if (Animal == null)
            {
                Animal = gameObject.FindComponent<MAnimal>();
            }
            behaviourRunner = GetComponent<BehaviourRunner>();
            AIControl ??= gameObject.FindInterface<IAIControl>();

            Stats animalStatscomponent = Animal.FindComponent<Stats>();
            if (animalStatscomponent)
            {
                AnimalStats = animalStatscomponent.Stats_Dictionary();
            }

            Animal.isPlayer.Value = false; //If is using a brain... disable that he is the main player
        }

        /// <summary>
        /// Add Event listeners
        /// </summary>
        public void OnEnable()
        {
            AIControl.TargetSet.AddListener(OnTargetSet);
            Animal.OnStateChange.AddListener(OnAnimalStateChange);
        }

        /// <summary>
        /// Remove Event Listeners
        /// </summary>
        public void OnDisable()
        {
            AIControl.TargetSet.RemoveListener(OnTargetSet);
            Animal.OnStateChange.RemoveListener(OnAnimalStateChange);
            AIControl.Stop();
            StopAllCoroutines();                      
        }
        #endregion

        /// <summary>
        /// Writes a line to the console
        /// </summary>
        /// <param name="log"></param>
        /// <param name="val"></param>
        protected virtual void Debugging(string log, UnityEngine.Object val)
        {
            if (debug)
            {
                Debug.Log($"<B><color=green>[{Animal.name}]</color> - </B> " + log, val);
            }
        }

        /// <summary>
        /// Respond to the Animator triggering a behaviour
        /// </summary>
        /// <param name="message"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool OnAnimatorBehaviourMessage(string message, object value) => this.InvokeWithParams(message, value);

        #region SelfAnimal Event Listeners

        /// <summary>
        /// Handle OnAnimalStateChange event from the Animal AI.
        /// </summary>
        /// <param name="state"></param>
        private void OnAnimalStateChange(int state)
        {
            if (state == StateEnum.Death) //meaning this animal has died
            {
                StopAllCoroutines();
                behaviourRunner.SetEnable(false);
                AIControl.ClearTarget();
                
                enabled = false;

                if (disableAIOnDeath)
                {
                    AIControl.SetActive(false);
                    this.SetEnable(false);
                }
            }
        }
        #endregion

        /// <summary>
        /// Stores if the Current Target is an Animal and if it has the Stats component
        /// </summary>
        /// <param name="target"></param>
        private void OnTargetSet(Transform target)
        {
            Target = target;
            if (target)
            {

                TargetAnimal = target.FindComponent<MAnimal>();// ?? target.GetComponentInChildren<MAnimal>();
                TargetStats = null;
                Stats targetStats = target.FindComponent<Stats>();// ?? target.GetComponentInChildren<Stats>();

                TargetHasStats = targetStats != null;
                if (TargetHasStats)
                {
                    TargetStats = targetStats.Stats_Dictionary();
                }
            }
        }

        /// <summary>
        /// Sets the last waypoint to the given transform
        /// </summary>
        /// <param name="target"></param>
        public void SetLastWayPoint(Transform target)
        {
            IWayPoint newLastWay = target.gameObject.FindInterface<IWayPoint>();
            if (newLastWay != null)
            {
                LastWayPoint = target?.gameObject.FindInterface<IWayPoint>(); //If not is a waypoint save the last one
            }
        }
    }
}