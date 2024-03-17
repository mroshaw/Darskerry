using System.Collections;
using MalbersAnimations.Controller;
using MalbersAnimations.Controller.AI;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;

namespace DaftAppleGames.Common.Quests
{
    public class PauseAiAgent : MonoBehaviour
    {

        private NavMeshAgent _aiAgent;
        private Rigidbody _rigidbody;
        private MAnimalAIControl _aiController;
        private MAnimal _animal;

        private Vector3 _agentVelocity = Vector3.zero;

        private Vector3 _position;

        private float _speed;

        private bool _isPaused = false;

        private void Start()
        {
            _aiAgent = GetComponentInChildren<NavMeshAgent>(true);
            _aiController = GetComponentInChildren<MAnimalAIControl>(true);
            _rigidbody = GetComponentInChildren<Rigidbody>(true);
            _animal = GetComponentInChildren<MAnimal>();
        }

        /// <summary>
        /// Dialog Event Wrapper for Unpause
        /// </summary>
        /// <param name="transform"></param>
        public void UnPauseAgent(Transform transform)
        {
            UnPauseAgent();
        }

        /// <summary>
        /// Pause the NavMeshAgent
        /// </summary>
        public void PauseAgent(Transform playerTransform)
        {
            _animal.LockMovement = true;
            // _isPaused = true;
            //_position = transform.position;
            //_aiAgent.isStopped = true;
            // StartCoroutine(StopAgentAsync(playerTransform));
            Debug.Log($"Paused at: {_position.x}, {_position.y}, {_position.z}");

        }

        /// <summary>
        /// UnPause the NavMeshAgent
        /// </summary>
        public void UnPauseAgent()
        {
            _animal.LockMovement = false;
            //_aiAgent.isStopped = false;
            //_isPaused = false;

        }

        private IEnumerator StopAgentAsync(Transform playerTransform)
        {
            while (_isPaused)
            {
                _aiAgent.velocity = Vector3.zero;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                transform.position = _position;
                transform.LookAt(playerTransform);
                yield return null;
            }
            Debug.Log("Agent stopped...");
            Debug.Log($"Unpaused at: {transform.position.x}, {transform.position.y}, {transform.position.z}");
        }
    }
}
