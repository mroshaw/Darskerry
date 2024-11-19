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
        Attacking,
        Moving
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
        [BoxGroup("Patrol Settings")][SerializeField] private PatrolRoute patrolRoute;
        [BoxGroup("Patrol Settings")][SerializeField] private float patrolSpeed;
        [BoxGroup("Patrol Settings")][SerializeField] private float patrolRotationRate;
        [BoxGroup("Patrol Settings")][SerializeField] private float patrolMinPause;
        [BoxGroup("Patrol Settings")][SerializeField] private float patrolMaxPause;

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
            _thirst = startingThirst;
        }

        protected virtual void Start()
        {
        }
        #endregion
        #region Update
        protected virtual void Update()
        {
            if (_thirst > 0)
            {
                _thirst -= thirstRate * Time.deltaTime;
            }
            else
            {
                _thirst = 0.0f;
            }
        }
        #endregion
        #region Class methods
        #region Move methods
        private void SetMoveSpeed(float speed)
        {
            _gameCharacter.maxWalkSpeed = speed;
        }

        private void SetRotationRate(float rotateRate)
        {
            _gameCharacter.rotationRate = rotateRate;
        }

        public void MoveTo(Vector3 position, NavMeshCharacter.DestinationReachedEventHandler arrivalCallBack)
        {
            SetMoveSpeed(wanderSpeed);
            _navMeshCharacter.DestinationReached -= arrivalCallBack;
            _navMeshCharacter.DestinationReached += arrivalCallBack;
            _navMeshCharacter.MoveToDestination(position);
        }
        #endregion
        #region Patrol methods

        public bool HasPatrolRoute()
        {
            return patrolRoute != null;
        }

        [Button("Start Patrol")]
        public void Patrol()
        {
            if(!patrolRoute || _aiState == AiState.Patrolling)
            {
                return;
            }

            _navMeshCharacter.DestinationReached -= ArrivedAtPatrolDestination;
            _navMeshCharacter.DestinationReached += ArrivedAtPatrolDestination;

            SetMoveSpeed(patrolSpeed);
            SetRotationRate(patrolRotationRate);

            _navMeshCharacter.MoveToDestination(patrolRoute.GetNextDestination().position);
            _aiState = AiState.Patrolling;
        }

        private void StopPatrolling()
        {
            _navMeshCharacter.DestinationReached -= ArrivedAtPatrolDestination;
            _aiState = AiState.Idle;
        }

        private void ArrivedAtPatrolDestination()
        {
            StartCoroutine(MoveToNextPatrolPointAfterDelayAsync());
        }

        private IEnumerator MoveToNextPatrolPointAfterDelayAsync()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(patrolMinPause, patrolMaxPause));
            _navMeshCharacter.MoveToDestination(patrolRoute.GetNextDestination().position);
        }
        #endregion
        #region Wander methods
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

        private void StopWandering()
        {
            _navMeshCharacter.StopMovement();
            _navMeshCharacter.DestinationReached -= ArrivedAtWanderDestination;
            _aiState = AiState.Idle;
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

        private IEnumerator MoveToRandomPositionAfterDelayAsync()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(wanderMinPause, wanderMaxPause));
            GoToRandomDestination();
        }

        private void ArrivedAtWanderDestination()
        {
            StartCoroutine(MoveToRandomPositionAfterDelayAsync());
        }
        #endregion
        #region Flee methods
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

        private void ArrivedAtFleeDestination()
        {
            StopFleeing();
        }
        #endregion
        #region Needs methods
        public bool IsThirsty()
        {
            return _thirst <= 0;
        }

        public void Drink(float thirstValue)
        {
            _thirst = (_thirst + thirstValue) > startingThirst ? startingThirst : (_thirst + thirstValue);
        }
        #endregion
        #endregion
    }
}