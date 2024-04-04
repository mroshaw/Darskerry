using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Environment
{

    public enum WeatherType { Clear, Cloudy, Rainy, RainStorm, LightSnow, SnowStorm }

    public class TimeAndWeatherManager : MonoBehaviour
    {
        [BoxGroup("Time of Day")] public int TimeMinutes;
        [BoxGroup("Time of Day")] public int TimeHours;
        [BoxGroup("Weather")] public WeatherType startingWeatherType;

        [FoldoutGroup("Time Events")] public UnityEvent<int>OnHourChangedEvent;
        [FoldoutGroup("Time Events")] public UnityEvent<int>OnMinuteChangedEvent;
        [FoldoutGroup("Time Events")] public UnityEvent DayToNightEvent;
        [FoldoutGroup("Time Events")] public UnityEvent NightToDayEvent;

        [FoldoutGroup("Weather Events")] public UnityEvent<WeatherType> OnWeatherChangedEvent;

        private WeatherType _currentWeatherType;
        private WeatherType _nextWeatherType;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}