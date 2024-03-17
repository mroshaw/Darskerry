using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    public class CandleLight : InteriorLight
    {
        [Header("Candle Configuration")]
        public List<ParticleSystem> flames = new();

        private const float candleRadius = 0.025f;
        private const float candleRange = 15.0f;
        private const float candleIntensity = 120.0f;

        /// <summary>
        /// Initialise the Candle Light
        /// </summary>
        public override void Start()
        {
            radius = candleRadius;
            range = candleRange;
            intensity = candleIntensity;
            base.Start();
        }

        /// <summary>
        /// Turn off Candle Light and Particle effects
        /// </summary>
        public override void TurnOffLight()
        {
            foreach(ParticleSystem flame in flames)
            {
                flame.Stop();
            }
            base.TurnOffLight();
        }

        /// <summary>
        /// Turn on Candle Light and Particle effects
        /// </summary>
        public override void TurnOnLight()
        {
            foreach (ParticleSystem flame in flames)
            {
                flame.Play();
            }
            base.TurnOnLight();
        }
    }
}