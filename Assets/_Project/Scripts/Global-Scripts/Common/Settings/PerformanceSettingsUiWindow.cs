using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Settings
{
    public class PerformanceSettingsUiWindow : SettingsUiWindow, ISettingsUiWindow
    {
        [BoxGroup("UI Configuration")] public TMP_Dropdown textureResDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown antiAliasingDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown antiAliasingModeDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown qualityPresetDropdown;
        [BoxGroup("UI Configuration")] public Slider terrainDetailLevelSlider;
        [BoxGroup("UI Configuration")] public Toggle enableShadowsToggle;
        [BoxGroup("UI Configuration")] public Toggle enableVSyncToggle;
        [BoxGroup("UI Configuration")] public TMP_Dropdown resolutionConfigurationDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown dlssQualityDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown fsrQualityDropdown;
        [BoxGroup("UI Configuration")] public TMP_Dropdown targetFpsDropdown;

        [BoxGroup("Settings Model")] public PerformanceSettingsManager performanceSettingsManager;
        
        /// <summary>
        /// Configure the UI control handlers to call public methods
        /// </summary>
        public override void InitControls()
        {
            // Remove all listeners, to prevent doubling up.
            textureResDropdown.onValueChanged.RemoveAllListeners();
            antiAliasingDropdown.onValueChanged.RemoveAllListeners();
            antiAliasingModeDropdown.onValueChanged.RemoveAllListeners();
            qualityPresetDropdown.onValueChanged.RemoveAllListeners();
            terrainDetailLevelSlider.onValueChanged.RemoveAllListeners();
            enableShadowsToggle.onValueChanged.RemoveAllListeners();
            enableVSyncToggle.onValueChanged.RemoveAllListeners();
            resolutionConfigurationDropdown.onValueChanged.RemoveAllListeners();
            dlssQualityDropdown.onValueChanged.RemoveAllListeners();
            fsrQualityDropdown.onValueChanged.RemoveAllListeners();
            targetFpsDropdown.onValueChanged.RemoveAllListeners();

            // Configure the UI component listeners
            textureResDropdown.onValueChanged.AddListener(UpdateTextureResolution);
            antiAliasingDropdown.onValueChanged.AddListener(UpdateAntiAliasing);
            antiAliasingModeDropdown.onValueChanged.AddListener(UpdateAntiAliasingMode);
            qualityPresetDropdown.onValueChanged.AddListener(UpdateQualityPreset);
            terrainDetailLevelSlider.onValueChanged.AddListener(UpdateTerrainDetailLevel);
            enableShadowsToggle.onValueChanged.AddListener(UpdateEnableShadows);
            enableVSyncToggle.onValueChanged.AddListener(UpdateVSync);
            resolutionConfigurationDropdown.onValueChanged.AddListener(UpdateResolutionConfiguration);
            dlssQualityDropdown.onValueChanged.AddListener(UpdateDlssQuality);
            fsrQualityDropdown.onValueChanged.AddListener(UpdateFsrQuality);
            targetFpsDropdown.onValueChanged.AddListener(UpdateTargetFps);

            // Init dynamic drop downs
            InitQualitySettings();
        }

        /// <summary>
        /// Populate Quality Drop Down from Quality Settings
        /// </summary>
        private void InitQualitySettings()
        {
            qualityPresetDropdown.ClearOptions();
            List<string> qualityOptions = new List<string>(QualitySettings.names);
            qualityPresetDropdown.AddOptions(qualityOptions);
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
            enableVSyncToggle.SetIsOnWithoutNotify(performanceSettingsManager.VSync);
            resolutionConfigurationDropdown.SetValueWithoutNotify(performanceSettingsManager.ResolutionConfigurationIndex);
            dlssQualityDropdown.SetValueWithoutNotify(performanceSettingsManager.DlssQualityIndex);
            fsrQualityDropdown.SetValueWithoutNotify(performanceSettingsManager.FsrQualityIndex);
            targetFpsDropdown.SetValueWithoutNotify(performanceSettingsManager.TargetFpsIndex);
            UpdateResConfigControlState();
            int qualityIndex = qualityPresetDropdown.options.FindIndex(i => i.text == performanceSettingsManager.QualityPresetName);
            qualityPresetDropdown.SetValueWithoutNotify(qualityIndex);
        }

        /// <summary>
        /// Sets the dynamic resolution quality control states
        /// </summary>
        private void UpdateResConfigControlState()
        {
            switch ((PerformanceSettingsManager.DynamicResolutionType)performanceSettingsManager.ResolutionConfigurationIndex)
            {
                case PerformanceSettingsManager.DynamicResolutionType.None:
                    dlssQualityDropdown.interactable = false;
                    fsrQualityDropdown.interactable = false;
                    antiAliasingDropdown.interactable = true;
                    antiAliasingModeDropdown.interactable = true;
                    break;
                case PerformanceSettingsManager.DynamicResolutionType.DLSS:
                    dlssQualityDropdown.interactable = true;
                    fsrQualityDropdown.interactable = false;
                    antiAliasingDropdown.interactable = false;
                    antiAliasingModeDropdown.interactable = false;
                    break;
                case PerformanceSettingsManager.DynamicResolutionType.FSR:
                    dlssQualityDropdown.interactable = false;
                    fsrQualityDropdown.interactable = true;
                    antiAliasingDropdown.interactable = false;
                    antiAliasingModeDropdown.interactable = false;

                    break;
            }
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
        /// Handle Quality Preset value changed
        /// </summary>
        /// <param name="qualityPresetIndex"></param>
        public void UpdateQualityPreset(int qualityPresetIndex)
        {
            string qualityName = qualityPresetDropdown.options[qualityPresetIndex].text;
            performanceSettingsManager.SetQualityPreset(qualityName);
        }

        /// <summary>
        /// Handle Terrain Detail value changed
        /// </summary>
        /// <param name="terrainDetailLevel"></param>
        public void UpdateTerrainDetailLevel(float terrainDetailLevel)
        {
            performanceSettingsManager.SetTerrainDetailLevel(terrainDetailLevel);
        }

        /// <summary>
        /// Handle VSync toggle changed
        /// </summary>
        /// <param name="isEnabled"></param>
        public void UpdateVSync(bool isEnabled)
        {
            performanceSettingsManager.SetVSync(isEnabled);
        }

        public void UpdateResolutionConfiguration(int indexValue)
        {
            performanceSettingsManager.SetResolutionConfiguration(indexValue);
            UpdateResConfigControlState();
        }

        public void UpdateDlssQuality(int indexValue)
        {
            performanceSettingsManager.SetDlssQuality(indexValue);
        }

        public void UpdateFsrQuality(int indexValue)
        {
            performanceSettingsManager.SetFsrQuality(indexValue);
        }

        public void UpdateTargetFps(int indexValue)
        {
            performanceSettingsManager.SetTargetFps(indexValue);
        }
    }
}