using System.Collections;
using ECM2;
using Sirenix.OdinInspector;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public enum AiState
    {
        Idle,
        Wandering,
        Patrolling,
        Fleeing,
        Attacking
    }

    public enum RelationshipToPlayer
    {
        Fearful,
        Enemy,
        Friend,
        Neutral
    }

    /// <summary>
    /// Helper component for AI Behaviour features
    /// </summary>
    public abstract class AiBrain : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Wander Settings")][SerializeField] private float wanderSpeed;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderRotationRate;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderMinRange;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderMaxRange;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderMinPause;
        [BoxGroup("Wander Settings")][SerializeField] private float wanderMaxPause;
        [BoxGroup("Wander Settings")][SerializeField] private Transform wanderCenterTransform;

        [BoxGroup("Flee Settings")][SerializeField] private float fleeSpeed;
        [BoxGroup("Flee Settings")][SerializeField] private float fleeRotationRate;
        [BoxGroup("Flee Settings")][SerializeField] private float fleeMinRange;
        [BoxGroup("Flee Settings")][SerializeField] private float fleeMaxRange;
        [BoxGroup("Flee Settings")] [SerializeField] private float fleeRestTime;

        [BoxGroup("Needs")][SerializeField] private float startingThirst = 100.0f;
        [BoxGroup("Needs")][SerializeField] private float thirstRate = 0.01f;
        [BoxGroup("Needs")][SerializeField] private float startingHunger = 100.0f;
        [BoxGroup("Needs")][SerializeField] private float hungerRate = 0.01f;
        [BoxGroup("Needs")][SerializeField] private float hungryThreshold = 10.0f;
        [BoxGroup("Needs")][SerializeField] private float thirstyThreshold = 10.0f;

        [BoxGroup("Needs Debug")][SerializeField] private float _hunger;
        [BoxGroup("Needs Debug")][SerializeField] private float _thirst;

        private NavMeshCharacter _navMeshCharacter;
        private GameCharacter _gameCharacter;

        private AiState _aiState;

        private BlackboardReference _blackboardRef;
        #endregion
        #region Properties
        public NavMeshCharacter NavMeshCharacter => _navMeshCharacter;
        public GameCharacter GameCharacter => _gameCharacter;
        public AiState State => _aiState;
        #endregion
        #region Startup
        protected virtual void Awake()
        {
            _navMeshCharacter = GetComponent<NavMeshCharacter>();
            _gameCharacter = GetComponent<GameCharacter>();

            if (!wanderCenterTransform)
            {
                wanderCenterTransform = transform;
            }

            _aiState = AiState.Idle;

        }

        protected virtual void Start()
        {
            _hunger = startingHunger;
            _thirst = startingThirst;
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
        }
        #endregion
        #region Class methods
        private void SetMoveSpeed(float speed)
        {
            _gameCharacter.maxWalkSpeed = speed;
        }

        private void SetRotationRate(float rotateRate)
        {
            _gameCharacter.rotationRate = rotateRate;
        }

        public void Wander()
        {
            if (_aiState == AiState.Wandering)
            {
                return;
            }
            SetMoveSpeed(wanderSpeed);
            SetRotationRate(wanderRotationRate);
            _navMeshCharacter.DestinationReached += ArrivedAtWanderDestination;
            GoToRandomDestination();
            _aiState = AiState.Wandering;
        }
        public void Flee(Transform fleeFromTarget)
        {
            switch (_aiState)
            {
                case AiState.Fleeing:
                    return;
                case AiState.Wandering:
                    StopWandering();
                    break;
            }

            SetMoveSpeed(fleeSpeed);
            SetRotationRate(fleeRotationRate);
            _navMeshCharacter.MoveToDestination(GetFleeDestination(fleeFromTarget));
            _navMeshCharacter.DestinationReached += ArrivedAtFleeDestination;
            _aiState = AiState.Fleeing;
        }

        private void StopFleeing()
        {
            _navMeshCharacter.DestinationReached -= ArrivedAtFleeDestination;
            StartCoroutine(RestAfterFlee());
        }

        private IEnumerator RestAfterFlee()
        {
            yield return new WaitForSeconds(fleeRestTime);
            _aiState = AiState.Idle;
        }

        public void StopWandering()
        {
            _navMeshCharacter.StopMovement();
            _navMeshCharacter.DestinationReached -= ArrivedAtWanderDestination;
            _aiState = AiState.Idle;

        }

        private Vector3 GetFleeDestination(Transform targetTransform)
        {
            float fleeRange = Random.Range(fleeMinRange, fleeMaxRange);
            Vector3 fleePosition = transform.position + ((transform.position - targetTransform.position).normalized) * fleeRange;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = fleePosition;

            if (NavMesh.SamplePosition(fleePosition, out NavMeshHit myNavHit, 100, -1))
            {
                return myNavHit.position;
            }
            return fleePosition;
        }

        private void GoToRandomDestination()
        {
            Vector3 wanderLocation = GetRandomWanderLocation(wanderCenterTransform.position, wanderMinRange, wanderMaxRange);
            _navMeshCharacter.MoveToDestination(wanderLocation);
        }

        private Vector3 GetRandomWanderLocation(Vector3 center, float minDistance, float maxDistance)
        {
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            float distance = UnityEngine.Random.Range(minDistance, maxDistance);
            Vector3 offset = new Vector3(randomDirection.x, 0, randomDirection.y) * distance;
            Vector3 randomPosition = center + offset;

            if (Terrain.activeTerrain)
            {
                float terrainHeight = Terrain.activeTerrain.SampleHeight(randomPosition);
                randomPosition.y = terrainHeight;
            }

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit myNavHit, 100, -1))
            {
                return myNavHit.position;
            }
            return randomPosition;
        }

        private IEnumerator MoveToNextAfterDelayAsync()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(wanderMinPause, wanderMaxPause));
            GoToRandomDestination();
        }

        private void ArrivedAtWanderDestination()
        {
            StartCoroutine(MoveToNextAfterDelayAsync());
        }

        private void ArrivedAtFleeDestination()
        {
            StopFleeing();
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