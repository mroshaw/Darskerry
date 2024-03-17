using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DaftAppleGames.Common.AI
{
    public enum MoveSpeed { Walk, Trot, Run, Sprint }

    public class SimpleWanderAction : AiAction
    {
        [BoxGroup("Wander Action Settings")]
        public Transform wanderAnchor;
        [BoxGroup("Wander Action Settings")]
        public float wanderRadius;
        [BoxGroup("Wander Action Settings")]
        public float minWanderDistance;
        [BoxGroup("Wander Action Settings")]
        public float maxWanderDistance;
        [BoxGroup("Wander Action Settings")]
        public float minIdleTime;
        [BoxGroup("Wander Action Settings")]
        public float maxIdleTime;

        [BoxGroup("Speed Settings")]
        public MoveSpeed moveSpeed;
        [BoxGroup("Speed Settings")]
        public MoveSpeeds walkSpeeds = new MoveSpeeds(1.0f, 130.0f);
        [BoxGroup("Speed Settings")]
        public MoveSpeeds trotSpeeds = new MoveSpeeds(1.8f, 110.0f);
        [BoxGroup("Speed Settings")]
        public MoveSpeeds runSpeeds = new MoveSpeeds(2.5f, 90.0f);
        [BoxGroup("Speed Settings")]
        public MoveSpeeds sprintSpeeds = new MoveSpeeds(3.0f, 80.0f);

        [BoxGroup("Wander Action Settings")]
        public float destinationThreshhold = 0.01f;

        private NavMeshAgent _agent;

        private bool _isIdle;

        /// <summary>
        /// Setup movement
        /// </summary>
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            SetNavMeshSpeed(moveSpeed);
        }

        /// <summary>
        /// Perform the Wander action
        /// </summary>
        public override void DoAction()
        {
            // If no path is set, set one
            if (!_agent.hasPath)
            {
                _agent.SetDestination(GetNewTarget());
                return;
            }

            // If destination has been reached, wait before moving on.
            if(_agent.remainingDistance < destinationThreshhold && !_isIdle)
            {
                // Get random wait time
                System.Random random = new System.Random();
                float idleWait = (float)random.NextDouble() * (maxIdleTime - minIdleTime) + minIdleTime;

                StartCoroutine(SetNewTargetAsync(idleWait));
            }
        }

        /// <summary>
        /// Updates the speed of the movement
        /// </summary>
        /// <param name="speed"></param>
        public void SetNavMeshSpeed(MoveSpeed speed)
        {
            switch (speed)
            {
                case MoveSpeed.Walk:
                    _agent.speed = walkSpeeds.ForwardSpeed;
                    _agent.angularSpeed = walkSpeeds.RotateSpeed;
                    break;

                case MoveSpeed.Trot:
                    _agent.speed = trotSpeeds.ForwardSpeed;
                    _agent.angularSpeed = trotSpeeds.RotateSpeed;
                    break;

                case MoveSpeed.Run:
                    _agent.speed = runSpeeds.ForwardSpeed;
                    _agent.angularSpeed = runSpeeds.RotateSpeed;
                    break;

                case MoveSpeed.Sprint:
                    _agent.speed = sprintSpeeds.ForwardSpeed;
                    _agent.angularSpeed = sprintSpeeds.RotateSpeed;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator SetNewTargetAsync(float delay)
        {
            Debug.Log($"SimpleWanderAction: Waiting for {delay} seconds...");
            _agent.isStopped = true;
            _isIdle = true;
            yield return new WaitForSeconds(delay);
            _isIdle = false;
            _agent.SetDestination(GetNewTarget());
            _agent.isStopped = false;
        }

        /// <summary>
        /// Locate a new target in range, on the NavMesh
        /// </summary>
        /// <returns></returns>
        private Vector3 GetNewTarget()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += wanderAnchor.position;

            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
            {
                finalPosition = hit.position;
            }

            return finalPosition;
        }

        [Serializable]
        public class MoveSpeeds
        {
            public float ForwardSpeed;
            public float RotateSpeed;

            public MoveSpeeds(float forwardSpeed, float rotateSpeed)
            {
                ForwardSpeed = forwardSpeed;
                RotateSpeed = rotateSpeed;
            }
        }
    }
}
