using UnityEngine;

namespace DaftAppleGames.Common.TimeAndWeather
{
    public abstract class TimeAndWeatherProviderBase : MonoBehaviour
    {
        public TimeAndWeatherManager TimeAndWeatherManager { get; set; }
    }
}