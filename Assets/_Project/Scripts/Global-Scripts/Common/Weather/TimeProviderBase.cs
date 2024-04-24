using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Weather
{
    public abstract class TimeProviderBase : TimeAndWeatherProviderBase
    {
        [BoxGroup("Events")] public int dayStartTime = 6;
        [BoxGroup("Events")] public int nightStartTime = 20;
        [BoxGroup("Events")] public UnityEvent <int>onHourPassedEvent;
        [BoxGroup("Events")] public UnityEvent <int>onMinutePassedEvent;
        [BoxGroup("Events")] public UnityEvent onGotoHourStartEvent;
        [BoxGroup("Events")] public UnityEvent onGotoHourEndEventEvent;
        [BoxGroup("Events")] public UnityEvent<TimeOfDayPresetSettingsBase> onTimePresetAppliedEvent;
        [BoxGroup("Events")] public UnityEvent onDayStartedEvent;
        [BoxGroup("Events")] public UnityEvent onNightStartedEvent;
        private int _currHour;
        private int _currMinute;

        /// <summary>
        /// Move to the given hour
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="transitionDuration"></param>
        public void GotoTime(int hour, int minute, float transitionDuration)
        {
            StartCoroutine(GotoTimeAsync(hour, minute, transitionDuration));
        }

        /// <summary>
        /// Sets time to the given hour over time
        /// </summary>
        /// <param name="targetHour"></param>
        /// <param name="targetMinute"></param>
        /// <param name="transitionDuration"></param>
        /// <returns></returns>
        private IEnumerator GotoTimeAsync(int targetHour, int targetMinute, float transitionDuration)
        {
            float timeElapsed = 0;
            int startHour = _currHour;
            int startMinutes = _currMinute;

            Debug.Log($"SetHourAsync: targetHour is {targetHour}, startHour is {startHour}");
            int initialMinutes = (startHour * 60) + startMinutes;
            int targetMinutes;
            if (targetHour <= startHour)
            {
                targetMinutes = (targetHour+24) * 60 + targetMinute;
            }
            else
            {
                targetMinutes = targetHour * 60 + targetMinute;
            }

            Debug.Log($"SetHourAsync: initialMinutes is {initialMinutes}, targetMinutes is {targetMinutes}");

            while (timeElapsed < transitionDuration)
            {
                int currentMinutes = (int)Mathf.Lerp(initialMinutes, (float)targetMinutes, timeElapsed / transitionDuration);
                int  newHours = currentMinutes / 60;
                int newMinutes = currentMinutes - (newHours * 60);

                // Cater for going past midnight
                if (newHours > 23)
                {
                    newHours -= 24;
                }

                // Update the time of day
                Hour = newHours;
                Minute = newMinutes;

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Set the final time of day
            Hour = targetHour;
            Minute = targetMinute;
        }

        /// <summary>
        /// Abstract hour property
        /// </summary>
        public virtual int Hour {
            get
            {
                _currHour = HourProvider;
                return _currHour;
            }
            set
            {
                if (value != _currHour)
                {
                    Debug.Log("An hour has passed...");
                    onHourPassedEvent.Invoke(value);

                    if (value == dayStartTime)
                    {
                        Debug.Log("Day-time has started...");
                        onDayStartedEvent.Invoke();
                    }

                    if (value == nightStartTime)
                    {
                        Debug.Log("Night-time has started...");
                        onNightStartedEvent.Invoke();
                    }
                }

                _currHour = value;
                HourProvider = _currHour;
            }
        }

        /// <summary>
        /// Abstract minute property
        /// </summary>
        public virtual int Minute {
            get
            {
                _currMinute = MinuteProvider;
                return _currMinute;
            }
            set
            {
                if (value != _currMinute)
                {
                    onMinutePassedEvent.Invoke(value);
                }
                _currMinute = value;
                MinuteProvider = _currMinute;
            }
        }

        public abstract int HourProvider { get; set; }

        public abstract int MinuteProvider { get; set; }

        /// <summary>
        /// Applies the given time of day preset
        /// </summary>
        /// <param name="timePreset"></param>
        /// <param name="transitionDuration"></param>
        public void ApplyTimePreset(TimeOfDayPresetSettingsBase timePreset, float transitionDuration)
        {
            ApplyTimePresetProvider(timePreset, transitionDuration);
            if (transitionDuration == 0)
            {
                Hour = timePreset.hour;
                Minute = timePreset.minute;
            }
            else
            {
                GotoTime(timePreset.hour, timePreset.minute, transitionDuration);
            }
            onTimePresetAppliedEvent.Invoke(timePreset);
        }

        /// <summary>
        /// Abstract provider implementation of ApplyTimePreset
        /// </summary>
        /// <param name="timePreset"></param>
        /// <param name="transitionDuration"></param>
        public abstract void ApplyTimePresetProvider(TimeOfDayPresetSettingsBase timePreset, float transitionDuration);
    }
}