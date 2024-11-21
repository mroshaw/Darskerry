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
        [BoxGroup("Needs")][SerializeField] private WaterSource[] knownWaterSources;

        [BoxGroup("Needs Debug")][SerializeField] private float _thirst;

        private BlackboardReference _blackboardRef;
        #endregion
        #region Properties
        public NavMeshCharacter NavMeshCharacter { get; private set; }
        public GameCharacter GameCharacter { get; private set; }
        public FovDetector FovDetector { get; private set; }
        public Animator Animator { get; private set; }
        public AiState AiState { get; private set; }
        #endregion
        #region Startup
        protected virtual void Awake()
        {
            NavMeshCharacter = GetComponent<NavMeshCharacter>();
            GameCharacter = GetComponent<GameCharacter>();
            FovDetector = GetComponent<FovDetector>();
            Animator = GetComponent<Animator>();

            if (!wanderCenterTransform)
            {
                wanderCenterTransform = transform;
            }

            AiState = AiState.Idle;
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
        public void SetMoveSpeed(float speed)
        {
            GameCharacter.maxWalkSpeed = speed;
        }

        public void SetRotationRate(float rotateRate)
        {
            GameCharacter.rotationRate = rotateRate;
        }

        public bool HasArrived()
        {
            return !NavMeshCharacter.agent.hasPath || NavMeshCharacter.agent.velocity.sqrMagnitude == 0.0f;
        }

        public void MoveTo(Vector3 position)
        {
            SetMoveSpeed(wanderSpeed);
            SetRotationRate(wanderRotationRate);
            NavMeshCharacter.MoveToDestination(position);
        }

        public void MoveTo(Vector3 position, float moveSpeed, float rotationRate)
        {
            SetMoveSpeed(moveSpeed);
            SetRotationRate(rotationRate);
            NavMeshCharacter.MoveToDestination(position);
        }

        public void MoveTo(Vector3 position, NavMeshCharacter.DestinationReachedEventHandler arrivalCallBack)
        {
            SetMoveSpeed(wanderSpeed);
            NavMeshCharacter.DestinationReached -= arrivalCallBack;
            NavMeshCharacter.DestinationReached += arrivalCallBack;
            NavMeshCharacter.MoveToDestination(position);
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
            if(!patrolRoute || AiState == AiState.Patrolling)
            {
                return;
            }

            NavMeshCharacter.DestinationReached -= ArrivedAtPatrolDestination;
            NavMeshCharacter.DestinationReached += ArrivedAtPatrolDestination;

            SetMoveSpeed(patrolSpeed);
            SetRotationRate(patrolRotationRate);

            NavMeshCharacter.MoveToDestination(patrolRoute.GetNextDestination().position);
            AiState = AiState.Patrolling;
        }

        private void StopPatrolling()
        {
            NavMeshCharacter.DestinationReached -= ArrivedAtPatrolDestination;
            AiState = AiState.Idle;
        }

        private void ArrivedAtPatrolDestination()
        {
            StartCoroutine(MoveToNextPatrolPointAfterDelayAsync());
        }

        private IEnumerator MoveToNextPatrolPointAfterDelayAsync()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(patrolMinPause, patrolMaxPause));
            NavMeshCharacter.MoveToDestination(patrolRoute.GetNextDestination().position);
        }
        #endregion
        #region Wander methods
        public void Wander()
        {
            if (AiState == AiState.Wandering)
            {
                return;
            }
            SetMoveSpeed(wanderSpeed);
            SetRotationRate(wanderRotationRate);
            NavMeshCharacter.DestinationReached += ArrivedAtWanderDestination;
            GoToRandomDestination();
            AiState = AiState.Wandering;
        }

        private void StopWandering()
        {
            NavMeshCharacter.StopMovement();
            NavMeshCharacter.DestinationReached -= ArrivedAtWanderDestination;
            AiState = AiState.Idle;
        }

        private void GoToRandomDestination()
        {
            Vector3 wanderLocation = GetRandomWanderLocation(wanderCenterTransform.position, wanderMinRange, wanderMaxRange);
            NavMeshCharacter.MoveToDestination(wanderLocation);
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
            switch (AiState)
            {
                case AiState.Fleeing:
                    return;
                case AiState.Wandering:
                    StopWandering();
                    break;
            }

            SetMoveSpeed(fleeSpeed);
            SetRotationRate(fleeRotationRate);
            NavMeshCharacter.MoveToDestination(GetFleeDestination(fleeFromTarget));
            NavMeshCharacter.DestinationReached += ArrivedAtFleeDestination;
            AiState = AiState.Fleeing;
        }

        private void StopFleeing()
        {
            NavMeshCharacter.DestinationReached -= ArrivedAtFleeDestination;
            StartCoroutine(RestAfterFlee());
        }

        private IEnumerator RestAfterFlee()
        {
            yield return new WaitForSeconds(fleeRestTime);
            AiState = AiState.Idle;
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

        public bool Drink(float thirstValue)
        {
            _thirst = (_thirst + thirstValue) > startingThirst ? startingThirst : (_thirst + thirstValue);
            return Mathf.Approximately(_thirst, startingThirst);
        }

        [Button("Update Water Sources")]
        private void GetWaterSources()
        {
            knownWaterSources = FindObjectsByType<WaterSource>(FindObjectsSortMode.None);
        }

        public WaterSource GetClosestWaterSource()
        {
            if (knownWaterSources == null || knownWaterSources.Length == 0)
                return null;

            WaterSource closestWaterSource = null;
            float closestDistanceSqr = float.MaxValue;
            Vector3 playerPosition = transform.position;

            foreach (WaterSource waterSource in knownWaterSources)
            {
                if (waterSource == null)
                    continue;

                Vector3 directionToWaterSource = waterSource.transform.position - playerPosition;
                float distanceSqr = directionToWaterSource.sqrMagnitude;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestWaterSource = waterSource;
                }
            }

            return closestWaterSource;
        }
        #endregion
        #endregion
    }
}