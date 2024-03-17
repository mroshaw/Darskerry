using System;
using System.Collections.Generic;
using DaftAppleGames.Common.GameControllers;
#if ENVIRO_3
using Enviro;
#endif
#if INVECTOR_SHOOTER
using Invector;
using Invector.vCamera;
#endif
#if HAP
using MalbersAnimations.HAP;
#endif
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Debugger
{
    /// <summary>
    /// Manager class for Debugger functions
    /// </summary>
    public class DebugManager : MonoBehaviour
    {
        [Serializable]
        public class TimesOfDayPreset
        {
            public string PresetName;
            public int Hour;
            public int Minute;
            
            public TimesOfDayPreset(string presetName, int hour, int minute)
            {
                PresetName = presetName;
                Hour = hour;
                Minute = minute;
            }
        }
        
        [Header("Settings")]
        public bool enableDebug;
        public KeyCode debugKeyCode = KeyCode.D;
        public KeyCode debugKeyQualifier = KeyCode.LeftControl;

        [Header("Teleport Targets")]
        public GameObject teleportTargetsGameObject;

        [Header("Weather")]
        public List<string> weatherPresets;
        
        [Header("Time of Day")]
        public List<TimesOfDayPreset> timesOfDay;

        [FoldoutGroup("Control Events")]
        public UnityEvent onDebugEnabledEvent;
        [FoldoutGroup("Control Events")]
        public UnityEvent onDebugDisabledEvent;
        
        [FoldoutGroup("Teleport Events")]
        public UnityEvent<List<string>> onTeleportTargetsChangeEvent;

        [FoldoutGroup("Weather Preset Events")]
        public UnityEvent<List<string>> onWeathersChangeEvent;

        [FoldoutGroup("Time Preset Events")]
        public UnityEvent<List<string>> onTimesChangeEvent;
        
        private bool _isEnabled;
        
        private List<string> _teleportTargetNames;
        private List<Transform> _teleportTargets;
        private int _teleportTargetIndex = 0;

        private List<string> _timeOfDayNames;
        private int _timeOfDayIndex = 0;
        private int _hour = 0;
        private int _minute = 0;
        
        private List<string> _weatherNames;
        private int _weatherIndex = 0;
        
        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            InitTeleportTargets();
            InitWeatherNames();
            InitTimesNames();
        }
        
        /// <summary>
        /// Check for Debug keypress
        /// </summary>
        private void Update()
        {
            if (Input.GetKey(debugKeyQualifier) && Input.GetKeyDown(debugKeyCode))
            {
                if (!_isEnabled)
                {
                    EnableDebug();    
                }
                else
                {
                    DisableDebug();
                }
            }
        }
        
        /// <summary>
        /// Enable debugging features
        /// </summary>
        public void EnableDebug()
        {
            _isEnabled = true;
            onDebugEnabledEvent.Invoke();
        }

        /// <summary>
        /// Disable Debugging features
        /// </summary>
        public void DisableDebug()
        {
            _isEnabled = false;
            onDebugDisabledEvent.Invoke();
        }

        /// <summary>
        /// Initialise the available teleport targets
        /// </summary>
        public void InitTeleportTargets()
        {
            _teleportTargetNames = new List<string>();
            _teleportTargets = new List<Transform>();
            
            foreach (Transform target in teleportTargetsGameObject.transform)
            {
                string formattedName = target.gameObject.name;
                _teleportTargetNames.Add(formattedName);
                _teleportTargets.Add(target);
            }
            onTeleportTargetsChangeEvent.Invoke(_teleportTargetNames);
        }

        /// <summary>
        /// Populate available weather names
        /// </summary>
        private void InitWeatherNames()
        {
            _weatherNames = new List<string>();
            for (int count = 0; count < weatherPresets.Count; count++)
            {
                _weatherNames.Add(weatherPresets[count]);
            }
            onWeathersChangeEvent.Invoke(_weatherNames);
        }

        /// <summary>
        /// Populate available time presets
        /// </summary>
        private void InitTimesNames()
        {
            _timeOfDayNames = new List<string>();
            
            for (int count = 0; count < timesOfDay.Count; count++)
            {
                _timeOfDayNames.Add(timesOfDay[count].PresetName);
            }
            onTimesChangeEvent.Invoke(_timeOfDayNames);
        }
        
        /// <summary>
        /// Clears player prefs
        /// </summary>
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// Toggles invincibility
        /// </summary>
        public void ToggleInvincible(bool state)
        {
            Debug.Log($"Invincibility set to: {state}");
            GameObject player = PlayerCameraManager.Instance.PlayerGameObject;
#if INVECTOR_SHOOTER
            vHealthController health = player.GetComponent<vHealthController>();
            health.isImmortal = state;
#endif
        }
        
        /// <summary>
        /// Sets the teleport target index
        /// </summary>
        /// <param name="targetIndex"></param>
        public void SetTeleportTarget(int targetIndex)
        {
            _teleportTargetIndex = targetIndex;
        }

        /// <summary>
        /// Teleport player to specified target
        /// </summary>
        public void TeleportToTarget()
        {
            Transform teleportTargetTransform = _teleportTargets[_teleportTargetIndex];
            string teleportTargetName = _teleportTargetNames[_teleportTargetIndex];
            Debug.Log($"Teleporting to: {teleportTargetName} at Position {teleportTargetTransform.position}");

            GameObject player = PlayerCameraManager.Instance.PlayerGameObject;
            Camera camera = PlayerCameraManager.Instance.MainCamera;
#if INVECTOR_SHOOTER
            vThirdPersonCamera vCamera = PlayerCameraManager.Instance.InvectorMainCamera;
            vCamera.enabled = false;
#endif            
            #if HAP
            MRider rider = player.GetComponent<MRider>();
            if (rider && rider.IsRiding)
            {
                // If riding, teleport the horse
                player.transform.root.transform.position = teleportTargetTransform.position;
            }
            else
            {
                player.transform.position = teleportTargetTransform.position;
            }
            #endif
#if INVECTOR_SHOOTER
            vCamera.gameObject.transform.position = teleportTargetTransform.position;
            vCamera.enabled = true;
#endif
        }

        /// <summary>
        /// Sets the hour
        /// </summary>
        /// <param name="hour"></param>
        public void SetHour(int hour)
        {
            _hour = hour;
        }

        /// <summary>
        /// Sets the minute
        /// </summary>
        /// <param name="minute"></param>
        public void SetMinute(int minute)
        {
            _minute = minute;
        }

        /// <summary>
        /// Apply the hour and minute times
        /// </summary>
        public void ApplyTime()
        {
            #if ENVIRO_3
            Debug.Log($"Changing time to: {_hour}:{_minute}");
            Enviro.EnviroManager.instance.Time.hours = _hour;
            Enviro.EnviroManager.instance.Time.minutes = _minute;
            #endif
        }
        
        /// <summary>
        /// Set the selected time index
        /// </summary>
        /// <param name="timeIndex"></param>
        public void SetTimePreset(int timeIndex)
        {
            _timeOfDayIndex = timeIndex;
        }

        /// <summary>
        /// Change the time to preset
        /// </summary>
        public void ApplyTimePreset()
        {
            #if ENVIRO_3
            string timeName = timesOfDay[_timeOfDayIndex].PresetName;
            int hour = timesOfDay[_timeOfDayIndex].Hour;
            int minute = timesOfDay[_timeOfDayIndex].Minute;
            Debug.Log($"Changing time to: {timeName} ({hour}:{minute})");
            Enviro.EnviroManager.instance.Time.hours = hour;
            Enviro.EnviroManager.instance.Time.minutes = minute;
            #endif
        }

        /// <summary>
        /// Set the selected Weather index
        /// </summary>
        /// <param name="weatherIndex"></param>
        public void SetWeatherPreset(int weatherIndex)
        {
            _weatherIndex = weatherIndex;
        }

        /// <summary>
        /// Change the weather
        /// </summary>
        public void ApplyWeatherPreset()
        {
            #if ENVIRO_3
            string weatherName = weatherPresets[_weatherIndex];
            Debug.Log($"Changing weather to: {weatherName}");
            Enviro.EnviroManager.instance.Weather.ChangeWeather(weatherName);
            #endif
        }
    }
}
