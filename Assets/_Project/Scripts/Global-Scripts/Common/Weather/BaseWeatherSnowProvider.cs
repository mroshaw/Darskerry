using UnityEngine;

namespace DaftAppleGames.Common.Weather
{
    /// <summary>
    /// Base class for the Snow provider, used by the Weather Sync Manager
    /// </summary>
    public abstract class BaseWeatherSnowProvider : MonoBehaviour, IWeatherSnowProvider
    {
        public abstract float GetSnowLevel();
    }
}