using System.Collections;
using DaftAppleGames.Darskerry.Core.Settings;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace DaftAppleGames.Darskerry.Core.TimeAndWeather
{
    [ExecuteInEditMode]
    public class TimeAndWeatherController : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Time")] [SerializeField] private TimeOfDay timeOfDay;
        [BoxGroup("Time")] [SerializeField] private float startingHour = 20.9f;
        [BoxGroup("Weather")] [SerializeField] private VolumeFader fogVolumeFader;
        [BoxGroup("Weather")] [SerializeField] private VolumeFader cloudVolumeFader;
        [BoxGroup("Weather")] [SerializeField] private WeatherPresets startingWeatherPreset;
        [BoxGroup("Debug")] [SerializeField] private float sceneHour = 6.0f;
        #endregion

        #region Startup
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            if (Application.isPlaying)
            {
                timeOfDay.timeOfDay = startingHour;
            }
        }

        #endregion

        #region Class methods


        #endregion

        #region Editor Methods

        [BoxGroup("Time Debug")][Button("Set Scene Time")]
        private void SetSceneToDebug()
        {
            SetSceneTime(sceneHour);
        }

        [BoxGroup("Time Debug")][Button("Dawn")]
        private void SetDawn()
        {
            SetSceneTime(5.5f);
        }


        [BoxGroup("Time Debug")][Button("Morning")]
        private void SetMorning()
        {
            SetSceneTime(8.0f);

        }

        [BoxGroup("Time Debug")][Button("Afternoon")]
        private void SetAfternoon()
        {
            SetSceneTime(14.0f);

        }
        [BoxGroup("Time Debug")][Button("Evening")]
        private void SetEvening()
        {
            SetSceneTime(17.0f);

        }
        [BoxGroup("Time Debug")][Button("Dusk")]
        private void SetDusk()
        {
            SetSceneTime(19.50f);

        }

        [BoxGroup("Time Debug")][Button("Night")]
        private void SetNight()
        {
            SetSceneTime(21.0f);

        }

        private void SetSceneTime(float hour)
        {
            timeOfDay.timeOfDay = hour;
        }
        #endregion
    }
}