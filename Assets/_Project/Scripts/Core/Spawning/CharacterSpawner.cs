using ECM2;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Behavior;
using UnityEngine.AI;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public class CharacterSpawner : MonoBehaviour, ISpawnable
    {
        #region Class Variables
        [FoldoutGroup("Events")] public UnityEvent SpawnEvent;
        [FoldoutGroup("Events")] public UnityEvent DespawnEvent;

        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private NavMeshCharacter _navMeshCharacter;
        private BehaviorGraphAgent _behaviourGraphAgent;

        public Spawner Spawner { get; set; }

        #endregion
        #region Startup

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshCharacter = GetComponent<NavMeshCharacter>();
            _behaviourGraphAgent = GetComponent<BehaviorGraphAgent>();
        }
        #endregion
        #region Class Methods

        public void PreSpawn()
        {
            DisableComponents();
        }

        public void Spawn()
        {
            EnableComponents();
            SpawnEvent.Invoke();
        }

        public void Despawn()
        {
            DisableComponents();
            DespawnEvent.Invoke();
        }

        private void DisableComponents()
        {
            _behaviourGraphAgent.enabled = false;
            _navMeshCharacter.enabled = false;
            _navMeshAgent.enabled = false;
        }

        private void EnableComponents()
        {
            _navMeshAgent.enabled = true;
            _navMeshCharacter.enabled = true;
            _behaviourGraphAgent.enabled = true;
        }
        #endregion
    }
}