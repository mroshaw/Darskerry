using UnityEngine;

namespace DaftAppleGames.Common.Weather
{
    /// <summary>
    /// Base class for the Rain provider, used by the Weather Sync Manager
    /// </summary>
    public abstract class BaseWeatherRainProvider : MonoBehaviour, IWeatherRainProvider
    {
        public virtual float GetRainLevel()
        {
            return 0.0f;
        }
    }
}