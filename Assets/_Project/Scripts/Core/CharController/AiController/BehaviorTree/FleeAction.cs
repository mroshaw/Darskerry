using System;
using ECM2;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.BehaviourTree
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Flee", story: "Flee from [Target] using [AiBrain]", category: "Action",
        id: "d9102898acb44b193285ab3326751e54")]
    public partial class FleeAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;

        private float _fleeDistance = 10.0f;
        private float _fleeSpeed = 1.0f;

        private AiBrain _aiBrain;
        private NavMeshCharacter _navMeshCharacter;

        private bool _hasArrived;

        protected override Status OnStart()
        {
            _aiBrain = AiBrain.Value;
            if (_aiBrain == null)
            {
                LogFailure("No Ai Brain assigned.");
                return Status.Failure;
            }

            if (Target.Value == null)
            {
                return Status.Success;
            }

            _hasArrived = false;
            _navMeshCharacter = _aiBrain.NavMeshCharacter;
            _navMeshCharacter.DestinationReached += ArrivedAtDestination;
            _aiBrain.SetMoveSpeed(_fleeSpeed);
            _navMeshCharacter.MoveToDestination(GetFleeDestination());

            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return !_hasArrived ? Status.Running : Status.Success;
        }

        protected override void OnEnd()
        {
        }

        private Vector3 GetFleeDestination()
        {
            Vector3 randomPosition = (Target.Value.position - _aiBrain.transform.position).normalized * _fleeDistance;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit myNavHit, 100, -1))
            {
                return myNavHit.position;
            }
            return randomPosition;
        }

        private void ArrivedAtDestination()
        {
            _hasArrived = true;
        }
    }
}