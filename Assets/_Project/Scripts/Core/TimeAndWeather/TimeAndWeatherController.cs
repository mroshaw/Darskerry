using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.TimeAndWeather
{
    [ExecuteInEditMode]
    public class TimeAndWeatherController : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Time")] [SerializeField] private TimeOfDay timeOfDay;
        [BoxGroup("Time")] [SerializeField] private float startingHour = 20.9f;
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

        #region Editor Methods

        [BoxGroup("Debug")][Button("Set Scene Time")]
        private void SetSceneToDebug()
        {
            SetSceneTime(sceneHour);
        }

        [BoxGroup("Debug")][Button("Dawn")]
        private void SetDawn()
        {
            SetSceneTime(5.5f);
        }


        [BoxGroup("Debug")][Button("Morning")]
        private void SetMorning()
        {
            SetSceneTime(8.0f);

        }

        [BoxGroup("Debug")][Button("Afternoon")]
        private void SetAfternoon()
        {
            SetSceneTime(14.0f);

        }
        [BoxGroup("Debug")][Button("Evening")]
        private void SetEvening()
        {
            SetSceneTime(17.0f);

        }
        [BoxGroup("Debug")][Button("Dusk")]
        private void SetDusk()
        {
            SetSceneTime(19.50f);

        }

        [BoxGroup("Debug")][Button("Night")]
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