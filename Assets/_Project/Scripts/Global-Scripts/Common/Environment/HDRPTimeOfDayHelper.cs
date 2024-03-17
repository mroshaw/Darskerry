#if ASMDEF
#if HDRPTIMEOFDAY_
using UnityEngine;
using UnityEngine.Events;
using ProceduralWorlds.HDRPTOD;

namespace DaftAppleGames.Common.Environment
{
    public class HDRPTimeOfDayHelper : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent OnHourPassed;
        public UnityEvent OnDay;
        public UnityEvent OnNight;
        public UnityEvent OnStartSnowing;
        public UnityEvent OnStopSnowing;
        public UnityEvent OnStartRaining;
        public UnityEvent OnStopRaining;

        public int currentHour;

        private int trackedHour;
        private bool trackedIsSnowing;
        private bool trackedIsRaining;
        private bool trackedIsDay;

        private int snowWeatherId;
        private int rainWeatherId;

        // Start is called before the first frame update
        private void Start()
        {
            trackedHour = (int)HDRPTimeOfDayAPI.GetTimeOfDay().TimeOfDay;
            trackedIsDay = HDRPTimeOfDayAPI.GetTimeOfDay().IsDayTime();
            currentHour = trackedHour;
            int curretWeatherId = HDRPTimeOfDayAPI.GetTimeOfDay().m_selectedActiveWeatherProfile;
            snowWeatherId = HDRPTimeOfDayAPI.GetWeatherIDByName("Snow");
            rainWeatherId = HDRPTimeOfDayAPI.GetWeatherIDByName("Rain");

            if (curretWeatherId == snowWeatherId)
            {
                trackedIsSnowing = true;
                OnStartSnowing.Invoke();
            }
            else
            {
                trackedIsSnowing = false;
            }

            if (curretWeatherId == rainWeatherId)
            {
                trackedIsRaining = true;
                OnStartRaining.Invoke();
            }
            else
            {
                trackedIsRaining = false;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            // Check the hour
            int currentHour = (int)HDRPTimeOfDayAPI.GetTimeOfDay().TimeOfDay;
            if(currentHour != trackedHour)
            {
                trackedHour = currentHour;
                currentHour = trackedHour;
                Debug.Log($"TimeOfDayHelper: Hour Passed ({currentHour}");
                OnHourPassed.Invoke();
            }

            // Check the time of day
            bool currIsDay = HDRPTimeOfDayAPI.GetTimeOfDay().IsDayTime();
            if(currIsDay != trackedIsDay)
            {
                trackedIsDay = currIsDay;
                if(trackedIsDay)
                {
                    OnDay.Invoke();
                }
                else
                {
                    OnNight.Invoke();
                }
            }

            // Check if snowing
            int curretWeatherId = HDRPTimeOfDayAPI.GetTimeOfDay().m_selectedActiveWeatherProfile;
            bool isCurrentSnowing = curretWeatherId == snowWeatherId;
            if(!HDRPTimeOfDayAPI.WeatherActive())
            {
                isCurrentSnowing = false;
            }
            if (isCurrentSnowing != trackedIsSnowing)
            {
                trackedIsSnowing = isCurrentSnowing;
                if(trackedIsSnowing)
                {
                    Debug.Log("TimeOfDayHelper: Started Snowing");
                    OnStartSnowing.Invoke();
                }
                else
                {
                    Debug.Log("TimeOfDayHelper: Stopped Snowing");
                    OnStopSnowing.Invoke();
                }
            }

            // Check if raining
            bool isCurrentRaining = curretWeatherId == rainWeatherId;
            if (!HDRPTimeOfDayAPI.WeatherActive())
            {
                isCurrentRaining = false;
            }
            if (isCurrentRaining != trackedIsRaining)
            {
                trackedIsRaining = isCurrentRaining;
                if (trackedIsRaining)
                {
                    Debug.Log("TimeOfDayHelper: Started Raining");
                    OnStartRaining.Invoke();
                }
                else
                {
                    Debug.Log("TimeOfDayHelper: Stopped Raining");
                    OnStopRaining.Invoke();
                }
            }
       }
    }
}
#endif
#endif