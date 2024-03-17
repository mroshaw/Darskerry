#if ASMDEF
#if ENVIRO_3___
using Enviro;
#endif
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    public class InteriorLightController : MonoBehaviour
    {
        [Header("Startup Settings")]
        public bool findAllLightsOnStartup = true;
        private InteriorLight[] _allLightsInScene;

        [Header("Time Control")]
        public int[] onHours;
        public int[] offHours;

        [SerializeField]
        [Header("Interior Lights")]
        private List<InteriorLight> _interiorLights = new();

#if ASMDEF
#if HDRPTIMEOFDAY_
        private HDRPTimeOfDayHelper _hdrpTodHelper;
#endif
#endif
        /// <summary>
        /// Set up the Controller
        /// </summary>
        private void Start()
        {
            #if ASMDEF
#if ENVIRO_3__
            // Subscribe to the Enviro OnHOurPassed event
            EnviroManager.instance.Events.Settings.onHourPassedActions.AddListener(HourPassedLightUpdate);
#endif
#endif

#if ASMDEF
#if HDRPTIMEOFDAY_
            _hdrpTodHelper = FindObjectOfType<HDRPTimeOfDayHelper>();
            if(_hdrpTodHelper)
            {
                _hdrpTodHelper.OnHourPassed.AddListener(HourPassedLightUpdate);
            }
#endif
#endif
        }

        /// <summary>
        /// Populate the internal list of scene lights
        /// </summary>
        private void GetAllLightsInScene(Boolean forceRefresh = false)
        {
            if(_allLightsInScene.Length == 0 || forceRefresh)
            {
                _allLightsInScene = FindObjectsOfType<InteriorLight>(true);
            }
        }

        /// <summary>
        /// Clears the Light List
        /// </summary>
        private void ClearLightList()
        {
            _interiorLights.Clear();
        }

        /// <summary>
        /// Public method to register a new Interior Light to the controller
        /// </summary>
        /// <param name="interiorLight"></param>
        public void RegisterLight(InteriorLight interiorLight)
        {
            _interiorLights.Add(interiorLight);
            interiorLight.SetLightController(this);
        }

        /// <summary>
        /// Method to hook into Hours Passed enviro events
        /// </summary>
        private void HourPassedLightUpdate()
        {
#if ASMDEF
#if ENVIRO_3__
            if(onHours.Contains(EnviroManager.instance.Time.hours))
            {
                TurnOnAllLights();
            }
            else if (offHours.Contains(EnviroManager.instance.Time.hours))
            {
                TurnOnAllLights();
            }
#endif
#endif
#if ASMDEF
#if HDRPTIMEOFDAY_
            if (onHours.Contains(_hdrpTodHelper.currentHour))
            {
                TurnOnAllLights();
            }
            else if (offHours.Contains(_hdrpTodHelper.currentHour))
            {
                TurnOnAllLights();
            }
#endif
#endif
        }

        /// <summary>
        /// Public method to turn on all lights within the controller
        /// </summary>
        public void TurnOnAllLights()
        {
            foreach (InteriorLight interiorLight in _interiorLights)
            {
                interiorLight.TurnOnLight();
            }
        }

        public void TurnOffAllLights()
        {
            foreach(InteriorLight interiorLight in _interiorLights)
            {
                interiorLight.TurnOffLight();
            }
        }

        /// <summary>
        /// Find and register all lights in Scene
        /// </summary>
        public void RegisterAllLightsInScene()
        {
            ClearLightList();
            GetAllLightsInScene();

            foreach(InteriorLight light in _allLightsInScene)
            {
                RegisterLight(light);
            }
        }

        /// <summary>
        /// Toggle the state of all lights
        /// </summary>
        public void ToggleAllLights()
        {
            InteriorLight[] allLightsInScene = FindObjectsOfType<InteriorLight>(true);

            foreach (InteriorLight light in allLightsInScene)
            {
                light.ToggleLight();
            }
        }
    }
}
