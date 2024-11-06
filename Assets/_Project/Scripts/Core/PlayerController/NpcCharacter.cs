using ECM2;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public enum MovementSpeed { Walking = 2, Running = 4, Sprinting = 6 }

    public class NpcCharacter : HumanCharacter
    {
        #region Class Variables
        [PropertyOrder(-1)][BoxGroup("Wander Settings")] public Transform wanderAreaCenter;
        [PropertyOrder(-1)][BoxGroup("Wander Settings")] public bool wandering;
        [PropertyOrder(-1)][BoxGroup("Wander Settings")] public float minWanderDistance;
        [PropertyOrder(-1)][BoxGroup("Wander Settings")] public float maxWanderDistance;
        [PropertyOrder(-1)][BoxGroup("Wander Settings")] public MovementSpeed movementSpeed;
        [PropertyOrder(-1)][BoxGroup("Wander Settings")] public float minDelayBetweenStops = 1.0f;
        [PropertyOrder(-1)][BoxGroup("Wander Settings")] public float maxDelayBetweenStops = 5.0f;

        private NavMeshCharacter _navMeshCharacter;
        #endregion

        #region Startup
        protected override void Awake()
        {
            base.Awake();
            _navMeshCharacter = GetComponent<NavMeshCharacter>();

        }

        protected override void Start()
        {
            base.Start();
            if (!wanderAreaCenter)
            {
                wanderAreaCenter = transform;
            }

            if (wandering)
            {
                _navMeshCharacter.DestinationReached += ArrivedAtDestination;
                // If character doesn't have a destination set or has arrived
                if (wandering && !_navMeshCharacter.IsPathFollowing())
                {
                    GoToNextDestination();
                }
            }
        }
        #endregion

        #region Update Logic
        #endregion

        #region Class methods
        private void GoToNextDestination()
        {
            maxWalkSpeed = (int)movementSpeed;
            _navMeshCharacter.MoveToDestination(GetRandomLocation(wanderAreaCenter.position, minWanderDistance, maxWanderDistance));
        }

        private void ArrivedAtDestination()
        {
            StartCoroutine(MoveToNextAfterDelayAsync());
        }

        private IEnumerator MoveToNextAfterDelayAsync()
        {
            yield return new WaitForSeconds(Random.Range(minDelayBetweenStops, maxDelayBetweenStops));
            GoToNextDestination();
        }

        private Vector3 GetRandomLocation(Vector3 center, float minDistance, float maxDistance)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDistance, maxDistance);
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
        #endregion

        #region Editor code
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(wanderAreaCenter ? wanderAreaCenter.position : gameObject.transform.position, minWanderDistance);
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(wanderAreaCenter ? wanderAreaCenter.position : gameObject.transform.position, maxWanderDistance);

        }

#endif
        #endregion
    }
}