using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    [Serializable]
    public struct WanderParams
    {
        [BoxGroup("Wander Settings")][SerializeField] private float speed;
        [BoxGroup("Wander Settings")][SerializeField] private float minRange;
        [BoxGroup("Wander Settings")][SerializeField] private float maxRange;
        [BoxGroup("Wander Settings")][SerializeField] private float minPause;
        [BoxGroup("Wander Settings")][SerializeField] private float maxPause;
        [BoxGroup("Wander Settings")][SerializeField] private Transform centerTransform;

        public float Speed => speed;
        public float MinRange => minRange;
        public float MaxRange => maxRange;
        public float MinPause => minPause;
        public float MaxPause => maxPause;
        public Transform CenterTransform => centerTransform;

    }
}
