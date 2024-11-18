using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    [Serializable]
    public struct PatrolParams
    {
        [BoxGroup("Patrol Settings")] [SerializeField] private Transform[] patrolPoints;
        [BoxGroup("Patrol Settings")][SerializeField] private float speed;
        [BoxGroup("Patrol Settings")][SerializeField] private float minPause;
        [BoxGroup("Patrol Settings")][SerializeField] private float maxPause;

        public float Speed => speed;
        public float MinPause => minPause;
        public float MaxPause => maxPause;
        public Transform[] PatrolPoints => patrolPoints;
        public int NumberOfPatrolPoints => patrolPoints.Length;
    }
}