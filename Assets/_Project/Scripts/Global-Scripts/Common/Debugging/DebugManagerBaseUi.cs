using System.Collections.Generic;
using DaftAppleGames.Common.Ui;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Settings
{
    public class DebugManagerBaseUi : BaseUiWindow
    {
        [Header("Ui Controls")]
        public TMP_Dropdown teleportTargetDropdown;
        public TMP_Dropdown weatherPresetDropdown;
        public TMP_Dropdown timePresetDropdown;
        
        [FoldoutGroup("Tools Events")]
        public UnityEvent onClearPlayerPrefsButtonClickedEvent;
        
        [FoldoutGroup("Cheats Events")]
        public UnityEvent<bool> onInvincibleToggleChangedEvent;

        [FoldoutGroup("Teleport Events")]
        public UnityEvent onTeleportButtonClickedEvent;
        [FoldoutGroup("Teleport Events")]
        public UnityEvent<int> onTeleportTargetChangedEvent;

        [FoldoutGroup("Weather Events")]
        public UnityEvent<int> onWeatherChangedEvent;
        [FoldoutGroup("Weather Events")]
        public UnityEvent onApplyWeatherClickEvent;
        
        [FoldoutGroup("Time of Day Events")]
        public UnityEvent<int> onTimeOfDayChangedEvent;
        [FoldoutGroup("Time of Day Events")]
        public UnityEvent<int> onHourChangedEvent;
        [FoldoutGroup("Time of Day Events")]
        public UnityEvent<int> onMinuteChangedEvent;
        [FoldoutGroup("Time of Day Events")]
        public UnityEvent onApplyTimePresetButtonClickedEvent;
        [FoldoutGroup("Time of Day Events")]
        public UnityEvent onApplyTimeButtonClickedEvent;
        
        [FoldoutGroup("Ui Events")]
        public UnityEvent onBackButtonClickEvent;
        
        /// <summary>
        /// Initialise the Settings Component
        /// </summary>
        public override void Start()
        {
            InitControls();
            base.Start();
        }

        /// <summary>
        /// Configure the UI control handlers to call public methods
        /// </summary>
        private void InitControls()
        {
        }

        /// <summary>
        /// Populates the teleport targets dropdown
        /// </summary>
        /// <param name="teleportTargetNames"></param>
        public void PopulateTeleportTargets(List<string> teleportTargetNames)
        {
            teleportTargetDropdown.ClearOptions();
            for (int count = 0; count < teleportTargetNames.Count; count++)
            {
                teleportTargetDropdown.options.Add(new TMP_Dropdown.OptionData(teleportTargetNames[count]));
            }
        }

        /// <summary>
        /// Populate Weather preset dropdown
        /// </summary>
        /// <param name="weatherPresets"></param>
        public void PopulateWeatherPresets(List<string> weatherPresets)
        {
            weatherPresetDropdown.ClearOptions();
            for (int count = 0; count < weatherPresets.Count; count++)
            {
                weatherPresetDropdown.options.Add(new TMP_Dropdown.OptionData(weatherPresets[count]));
            }
        }
        
        /// <summary>
        /// Populate time presets dropdown
        /// </summary>
        /// <param name="timePresets"></param>
        public void PopulateTimePresets(List<string> timePresets)
        {
            timePresetDropdown.ClearOptions();
            for (int count = 0; count < timePresets.Count; count++)
            {
                timePresetDropdown.options.Add(new TMP_Dropdown.OptionData(timePresets[count]));
            }
        }
        
        /// <summary>
        /// Proxy method invoking the Clear Prefs button
        /// </summary>
        public void ClearPrefsButtonProxy()
        {
            onClearPlayerPrefsButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Proxy method invoking the invincible toggle
        /// </summary>
        public void InvincibleToggleProxy(bool state)
        {
            onInvincibleToggleChangedEvent.Invoke(state);
        }

        /// <summary>
        /// Proxy method invoking the teleport button
        /// </summary>
        public void TeleportButtonProxy()
        {
            onTeleportButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Proxy method invoking the teleport target dropdown
        /// </summary>
        public void TeleportTargetProxy(int index)
        {
            onTeleportTargetChangedEvent.Invoke(index);
        }

        /// <summary>
        /// Proxy method for Weather preset dropdown
        /// </summary>
        /// <param name="index"></param>
        public void WeatherPresetProxy(int index)
        {  
            onWeatherChangedEvent.Invoke(index);
        }

        /// <summary>
        /// Proxy method for apply weather button
        /// </summary>
        public void WeatherButtonProxy()
        {
            onApplyWeatherClickEvent.Invoke();
        }

        /// <summary>
        /// Proxy for time preset changed
        /// </summary>
        /// <param name="index"></param>
        public void TimePresetProxy(int index)
        {
            onTimeOfDayChangedEvent.Invoke(index);
        }

        /// <summary>
        /// Proxy for apply time preset button
        /// </summary>
        public void TimePresetButtonProxy()
        {
            onApplyTimePresetButtonClickedEvent.Invoke();
        }
        
        /// <summary>
        /// Proxy for hour slider
        /// </summary>
        /// <param name="hour"></param>
        public void HourChangedProxy(float hour)
        {
            onHourChangedEvent.Invoke((int)hour);
        }

        /// <summary>
        /// Proxy for minute slider
        /// </summary>
        /// <param name="minute"></param>
        public void MinuteChangedProxy(float minute)
        {
            onMinuteChangedEvent.Invoke((int)minute);
        }

        /// <summary>
        /// Proxy for apply time button
        /// </summary>
        public void ApplyTimeButtonProxy()
        {
            onApplyTimeButtonClickedEvent.Invoke();
        }
        
        /// <summary>
        /// Proxy method for Back button click
        /// </summary>
        public void BackButtonProxy()
        {
            onBackButtonClickEvent.Invoke();
        }
    }
}