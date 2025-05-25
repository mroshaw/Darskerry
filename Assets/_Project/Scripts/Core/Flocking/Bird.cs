using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Flocking
{
    public class Bird : MonoBehaviour
    {
        #region Class properties

        [SerializeField] private float movementSpeed;
        [SerializeField] private float nearbyBirdRadius;
        [SerializeField] private float cohesionRuleThreshold;
        [SerializeField] private float cohesionRuleCoefficient;
        [Range(0f, 1f)] [SerializeField] private float directionLowPassFilterCutoff;

        private FlockManager _flockManager;
        private List<Vector3> _nearbyBirds;
        private Collider[] _nearbyBirdsCache;
        private Rigidbody _rigidBody;

        private float _cohesionRuleWeight;
        private float _alignmentRuleWeight;
        private float _separationRuleWeight;
        private float _borderAvoidanceRuleWeight;

        private Vector3 _separationDirection;
        private Vector3 _averageHeadingDirection;
        private Vector3 _centerOfMassDirection;
        private Vector3 _borderAvoidanceDirection;

        private float _flockRadius;
        private Vector3 _flockOrigin;
        private Vector3 _previousDirection;

        #endregion

        #region Startup

        private void Awake()
        {
            _nearbyBirds = new List<Vector3>();
            _rigidBody = GetComponent<Rigidbody>();
            _previousDirection = Vector3.zero;
        }

        #endregion

        #region Update

        private void Update()
        {
            CalculateAlignmentRule();
            CalculateCohesionRule();
            CalculateSeparationRule();
            CalculateBorderAvoidanceRule();


            Vector3 direction = _separationRuleWeight * _separationDirection +
                                _cohesionRuleWeight * _centerOfMassDirection +
                                _alignmentRuleWeight * _averageHeadingDirection +
                                _borderAvoidanceRuleWeight * _borderAvoidanceDirection;

            direction = Vector3.Lerp(direction, _previousDirection, directionLowPassFilterCutoff);
            _previousDirection = direction;

            Quaternion lookQuaternion = Quaternion.LookRotation(direction.normalized);
            transform.rotation = lookQuaternion;
            _rigidBody.linearVelocity = transform.forward * movementSpeed;
        }

        #endregion

        #region Class methods

        private void CalculateSeparationRule()
        {
            Vector3 separationDirection = Vector3.zero;

            int numBirds = Physics.OverlapSphereNonAlloc(transform.position, nearbyBirdRadius, _nearbyBirdsCache);

            for (int currBirdIndex = 0; currBirdIndex < numBirds; currBirdIndex++)
            {
                if (transform.position.Equals(_nearbyBirdsCache[currBirdIndex].transform.position))
                {
                    continue;
                }

                Vector3 dist = Vector3.zero;

                if (_nearbyBirdsCache[currBirdIndex].tag.Equals("Bird"))
                {
                    dist = (_nearbyBirdsCache[currBirdIndex].transform.position - transform.position).normalized;
                    dist /= Vector3.Distance(_nearbyBirdsCache[currBirdIndex].transform.position, transform.position);
                }
                else if (_nearbyBirdsCache[currBirdIndex].tag.Equals("BirdObstacle"))
                {
                    dist = (_nearbyBirdsCache[currBirdIndex].ClosestPoint(transform.position) - transform.position).normalized;
                    dist /= Vector3.Distance(_nearbyBirdsCache[currBirdIndex].ClosestPoint(transform.position), transform.position);
                }

                // dist /= Vector3.Distance(col.transform.position, transform.position);
                separationDirection -= dist;
            }

            _separationDirection = separationDirection.normalized;
            _nearbyBirds.Clear();
        }

        private void CalculateAlignmentRule()
        {
            _averageHeadingDirection = (transform.position - _flockManager.GetAverageHeading().normalized).normalized;
        }

        private void CalculateCohesionRule()
        {
            Vector3 com = _flockManager.GetCenterofMass();
            _centerOfMassDirection = (com - transform.position).normalized;

            if (Vector3.Distance(transform.position, com) <= cohesionRuleThreshold)
            {
                _centerOfMassDirection *= cohesionRuleCoefficient;
            }
        }

        private void CalculateBorderAvoidanceRule()
        {
            if (Vector3.Distance(transform.position, _flockOrigin) >= _flockRadius)
            {
                _borderAvoidanceDirection = _flockOrigin - transform.position;
            }

            _borderAvoidanceDirection = _borderAvoidanceDirection.normalized;
        }

        internal void SetFlockManager(FlockManager flockManager)
        {
            _flockManager = flockManager;
        }

        internal void SetWeights(float separationRuleWeight, float cohesionRuleWeight, float alignmentRuleWeight, float borderAvoidanceRuleWeight)
        {
            _separationRuleWeight = separationRuleWeight;
            _cohesionRuleWeight = cohesionRuleWeight;
            _alignmentRuleWeight = alignmentRuleWeight;
            _borderAvoidanceRuleWeight = borderAvoidanceRuleWeight;
        }

        internal void SetFlockOrigin(Vector3 position)
        {
            _flockOrigin = position;
        }

        internal void SetFlockRadius(float radius)
        {
        }

        #endregion
    }
}