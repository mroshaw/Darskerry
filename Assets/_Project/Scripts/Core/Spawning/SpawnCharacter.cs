using System.Collections;
using ECM2;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Behavior;
using UnityEngine.AI;
using UnityEngine.Events;
using Action = System.Action;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public class SpawnCharacter : MonoBehaviour, ISpawnable
    {
        #region Class Variables
        [BoxGroup("Spawn Settings")] [SerializeField] private bool fadeIn = true;
        [BoxGroup("Spawn Settings")] [SerializeField] private float fadeInTime = 2.0f;
        [BoxGroup("Spawn Settings")] [SerializeField] private bool fadeOut = true;
        [BoxGroup("Spawn Settings")] [SerializeField] private float fadeOutTime = 2.0f;

        [FoldoutGroup("Events")] public UnityEvent SpawnEvent;
        [FoldoutGroup("Events")] public UnityEvent DespawnEvent;

        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private NavMeshCharacter _navMeshCharacter;
        private BehaviorGraphAgent _behaviourGraphAgent;

        private Material[] _fadeMaterials;

        public Spawner Spawner { get; set; }

        #endregion
        #region Startup

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshCharacter = GetComponent<NavMeshCharacter>();
            _behaviourGraphAgent = GetComponent<BehaviorGraphAgent>();

            _fadeMaterials = GetComponentInChildren<SkinnedMeshRenderer>(true).materials;
        }
        #endregion
        #region Class Methods

        public void PreSpawn()
        {
            DisableComponents();

            if (fadeIn)
            {
                SetMaterialsAlpha(0);
            }
        }

        public void Spawn()
        {
            if (fadeIn)
            {
                StartCoroutine(FadeMaterialAlpha(0, 1, fadeInTime, SpawnFadeComplete));
            }

            SpawnFadeComplete();
        }

        private void SpawnFadeComplete()
        {
            EnableComponents();
            SpawnEvent.Invoke();
        }

        public void Despawn()
        {
            if (fadeOut)
            {
                StartCoroutine(FadeMaterialAlpha(1, 0, fadeOutTime, DespawnFadeComplete));
                return;
            }
            DespawnEvent.Invoke();
        }

        private void DespawnFadeComplete()
        {
            DisableComponents();
            DespawnEvent.Invoke();
        }


        private void DisableComponents()
        {
            SetComponentsState(false);
        }

        private void EnableComponents()
        {
            SetComponentsState(true);
        }

        private void SetComponentsState(bool state)
        {
            _navMeshAgent.enabled = state;
            _navMeshCharacter.enabled = state;
            _behaviourGraphAgent.enabled = state;

        }

        private IEnumerator FadeMaterialAlpha(float start, float end, float fadeTime, Action onComplete)
        {
            float time = 0;

            while (time < fadeTime)
            {
                SetMaterialsAlpha(Mathf.Lerp(start, end, time / fadeTime));
                time += Time.deltaTime;
                yield return null;
            }

            SetMaterialsAlpha(end);
            onComplete.Invoke();
        }

        private void SetMaterialsAlpha(float alpha)
        {
            foreach (Material mat in _fadeMaterials)
            {
                Color fadedColor = mat.color;
                fadedColor.a = alpha;
                mat.color = fadedColor;
            }
        }

        #endregion
    }
}