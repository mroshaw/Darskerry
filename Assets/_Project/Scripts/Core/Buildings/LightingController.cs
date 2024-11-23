using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    public class LightingController : MonoBehaviour
    {
        [BoxGroup("Settings")] [SerializeField] private bool findLightsOnAwake;
        [BoxGroup("Building Lights")] [SerializeField] private List<BuildingLight> candleLights;

        #region Startup

        private void Awake()
        {
            if (findLightsOnAwake)
            {
                UpdateLights();
            }
        }

        #endregion

        #region Class Methods

        [Button("Update Lights")]
        public void UpdateLights()
        {
            candleLights = new List<BuildingLight>();
            BuildingLight[] allLights = gameObject.GetComponentsInChildren<BuildingLight>(true);
            foreach (BuildingLight currLight in allLights)
            {
                if (currLight.BuildingLightType == BuildingLightType.IndoorCandle)
                {
                    currLight.UpdateLights();
                    candleLights.Add(currLight);
                }
            }
        }

        [Button("Turn On Candles")]
        public void TurnOnCandleLights()
        {
            SetLightsState(candleLights, true);
        }

        [Button("Turn Off Candles")]
        public void TurnOffCandleLights()
        {
            SetLightsState(candleLights, false);
        }

        private void SetLightsState(List<BuildingLight> lights, bool state)
        {
            foreach (BuildingLight currLight in lights)
            {
                currLight.SetLightState(state);
            }
        }

        #endregion
    }
}