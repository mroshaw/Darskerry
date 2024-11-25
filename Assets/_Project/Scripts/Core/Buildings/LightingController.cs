using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    public class LightingController : MonoBehaviour
    {
        [BoxGroup("Settings")] [SerializeField] private bool findLightsOnAwake;
        [BoxGroup("Building Lights")] [SerializeField] private List<BuildingLight> candleLights;
        [BoxGroup("Building Lights")] [SerializeField] private List<BuildingLight> cookingLights;

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
                currLight.UpdateLights();

                switch (currLight.BuildingLightType)
                {
                    case BuildingLightType.IndoorCandle:
                        candleLights.Add(currLight);
                        break;
                    case BuildingLightType.IndoorCooking:
                        cookingLights.Add(currLight);
                        break;
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

        [Button("Turn On Cooking")]
        public void TurnOnCookingLights()
        {
            SetLightsState(cookingLights, true);
        }

        [Button("Turn Off Cooking")]
        public void TurnOffCookingLights()
        {
            SetLightsState(cookingLights, false);
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