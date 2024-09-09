using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace DaftAppleGames.Darskerry.Core.UserInterface.Settings
{
    public class PerformanceSettingsUiWindow : SettingsUiWindow, ISettingsUiWindow
    {
        [BoxGroup("UI Configuration")] public TMP_Dropdown textureResDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown antiAliasingDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown antiAliasingModeDropdown;
        [BoxGroup("UI Configuration")] public Slider terrainDetailLevelSlider;
        [BoxGroup("UI Configuration")] public Toggle enableShadowsToggle;
        [BoxGroup("UI Configuration")] public TMP_Dropdown targetFpsDropdown;
        [BoxGroup("UI Configuration")] public Toggle enableSSRToggle;
        [BoxGroup("UI Configuration")] public Toggle enableSSGIToggle;
        [BoxGroup("UI Configuration")] public Toggle enableSSAOToggle;
        [BoxGroup("UI Configuration")] public Toggle enableOverridesToggle;

        [BoxGroup("Settings Model")] public PerformanceSettingsManager performanceSettingsManager;
        
        /// <summary>
        /// Configure the UI control handlers to call public methods
        /// </summary>
        public override void InitControls()
        {
            // Remove all listeners, to prevent doubling up.
            textureResDropdown.onValueChanged.RemoveListener(UpdateTextureResolution);
            antiAliasingDropdown.onValueChanged.RemoveListener(UpdateAntiAliasing);
            antiAliasingModeDropdown.onValueChanged.RemoveListener(UpdateAntiAliasingMode);
            terrainDetailLevelSlider.onValueChanged.RemoveListener(UpdateTerrainDetailLevel);
            enableShadowsToggle.onValueChanged.RemoveListener(UpdateEnableShadows);
            targetFpsDropdown.onValueChanged.RemoveListener(UpdateTargetFps);
            enableSSRToggle.onValueChanged.RemoveListener(UpdateSSR);
            enableSSGIToggle.onValueChanged.RemoveListener(UpdateSSGI);
            enableSSAOToggle.onValueChanged.RemoveListener(UpdateSSAO);
            enableOverridesToggle.onValueChanged.RemoveListener(UpdateEnableOverrides);

            // Configure the UI component listeners
            textureResDropdown.onValueChanged.AddListener(UpdateTextureResolution);
            antiAliasingDropdown.onValueChanged.AddListener(UpdateAntiAliasing);
            antiAliasingModeDropdown.onValueChanged.AddListener(UpdateAntiAliasingMode);
            terrainDetailLevelSlider.onValueChanged.AddListener(UpdateTerrainDetailLevel);
            enableShadowsToggle.onValueChanged.AddListener(UpdateEnableShadows);
            targetFpsDropdown.onValueChanged.AddListener(UpdateTargetFps);
            enableSSRToggle.onValueChanged.AddListener(UpdateSSR);
            enableSSGIToggle.onValueChanged.AddListener(UpdateSSGI);
            enableSSAOToggle.onValueChanged.AddListener(UpdateSSAO);
            enableOverridesToggle.onValueChanged.AddListener(UpdateEnableOverrides);
        }


        /// <summary>
        /// Initialise the controls with current settings
        /// </summary>
        public override void RefreshControlState()
        {
            base.RefreshControlState();
            textureResDropdown.SetValueWithoutNotify(performanceSettingsManager.TextureResolutionIndex);
            antiAliasingDropdown.SetValueWithoutNotify(performanceSettingsManager.AntiAliasingResolutionIndex);
            antiAliasingModeDropdown.SetValueWithoutNotify(performanceSettingsManager.AntiAliasingModeIndex);
            terrainDetailLevelSlider.SetValueWithoutNotify(performanceSettingsManager.TerrainDetailLevel);
            enableShadowsToggle.SetIsOnWithoutNotify(performanceSettingsManager.EnableShadows);
            targetFpsDropdown.SetValueWithoutNotify(performanceSettingsManager.TargetFpsIndex);

            enableSSRToggle.SetIsOnWithoutNotify(performanceSettingsManager.EnableSSR);
            enableSSGIToggle.SetIsOnWithoutNotify(performanceSettingsManager.EnableSSGI);
            enableSSAOToggle.SetIsOnWithoutNotify(performanceSettingsManager.EnableSSAO);
            enableOverridesToggle.SetIsOnWithoutNotify(performanceSettingsManager.EnableOverrides);
            SetAdvancedControlState(performanceSettingsManager.EnableOverrides);
        }

        /// <summary>
        /// Handle toggling the override toggle
        /// </summary>
        /// <param name="isEnabled"></param>
        public void UpdateEnableOverrides(bool isEnabled)
        {
            performanceSettingsManager.EnableOverrides = isEnabled;
            SetAdvancedControlState(isEnabled);
        }

        /// <summary>
        /// Set override control state
        /// </summary>
        /// <param name="isEnabled"></param>
        public void SetAdvancedControlState(bool isEnabled)
        {
            textureResDropdown.interactable = isEnabled;
            antiAliasingDropdown.interactable = isEnabled;
            antiAliasingModeDropdown.interactable = isEnabled;
            terrainDetailLevelSlider.interactable = isEnabled;
            enableShadowsToggle.interactable = isEnabled;
            targetFpsDropdown.interactable = isEnabled;
            enableSSRToggle.interactable = isEnabled;
            enableSSGIToggle.interactable = isEnabled;
            enableSSAOToggle.interactable = isEnabled;
        }

        /// <summary>
        /// Update the Enable Shadows setting
        /// </summary>
        /// <param name="enableShadows"></param>
        public void UpdateEnableShadows(bool enableShadows)
        {
            performanceSettingsManager.SetEnableShadows(enableShadows);
        }

        /// <summary>
        /// UI controller method to manage "Master Volume" UI changes
        /// </summary>
        /// <param name="textureResIndex"></param>
        public void UpdateTextureResolution(int textureResIndex)
        {
            performanceSettingsManager.SetTextureResolution(textureResIndex);
        }

        /// <summary>
        /// Handle Anti Aliasing value change
        /// </summary>
        /// <param name="antiAliasingIndex"></param>
        public void UpdateAntiAliasing(int antiAliasingIndex)
        {
            performanceSettingsManager.SetAntiAliasing(antiAliasingIndex);
        }

        /// <summary>
        /// Handle Anti Aliasing mode value change
        /// </summary>
        /// <param name="antiAliasingModeIndex"></param>
        public void UpdateAntiAliasingMode(int antiAliasingModeIndex)
        {
            performanceSettingsManager.SetAntiAliasingMode(antiAliasingModeIndex);
        }

        /// <summary>
        /// Handles SSR toggle change
        /// </summary>
        /// <param name="isEnabled"></param>
        public void UpdateSSR(bool isEnabled)
        {
            performanceSettingsManager.SetSSR(isEnabled);
        }

        /// <summary>
        /// Handles SSGI toggle change
        /// </summary>
        /// <param name="isEnabled"></param>
        public void UpdateSSGI(bool isEnabled)
        {
            performanceSettingsManager.SetSSGI(isEnabled);
        }

        /// <summary>
        /// Handles the SSAO toggle change
        /// </summary>
        /// <param name="isEnabled"></param>
        public void UpdateSSAO(bool isEnabled)
        {
            performanceSettingsManager.SetSSAO(isEnabled);
        }


        /// <summary>
        /// Handle Terrain Detail value changed
        /// </summary>
        /// <param name="terrainDetailLevel"></param>
        public void UpdateTerrainDetailLevel(float terrainDetailLevel)
        {
            performanceSettingsManager.SetTerrainDetailLevel(terrainDetailLevel);
        }

        public void UpdateTargetFps(int indexValue)
        {
            performanceSettingsManager.SetTargetFps(indexValue);
        }
    }
}