using DaftAppleGames.Darskerry.TimeAndWeather.TimeOfDay;
using DaftAppleGames.Darskerry.TimeAndWeather.Weather;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.TimeAndWeather
{
    public class TimeAndWeatherEvents : MonoBehaviour
    {
        [FoldoutGroup("Time Events")] public UnityEvent onDayStartedEvent;
        [FoldoutGroup("Time Events")] public UnityEvent onNightStartedEvent;
        [FoldoutGroup("Time Events")] public UnityEvent <int>onHourPassedEvent;
        [FoldoutGroup("Time Events")] public UnityEvent <int>onMinutePassedEvent;
        [FoldoutGroup("Time Events")] public UnityEvent<TimeOfDayPresetSettingsBase> onTimePresetAppliedEvent;

        [FoldoutGroup("Weather Events")] public UnityEvent<WeatherPresetSettingsBase> onWeatherPresetAppliedEvent;

        /// <summary>
        /// Register events with the TimeAndWeather Manager
        /// </summary>
        private void OnEnable()
        {

            // Time events
            TimeAndWeatherManager.Instance.onDayTimeStartedEvent.AddListener(OnDayStartedEventProxy);
            TimeAndWeatherManager.Instance.onNightTimeStartedEvent.AddListener(OnNightStartedEventProxy);
            TimeAndWeatherManager.Instance.onHourPassedEvent.AddListener(OnHourPassedEventProxy);
            TimeAndWeatherManager.Instance.onMinutePassedEvent.AddListener(OnMinutePassedEventProxy);
            TimeAndWeatherManager.Instance.onTimePresetAppliedEvent.AddListener(OnTimePresetAppliedEventProxy);

            // Weather events
            TimeAndWeatherManager.Instance.onWeatherPresetAppliedEvent.AddListener(OnWeatherPresetAppliedEventProxy);

        }

        /// <summary>
        /// Unregister events from the TimeAndWeather Manager
        /// </summary>
        private void OnDisable()
        {
            // Time events
            TimeAndWeatherManager.Instance.onDayTimeStartedEvent.RemoveListener(OnDayStartedEventProxy);
            TimeAndWeatherManager.Instance.onNightTimeStartedEvent.RemoveListener(OnNightStartedEventProxy);
            TimeAndWeatherManager.Instance.onHourPassedEvent.RemoveListener(OnHourPassedEventProxy);
            TimeAndWeatherManager.Instance.onMinutePassedEvent.RemoveListener(OnMinutePassedEventProxy);
            TimeAndWeatherManager.Instance.onTimePresetAppliedEvent.RemoveListener(OnTimePresetAppliedEventProxy);

            // Weather events
            TimeAndWeatherManager.Instance.onWeatherPresetAppliedEvent.RemoveListener(OnWeatherPresetAppliedEventProxy);
        }

        /// <summary>
        /// Proxy event handler
        /// </summary>
        private void OnDayStartedEventProxy()
        {
            onDayStartedEvent.Invoke();
        }

        /// <summary>
        /// Proxy event handler
        /// </summary>
        private void OnNightStartedEventProxy()
        {
            onNightStartedEvent.Invoke();
        }

        /// <summary>
        /// Proxy event handler
        /// </summary>
        private void OnHourPassedEventProxy(int hour)
        {
            onHourPassedEvent.Invoke(hour);
        }

        /// <summary>
        /// Proxy event handler
        /// </summary>
        private void OnMinutePassedEventProxy(int minute)
        {
            onMinutePassedEvent.Invoke(minute);
        }

        /// <summary>
        /// Proxy event handler
        /// </summary>
        /// <param name="timeSettings"></param>
        private void OnTimePresetAppliedEventProxy(TimeOfDayPresetSettingsBase timeSettings)
        {
            onTimePresetAppliedEvent.Invoke(timeSettings);
        }

        /// <summary>
        /// Proxy event handler
        /// </summary>
        /// <param name="weatherSettings"></param>
        private void OnWeatherPresetAppliedEventProxy(WeatherPresetSettingsBase weatherSettings)
        {
            onWeatherPresetAppliedEvent.Invoke(weatherSettings);
        }
    }
}