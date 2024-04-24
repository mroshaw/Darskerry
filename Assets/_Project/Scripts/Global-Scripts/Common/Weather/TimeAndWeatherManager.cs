using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Weather
{
    public enum WeatherSeason { Spring, Summer, Autumn, Winter }

    [ExecuteInEditMode]
    public class TimeAndWeatherManager : MonoBehaviour
    {
        // Singleton static instance
        private static TimeAndWeatherManager _instance;
        public static TimeAndWeatherManager Instance => _instance;

        /// <summary>
        /// Hour public property
        /// </summary>
        [ShowInInspector] [BoxGroup("Time Of Day")] [PropertyRange(0, 23)] [PropertyOrder(-1)] public int Hour {
            get =>
                !timeProvider ?0 :timeProvider.Hour;
            set
            {
                if(timeProvider) timeProvider.Hour = value;
            }
        }

        /// <summary>
        /// Minute public property
        /// </summary>
        [ShowInInspector] [BoxGroup("Time Of Day")] [PropertyRange(0, 59)] [PropertyOrder(-1)] public int Minute {
            get =>
                !timeProvider ?0 :timeProvider.Minute;
            set
            {
                if(timeProvider) timeProvider.Minute = value;
            }
        }
        [BoxGroup("Time Of Day")] public TimeOfDayPresetSettingsBase defaultTimeOfDayPreset;
        [BoxGroup("Time Of Day")] public List<TimeOfDayPresetSettingsBase> timeOfDayPresets;
        [BoxGroup("Weather")] public WeatherPresetSettingsBase defaultWeatherPreset;
        [BoxGroup("Weather")] public List<WeatherPresetSettingsBase> weatherPresets;

        public TimeProviderBase timeProvider;
        public WeatherProviderBase weatherProvider;

        /// <summary>
        /// Initialise the provider components
        /// </summary>
        private void OnEnable()
        {
            timeProvider = GetComponent<TimeProviderBase>();
            weatherProvider = GetComponent<WeatherProviderBase>();
        }

        /// <summary>
        /// Initialise the TimeAndWeatherManager Singleton instance
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        /// <summary>
        /// Set up component and apply and defaults
        /// </summary>
        private void Start()
        {
            // Set time defaults
            timeProvider.ApplyTimePreset(defaultTimeOfDayPreset, 0.1f);

            // Start the default weather
            weatherProvider.ApplyWeatherPreset(defaultWeatherPreset, 1.0f);
        }

        /// <summary>
        /// Apply a given preset based on name
        /// </summary>
        /// <param name="timePresetName"></param>
        /// <param name="transitionDuration"></param>
        public void ApplyTimePreset(string timePresetName, float transitionDuration)
        {
            ApplyTimePreset(GetTimePresetByName(timePresetName), transitionDuration);
        }

        /// <summary>
        /// Apply a given time preset
        /// </summary>
        /// <param name="timePreset"></param>
        /// <param name="transitionDuration"></param>
        public void ApplyTimePreset(TimeOfDayPresetSettingsBase timePreset, float transitionDuration)
        {
            timeProvider.ApplyTimePreset(timePreset, transitionDuration);
        }

        /// <summary>
        /// Apply a given weather preset by name
        /// </summary>
        /// <param name="weatherPresetName"></param>
        /// <param name="transitionDuration"></param>
        public void ApplyWeatherPreset(string weatherPresetName, float transitionDuration)
        {
            ApplyWeatherPreset(GetWeatherPresetByName(weatherPresetName), transitionDuration);
        }

        /// <summary>
        /// Apply the given weather preset
        /// </summary>
        /// <param name="weatherPreset"></param>
        /// <param name="transitionDuration"></param>
        public void ApplyWeatherPreset(WeatherPresetSettingsBase weatherPreset, float transitionDuration)
        {
            weatherProvider.ApplyWeatherPreset(weatherPreset, transitionDuration);
        }

        /// <summary>
        /// Return a list of names of registered time presets
        /// </summary>
        /// <returns></returns>
        public List<string> GetTimePresetNames()
        {
            List<string> presetNames = new List<string>();
            foreach (TimeOfDayPresetSettingsBase currSettings in timeOfDayPresets)
            {
                presetNames.Add(currSettings.presetName);
            }
            return presetNames;
        }

        /// <summary>
        /// Return a list of names of registered weather presets
        /// </summary>
        /// <returns></returns>
        public List<string> GetWeatherPresetNames()
        {
            List<string> presetNames = new List<string>();
            foreach (WeatherPresetSettingsBase currSettings in weatherPresets)
            {
                presetNames.Add(currSettings.presetName);
            }
            return presetNames;
        }

        /// <summary>
        /// Look up a weather preset by name
        /// </summary>
        /// <param name="weatherPresetName"></param>
        /// <returns></returns>
        private WeatherPresetSettingsBase GetWeatherPresetByName(string weatherPresetName)
        {
            foreach (WeatherPresetSettingsBase currSettings in weatherPresets)
            {
                if (currSettings.presetName == weatherPresetName)
                {
                    return currSettings;
                }
            }
            return null;
        }

        /// <summary>
        /// Look up a time of day preset by name
        /// </summary>
        /// <param name="timePresetName"></param>
        /// <returns></returns>
        private TimeOfDayPresetSettingsBase GetTimePresetByName(string timePresetName)
        {
            foreach (TimeOfDayPresetSettingsBase currSettings in timeOfDayPresets)
            {
                if (currSettings.presetName == timePresetName)
                {
                    return currSettings;
                }
            }
            return null;
        }
    }
}