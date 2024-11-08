using DaftAppleGames.Darskerry.Core.PlayerController;
using ECM2;
using System;
using System.Collections;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AiWander", story: "Wander within a minimum [minRange] and maximum [maxRange] range from transform [wanderTransform] at speed [movementSpeed] with minimum [minPauseDelay] and maximum [maxPauseDelay] pauses using an [AiBrain]", category: "Action", id: "79530ae655e4c01296bec8f3939d187e")]
public partial class AiWanderAction : Action
{
    [SerializeReference] public BlackboardVariable<float> MinRange;
    [SerializeReference] public BlackboardVariable<float> MaxRange;
    [SerializeReference] public BlackboardVariable<Transform> WanderTransform;
    [SerializeReference] public BlackboardVariable<float> MovementSpeed;
    [SerializeReference] public BlackboardVariable<float> MinPauseDelay;
    [SerializeReference] public BlackboardVariable<float> MaxPauseDelay;
    [SerializeReference] public BlackboardVariable<AiBrain> AiBrain;
    private AiBrain _aiBrain;
    private NavMeshCharacter _navMeshCharacter;
    private Vector3 _wanderTransformCenter;

    private bool _hasDestination = true;

    protected override Status OnStart()
    {
        if (AiBrain.Value == null)
        {
            LogFailure("No Ai Brain assigned.");
            return Status.Failure;
        }

        if (MinRange.Value == 0.0f || MaxRange.Value == 0.0f || MovementSpeed.Value == 0.0f)
        {
            LogFailure("Minimum and Maximum Range and Speed must all be non-zero.");
            return Status.Failure;
        }

        if (MinPauseDelay > MaxPauseDelay)
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
        _aiBrain = AiBrain.Value.GetComponent<AiBrain>();
        _navMeshCharacter = _aiBrain.NavMeshCharacter;
        _wanderTransformCenter = WanderTransform.Value != null ? WanderTransform.Value.position : _aiBrain.transform.position;
        _navMeshCharacter.DestinationReached += ArrivedAtDestination;
    }

    private void GoToNextDestination()
    {
        _aiBrain.SetMoveSpeed(MovementSpeed.Value);
        Vector3 wanderLocation = GetRandomWanderLocation(_wanderTransformCenter, MinRange.Value, MaxRange.Value);
        Debug.Log($"AiWanderAction: Moving to new target: {wanderLocation}");
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
        yield return new WaitForSeconds(UnityEngine.Random.Range(MinPauseDelay, MaxPauseDelay));
        GoToNextDestination();
    }

    private void ArrivedAtDestination()
    {
        _hasDestination = false;
    }
}

