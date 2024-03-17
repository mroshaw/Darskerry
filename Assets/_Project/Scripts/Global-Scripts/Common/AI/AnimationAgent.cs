using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;

namespace DaftAppleGames.Common.AI
{
    public class AnimationAgent : MonoBehaviour
    {
        [BoxGroup("Movement Mode")]
        public bool useRootMotion = true;

        private Animator _animator;
        private NavMeshAgent _agent;
        private Vector2 _smoothDeltaPosition = Vector2.zero;

        [SerializeField]
        [BoxGroup("Debug")]
        private Vector2 _velocity = Vector2.zero;
        [SerializeField]
        [BoxGroup("Debug")]
        private LookAt _lookAt;

        [BoxGroup("Debug")]
        public float maxX = 0.0f;

        [BoxGroup("Debug")]
        public float maxY = 0.0f;


        // Animation hashes
        private static int MoveAnimationHash = Animator.StringToHash("Move");
        private static int RotateAnimationHash = Animator.StringToHash("Rotation");
        private static int ForwardSpeedAnimationHash = Animator.StringToHash("ForwardSpeed");
        private static int IdleMoveSetHash = Animator.StringToHash("IdleMoveSet");

        private  void Start()
        {
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _lookAt = GetComponent<LookAt>();

            if (useRootMotion)
            {
                // Don’t update position automatically
                _animator.applyRootMotion = true;
                _agent.updatePosition = false;
                _agent.updateRotation = false;
            }
            else
            {
                _animator.applyRootMotion = false;
                _agent.updatePosition = true;
                _agent.updateRotation = true;
            }
        }

       private void Update()
       {
           
           Vector3 worldDeltaPosition = _agent.nextPosition - transform.position;
           worldDeltaPosition.y = 0.0f;
           
           // Map 'worldDeltaPosition' to local space
           float dx = Vector3.Dot(transform.right, worldDeltaPosition);
           float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
           Vector2 deltaPosition = new Vector2(dx, dy);

           // Low-pass filter the deltaMove
           float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
           _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

           // Update velocity if time advances
           if (Time.deltaTime > 1e-5f)
               _velocity = _smoothDeltaPosition / Time.deltaTime;

           // Smooth velocity approaching stopping
           if (_agent.remainingDistance <= _agent.stoppingDistance)
           {
               _velocity = Vector2.Lerp(Vector2.zero, _velocity, _agent.remainingDistance / _agent.stoppingDistance);
           }

           bool shouldMove = _velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.stoppingDistance;

            // Update animation parameters
            _animator.SetBool(MoveAnimationHash, shouldMove);
            _animator.SetFloat(RotateAnimationHash, _velocity.x);
            _animator.SetFloat(ForwardSpeedAnimationHash, _velocity.y);

            // Debug
            if (_velocity.x > maxX)
            {
                maxX = _velocity.x;
            }

            if (_velocity.y > maxY)
            {
                maxY = _velocity.y;
            }

            float deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _agent.radius / 2.0f)
            {
                transform.position = Vector3.Lerp(_animator.rootPosition, _agent.nextPosition, smooth);
            }

            // _lookAt.lookAtTargetPosition = _agent.steeringTarget + transform.forward;
       }

       /// <summary>
       /// Sync our animator 
       /// </summary>
       void OnAnimatorMove()
       {
           if (useRootMotion)
           {
               Vector3 rootPosition = _animator.rootPosition;

               // Update position and rotation
               rootPosition.y = _agent.nextPosition.y;
               transform.position = rootPosition;
               transform.rotation = _animator.rootRotation;

               // Update NavMeshAgent
               _agent.nextPosition = rootPosition;
           }
           else
           {
               // Update position to agent position
               transform.position = _agent.nextPosition;
            }
        }
    }
}
