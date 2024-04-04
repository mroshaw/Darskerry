using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    public enum StartBehaviour { AllOn, AllOff, DayState, NightState, Configured }

    public class BuildingLights : MonoBehaviour
    {
        [BoxGroup("Behaviour")] public bool refreshOnStart = false;
        [BoxGroup("Behaviour")] public bool configureOnStart = false;
        [BoxGroup("Behaviour")] public StartBehaviour startBehaviour = StartBehaviour.AllOff;

        [InlineEditor][FoldoutGroup("Interior Light Settings")] public BuildingLightSettings intCandleSettings;
        [InlineEditor][FoldoutGroup("Interior Light Settings")] public BuildingLightSettings intFireplaceSettings;
        [InlineEditor][FoldoutGroup("Interior Light Settings")] public BuildingLightSettings intCookingFireSettings;

        [InlineEditor][FoldoutGroup("Exterior Light Settings")] public BuildingLightSettings extTorchSettings;
        [InlineEditor][FoldoutGroup("Exterior Light Settings")] public BuildingLightSettings extStreetLampSettings;

        [FoldoutGroup("Interior Lights")] public List<BuildingLight> intCandles;
        [FoldoutGroup("Interior Lights")] public List<BuildingLight> intFireplaces;
        [FoldoutGroup("Interior Lights")] public List<BuildingLight> intCookingFires;
        [FoldoutGroup("Interior Lights")] public List<BuildingReflectionProbe> intReflectionProbes;

        [FoldoutGroup("Exterior Lights")] public List<BuildingLight> extTorches;
        [FoldoutGroup("Exterior Lights")] public List<BuildingLight> extStreetLamps;
        [FoldoutGroup("Exterior Lights")] public List<BuildingReflectionProbe> extReflectionProbes;

        /// <summary>
        /// Set up the component
        /// </summary>
        private void Start()
        {
            if (refreshOnStart)
            {
                RefreshLightLists();
            }

            if (configureOnStart)
            {
                ApplyLightSettings();
            }

            switch (startBehaviour)
            {
                case StartBehaviour.AllOff:
                    TurnOffAllLights();
                    break;
                case StartBehaviour.AllOn:
                    TurnOnAllLights();
                    break;
                case StartBehaviour.DayState:
                    EnableDaytimeLights();
                    break;
                case StartBehaviour.NightState:
                    EnableNighttimeLights();
                    break;
            }
        }

        [BoxGroup("Configuration")][Button("Refresh Light Lists")]
        private void RefreshLightLists()
        {
            intCandles = new List<BuildingLight>();
            intFireplaces = new List<BuildingLight>();
            intCookingFires = new List<BuildingLight>();
            intReflectionProbes = new List<BuildingReflectionProbe>();

            extTorches = new List<BuildingLight>();
            extStreetLamps = new List<BuildingLight>();
            extReflectionProbes = new List<BuildingReflectionProbe>();

            BuildingLight[] allBuildingLights = GetComponentsInChildren<BuildingLight>(true);

            foreach (BuildingLight currentBuildingLight in allBuildingLights)
            {
                currentBuildingLight.RefreshVisualEffects();
                switch (currentBuildingLight.lightLocation)
                {
                    case BuildingLightLocation.Interior:
                        switch (currentBuildingLight.lightType)
                        {
                            case BuildingLightType.Candle:
                                {
                                    intCandles.Add(currentBuildingLight);
                                    break;

                                }
                            case BuildingLightType.Fireplace:
                                {
                                    intFireplaces.Add(currentBuildingLight);
                                    break;
                                }
                            case BuildingLightType.Cooking:
                                {
                                    intCookingFires.Add(currentBuildingLight);
                                    break;
                                }
                        }

                        break;

                    case BuildingLightLocation.Exterior:
                    {
                        switch (currentBuildingLight.lightType)
                        {
                            case BuildingLightType.Torch:
                                extTorches.Add(currentBuildingLight);
                                break;
                            case BuildingLightType.StreetLamp:
                                extStreetLamps.Add(currentBuildingLight);
                                break;
                        }
                        break;
                    }
                }
            }

            BuildingReflectionProbe[] allProbes = GetComponentsInChildren<BuildingReflectionProbe>(true);
            foreach (BuildingReflectionProbe probe in allProbes)
            {
                switch (probe.lightLocation)
                {
                    case BuildingLightLocation.Interior:
                        {
                            intReflectionProbes.Add(probe);
                            break;

                        }
                    case BuildingLightLocation.Exterior:
                        {
                            extReflectionProbes.Add(probe);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Populates lists of all lights
        /// </summary>
        [BoxGroup("Configuration")] [Button("Apply Settings")]
        private void ApplyLightSettings()
        {
            // Apply internal candle settings
            foreach (BuildingLight intCandle in intCandles)
            {
                intCandleSettings.ConfigureLight(intCandle.Light);
            }

            // Apply internal fireplace settings
            foreach (BuildingLight intFireplace in intFireplaces)
            {
                intFireplaceSettings.ConfigureLight(intFireplace.Light);
            }

            // Apply internal cooking settings
            foreach (BuildingLight intCookingFire in intCookingFires)
            {
                intCookingFireSettings.ConfigureLight(intCookingFire.Light);
            }

            // Apply external torch settings
            foreach (BuildingLight extTorch in extTorches)
            {
                intCandleSettings.ConfigureLight(extTorch.Light);
            }

            // Apply external streetlight settings
            foreach (BuildingLight extStreetLamp in extStreetLamps)
            {
                extStreetLampSettings.ConfigureLight(extStreetLamp.Light);
            }
        }

        [BoxGroup("Control")]
        [Button("Turn All On")]
        public void TurnOnAllLights()
        {
            TurnOnIntCandles();
            TurnOnIntCookingFires();
            TurnOnIntFireplaces();
            TurnOnExtStreetLamps();
            TurnOnExtTorches();
        }

        [BoxGroup("Control")]
        [Button("Turn All Off")]
        public void TurnOffAllLights()
        {
            TurnOffIntCandles();
            TurnOffIntCookingFires();
            TurnOffIntFireplaces();
            TurnOffExtStreetLamps();
            TurnOffExtTorches();
        }

        [BoxGroup("Control")] [Button("Daytime Lights")]
        public void EnableDaytimeLights()
        {
            SetLightsState(intCandles, intCandleSettings.dayTimeState);
            SetLightsState(intFireplaces, intFireplaceSettings.dayTimeState);
            SetLightsState(intCookingFires, intCookingFireSettings.dayTimeState);
            SetLightsState(extTorches, extTorchSettings.dayTimeState);
            SetLightsState(extStreetLamps, extStreetLampSettings.dayTimeState);
        }

        [BoxGroup("Control")] [Button("Nighttime Lights")]
        public void EnableNighttimeLights()
        {
            SetLightsState(intCandles, intCandleSettings.nightTimeState);
            SetLightsState(intFireplaces, intFireplaceSettings.nightTimeState);
            SetLightsState(intCookingFires, intCookingFireSettings.nightTimeState);
            SetLightsState(extTorches, extTorchSettings.nightTimeState);
            SetLightsState(extStreetLamps, extStreetLampSettings.nightTimeState);
        }

        [FoldoutGroup("Interior Granular Control")]
        [Button("Turn on Candles")]
        public void TurnOnIntCandles()
        {
            SetLightsState(intCandles, true);
            RefreshInteriorProbes();
        }

        [FoldoutGroup("Interior Granular Control")]
        [Button("Turn off Candles")]
        public void TurnOffIntCandles()
        {
            SetLightsState(intCandles, false);
            RefreshInteriorProbes();
        }

        [FoldoutGroup("Interior Granular Control")]
        [Button("Turn on Fireplaces")]
        public void TurnOnIntFireplaces()
        {
            SetLightsState(intFireplaces, true);
            RefreshInteriorProbes();
        }

        [FoldoutGroup("Interior Granular Control")]
        [Button("Turn off Fireplaces")]
        public void TurnOffIntFireplaces()
        {
            SetLightsState(intFireplaces, false);
            RefreshInteriorProbes();
 }

        [FoldoutGroup("Interior Granular Control")]
        [Button("Turn on Cooking Fires")]
        public void TurnOnIntCookingFires()
        {
            SetLightsState(intCookingFires, true);
            RefreshInteriorProbes();
        }

        [FoldoutGroup("Interior Granular Control")]
        [Button("Turn off Cooking Fires")]
        public void TurnOffIntCookingFires()
        {
            SetLightsState(intCookingFires, false);
            RefreshInteriorProbes();
        }

        [FoldoutGroup("Exterior Granular Control")]
        [Button("Turn on Torches")]
        public void TurnOnExtTorches()
        {
            SetLightsState(extTorches, true);
            RefreshExteriorProbes();
        }

        [FoldoutGroup("Exterior Granular Control")]
        [Button("Turn off Torches")]
        public void TurnOffExtTorches()
        {
            SetLightsState(extTorches, false);
            RefreshExteriorProbes();
        }

        [FoldoutGroup("Exterior Granular Control")]
        [Button("Turn on Street Lamps")]
        public void TurnOnExtStreetLamps()
        {
            SetLightsState(extStreetLamps, true);
            RefreshExteriorProbes();
        }

        [FoldoutGroup("Exterior Granular Control")]
        [Button("Turn off Torches")]
        public void TurnOffExtStreetLamps()
        {
            SetLightsState(extStreetLamps, false);
            RefreshExteriorProbes();
        }

        /// <summary>
        /// Refresh interior Reflection probes
        /// </summary>
        public void RefreshInteriorProbes()
        {
            RefreshProbes(intReflectionProbes);
        }

        /// <summary>
        /// Refresh exterior Reflection probes
        /// </summary>
        public void RefreshExteriorProbes()
        {
            RefreshProbes(extReflectionProbes);
        }

        /// <summary>
        /// Refresh all ReflectionProbes in the list
        /// </summary>
        /// <param name="reflectionProbes"></param>
        private void RefreshProbes(List<BuildingReflectionProbe> reflectionProbes)
        {
            foreach (BuildingReflectionProbe probe in intReflectionProbes)
            {
                probe.Refresh();
            }
        }

        /// <summary>
        /// Sets all lights in the list to the provided state
        /// </summary>
        /// <param name="lights"></param>
        /// <param name="state"></param>
        private void SetLightsState(List<BuildingLight> lights, bool state)
        {
            if (lights == null)
            {
                return;

            }

            foreach (BuildingLight currentLight in lights)
            {
                if (currentLight)
                {
                    currentLight.SetLightState(state);
                }
            } 
        }
    }
}