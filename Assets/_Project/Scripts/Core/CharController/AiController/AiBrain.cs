using ECM2;
using Sirenix.OdinInspector;
using Unity.Behavior;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    /// <summary>
    /// Helper component for AI Behaviour features
    /// </summary>
    public abstract class AiBrain : MonoBehaviour
    {
        private static string WanderSpeedVariableName = "WanderSpeed";
        private static string WanderMinRangeVariableName = "WanderMinRange";
        private static string WanderMaxRangeVariableName = "WanderMaxRange";
        private static string WanderMinPauseVariableName = "WanderMinPause";
        private static string WanderMaxPauseVariableName = "WanderMaxPause";
        private static string WanderCenterTransformVariableName = "WanderCenterTransform";
        private static string IsHungryVariableName = "IsHungry";
        private static string IsThirstyVariableName = "IsThirsty";

        #region Class Variables
        [BoxGroup("Blackboard Sync Settings")][SerializeField] private int blackboardRefreshInterval = 5;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderSpeed;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderMinRange;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderMaxRange;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderMinPause;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderMaxPause;
        [BoxGroup("Wander Settings")][SerializeField] private Transform wanderCenterTransform;

        [BoxGroup("Needs")][SerializeField] private float startingThirst = 100.0f;
        [BoxGroup("Needs")][SerializeField] private float thirstRate = 0.01f;
        [BoxGroup("Needs")][SerializeField] private float startingHunger = 100.0f;
        [BoxGroup("Needs")][SerializeField] private float hungerRate = 0.01f;
        [BoxGroup("Needs")][SerializeField] private float hungryThreshold = 10.0f;
        [BoxGroup("Needs")][SerializeField] private float thirstyThreshold = 10.0f;
        [BoxGroup("Needs")][SerializeField] private float hungerDamageRate = 0.5f;
        [BoxGroup("Needs")][SerializeField] private float thirstyDamageRate = 0.5f;

        [BoxGroup("Needs Debug")][SerializeField] private float _hunger;
        [BoxGroup("Needs Debug")][SerializeField] private float _thirst;

        private BehaviorGraphAgent _behaviorGraphAgent;
        private NavMeshCharacter _navMeshCharacter;
        private GameCharacter _gameCharacter;
        BlackboardReference _blackboardRef;
        #endregion

        #region Properties
        public NavMeshCharacter NavMeshCharacter => _navMeshCharacter;
        public GameCharacter GameCharacter => _gameCharacter;
        #endregion

        #region Startup
        protected virtual void Awake()
        {
            _navMeshCharacter = GetComponent<NavMeshCharacter>();
            _gameCharacter = GetComponent<GameCharacter>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _blackboardRef = _behaviorGraphAgent.BlackboardReference;
        }

        protected virtual void Start()
        {
            _hunger = startingHunger;
            _thirst = startingThirst;
            SetBlackboard();
        }
        #endregion

        #region Update
        protected virtual void Update()
        {
            if (_hunger > 0)
            {
                _hunger -= hungerRate * Time.deltaTime;
            }
            if (_thirst > 0)
            {
                _thirst -= thirstRate * Time.deltaTime;
            }

            if (Time.frameCount % blackboardRefreshInterval == 0)
            {
                SyncBlackboard();
            }
        }
        #endregion
        #region Class methods
        protected virtual void SetBlackboard()
        {
            SetVariableFloat(WanderSpeedVariableName, wanderSpeed);
            SetVariableFloat(WanderMinRangeVariableName, wanderMinRange);
            SetVariableFloat(WanderMaxRangeVariableName, wanderMaxRange);
            SetVariableFloat(WanderMinPauseVariableName, wanderMinPause);
            SetVariableFloat(WanderMaxPauseVariableName, wanderMaxPause);
            if (wanderCenterTransform)
            {
                SetVariableTransform(WanderCenterTransformVariableName, wanderCenterTransform);
            }
            else
            {
                SetVariableTransform(WanderCenterTransformVariableName, transform);
            }
        }

        protected virtual void SyncBlackboard()
        {
            SetVariableBool(IsHungryVariableName, IsHungry());
            SetVariableBool(IsThirstyVariableName, IsThirsty());
        }

        protected void SetVariableFloat(string variableName, float floatValue)
        {
            BlackboardVariable<float> blackboardFloat = new();
            if (_blackboardRef.GetVariable(variableName, out blackboardFloat))
            {
                blackboardFloat.Value = floatValue;
            }
        }

        protected void SetVariableTransform(string variableName, Transform transformValue)
        {
            BlackboardVariable<Transform> blackboardTransform = new();
            if (_blackboardRef.GetVariable(variableName, out blackboardTransform))
            {
                blackboardTransform.Value = transformValue;
            }
        }

        protected void SetVariableBool(string variableName, bool boolValue)
        {
            BlackboardVariable<bool> blackboardBool = new();
            if (_blackboardRef.GetVariable(variableName, out blackboardBool))
            {
                blackboardBool.Value = boolValue;
            }
        }

        public void SetMoveSpeed(float speed)
        {
            _gameCharacter.maxWalkSpeed = speed;
        }

        private bool IsHungry()
        {
            return _hunger < hungryThreshold;
        }

        private bool IsThirsty()
        {
            return _thirst < thirstyThreshold;
        }


        public void Eat(float foodValue)
        {
            _hunger = (_hunger + foodValue) > startingHunger ? startingHunger : (_hunger + foodValue);
        }

        public void Drink(float thirstValue)
        {
            _thirst = (_thirst + thirstValue) > startingThirst ? startingThirst : (_thirst + thirstValue);
        }
        #endregion
    }
}