using ECM2;
using System;
using System.Collections;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Wander", story: "Wander using an [AiBrain]", category: "Action/Navigation", id: "79530ae655e4c01296bec8f3939d187e")]
    public partial class WanderAction : Action
    {
        [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;
        private AiBrain _aiBrain;
        private NavMeshCharacter _navMeshCharacter;
        private Vector3 _wanderTransformCenter;

        private bool _hasDestination = true;

        protected override Status OnStart()
        {
            _aiBrain = AiBrain.Value;
            if (_aiBrain == null)
            {
                LogFailure("No Ai Brain assigned.");
                return Status.Failure;
            }

            if (_aiBrain.WanderParams.MinRange == 0.0f || _aiBrain.WanderParams.MaxRange == 0.0f || _aiBrain.WanderParams.Speed == 0.0f)
            {
                LogFailure("Minimum and Maximum Range and Speed must all be non-zero.");
                return Status.Failure;
            }

            if (_aiBrain.WanderParams.MinPause > _aiBrain.WanderParams.MaxPause)
            {
                LogFailure("Minimum Pause Delay is greater than Maximum Pause Delay.");
                return Status.Failure;
            }

            Initialize();
            _hasDestination = false;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (AiBrain.Value == null)
            {
                return Status.Failure;
            }

            if (!_hasDestination)
            {
                _hasDestination = true;
                _aiBrain.StartCoroutine(MoveToNextAfterDelayAsync());
            }
            return Status.Running;
        }

        protected override void OnEnd()
        {
        }

        private void Initialize()
        {
            _navMeshCharacter = _aiBrain.NavMeshCharacter;
            _wanderTransformCenter = _aiBrain.WanderParams.CenterTransform != null ? _aiBrain.WanderParams.CenterTransform.position : _aiBrain.transform.position;
            _navMeshCharacter.DestinationReached += ArrivedAtDestination;
        }

        private void GoToNextDestination()
        {
            _aiBrain.SetMoveSpeed(_aiBrain.WanderParams.Speed);
            Vector3 wanderLocation = GetRandomWanderLocation(_wanderTransformCenter, _aiBrain.WanderParams.MinRange, _aiBrain.WanderParams.MaxRange);
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
            yield return new WaitForSeconds(UnityEngine.Random.Range(_aiBrain.WanderParams.MinPause, _aiBrain.WanderParams.MaxPause));
            GoToNextDestination();
        }

        private void ArrivedAtDestination()
        {
            _hasDestination = false;
        }
    }
}