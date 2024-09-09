using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#if GPU_INSTANCER
using GPUInstancer;
#endif


namespace DaftAppleGames.Darskerry.Core.UserInterface.Settings
{
    public class DisplaySettingsManager : BaseSettingsManager, ISettings
    {
        [BoxGroup("Defaults")] public float defaultScreenBrightness = 1.0f;
        [BoxGroup("Defaults")] public bool defaultFullScreen = true;
        [BoxGroup("Defaults")] public int defaultDisplayResIndex = 0;
        [BoxGroup("Defaults")] public string defaultQualityPresetName = "Balanced";
        [BoxGroup("Defaults")] public int defaultDLSSQualityIndex = 0;
        [BoxGroup("Defaults")] public int defaultFSRQualityIndex = 0;
        [BoxGroup("Defaults")] public bool defaultVSync = true;

        [BoxGroup("Setting Keys")] public string screenBrightnessKey = "ScreenBrightness";
        [BoxGroup("Setting Keys")] public string fullScreenKey = "FullScreen";
        [BoxGroup("Setting Keys")] public string displayResIndexKey = "ScreenResolution";
        [BoxGroup("Setting Keys")] private const string QualityPresetNameKey = "QualityPreset";
        [BoxGroup("Setting Keys")] private const string ResolutionConfigurationKey = "EnableDLSS";
        [BoxGroup("Setting Keys")] private const string DLSSQualityIndexKey = "DlssQuality";
        [BoxGroup("Setting Keys")] private const string FSRQualityIndexKey = "FsrQuality";
        [BoxGroup("Setting Keys")] private const string VSyncKey = "VSync";

        [BoxGroup("Core Game Objects")] public Light mainDirectionalLight;
        [BoxGroup("Core Game Objects")] public Camera mainCamera;

        // Public properties
        public float ScreenBrightness { get; set; }
        public bool FullScreen { get; set; }
        public FullScreenMode FullScreenMode => (FullScreen) ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        public bool VSync { get; set; }
        public int DisplayResIndex { get; set; }
        public string QualityPresetName { get; set; }
        public Resolution[] DisplayResArray { get; set; }
        public int ResolutionConfigurationIndex { get; set; }
        public int DlssQualityIndex { get; set; }
        public int FsrQualityIndex { get; set; }

        /// <summary>
        /// Determine the default Dynamic Resolution mode
        /// </summary>
        /// <returns></returns>
        private int GetDefaultDynamicConfigResIndex()
        {
            // First, check for DLSS
            if (HDDynamicResolutionPlatformCapabilities.DLSSDetected)
            {
                return 1;
            }
            else
            {
                // Enable FSR
                return 2;
            }
        }

        public ResolutionConfiguration[] Configurations = new ResolutionConfiguration[]
        {
            new ResolutionConfiguration
            {
                name = "Native",
                AntialiasingMode = HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing,
                TAAQualityLevel = HDAdditionalCameraData.TAAQualityLevel.High,
                DynamicResolutionScale = 1,
            },
            new ResolutionConfiguration
            {
                name = "Dlss",
                dLSSQuality = DLSSQuality.MaximumQuality,
                QualityMode = 2,
                AntialiasingMode = HDAdditionalCameraData.AntialiasingMode.None,
                DynamicResolutionScale = 1
            },
            new ResolutionConfiguration
            {
                name = "FSR",
                fsrQuality = FSRQuality.UltraQuality,
                AntialiasingMode = HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing,
                TAAQualityLevel = HDAdditionalCameraData.TAAQualityLevel.Low,
                DynamicResolutionScale = 1
            }
        };

        /// <summary>
        /// Save settings to Player Prefs
        /// </summary>
        public override void SaveSettings()
        {
            SettingsUtils.SaveFloatSetting(screenBrightnessKey, ScreenBrightness);
            SettingsUtils.SaveBoolSetting(fullScreenKey, FullScreen);
            SettingsUtils.SaveIntSetting(displayResIndexKey, DisplayResIndex);
            SettingsUtils.SaveStringSetting(QualityPresetNameKey, QualityPresetName);
            SettingsUtils.SaveIntSetting(ResolutionConfigurationKey, ResolutionConfigurationIndex);
            SettingsUtils.SaveIntSetting(DLSSQualityIndexKey, DlssQualityIndex);
            SettingsUtils.SaveIntSetting(FSRQualityIndexKey, FsrQualityIndex);
            SettingsUtils.SaveBoolSetting(VSyncKey, VSync);
            base.SaveSettings();
        }

        /// <summary>
        /// Load settings from Player Prefs
        /// </summary>
        public override void LoadSettings()
        {
            ScreenBrightness = SettingsUtils.LoadFloatSetting(screenBrightnessKey, defaultScreenBrightness);
            FullScreen = SettingsUtils.LoadBoolSetting(fullScreenKey, defaultFullScreen);
            DisplayResIndex = SettingsUtils.LoadIntSetting(displayResIndexKey, GetCurrentResolutionIndex());
            QualityPresetName = SettingsUtils.LoadStringSetting(QualityPresetNameKey, defaultQualityPresetName);
            ResolutionConfigurationIndex =
                SettingsUtils.LoadIntSetting(ResolutionConfigurationKey, GetDefaultDynamicConfigResIndex());
            DlssQualityIndex = SettingsUtils.LoadIntSetting(DLSSQualityIndexKey, defaultDLSSQualityIndex);
            FsrQualityIndex = SettingsUtils.LoadIntSetting(FSRQualityIndexKey, defaultFSRQualityIndex);
            VSync = SettingsUtils.LoadBoolSetting(VSyncKey, defaultVSync);
            base.LoadSettings();
        }
        
        /// <summary>
        /// Apply all current settings
        /// </summary>
        public override void ApplySettings()
        {
            ApplyQualityPresets();
            ApplyVSync();
            ApplyScreenBrightness();
            ApplyFullScreen();
            ApplyDisplayResolution();
            ApplyResolutionConfiguration();
            ApplyDlssQuality();
            ApplyFsrQuality();
            base.ApplySettings();
        }
        
        /// <summary>
        /// Setup any lists or arrays
        /// </summary>
        public override void InitSettings()
        {
            // Populate the "Screen Resolution" list
            PopulateDisplayResolutions();
        }

        /// <summary>
        /// Populate the list of available screen resolutions
        /// </summary>
        private void PopulateDisplayResolutions()
        {
            Resolution[] resolutions = Screen.resolutions;

            List<Resolution> filterResolutions = new();

            foreach (Resolution res in resolutions)
            {
                if (!filterResolutions.Any(item => item.height == res.height && item.width == res.width))
                {
                    filterResolutions.Add(res);
                }
            }

            DisplayResArray = filterResolutions.ToArray();
        }

        /// <summary>
        /// Returns the index of the current, active screen resolution
        /// </summary>
        /// <returns></returns>
        public int GetCurrentResolutionIndex()
        {
            for (int currRes = 0; currRes < DisplayResArray.Length; currRes++)
            {
                 if (DisplayResArray[currRes].width == Screen.currentResolution.width && DisplayResArray[currRes].height == Screen.currentResolution.height)
                 {
                     return currRes;
                 }
            }
            Debug.Log($"Resolution not found! Defaulting....");
            return defaultDisplayResIndex;
        }

        /// <summary>
        /// Updates and applies Quality Preset
        /// </summary>
        /// <param name="value"></param>
        public void SetQualityPreset(string value)
        {
            QualityPresetName = value;
            ApplyQualityPresets();
        }

        /// <summary>
        /// Set the Resolution configuration
        /// </summary>
        /// <param name="value"></param>
        public void SetResolutionConfiguration(int value)
        {
            ResolutionConfigurationIndex = value;
            ApplyResolutionConfiguration();
        }

        /// <summary>
        /// Set the DLSS Quality
        /// </summary>
        /// <param name="value"></param>
        public void SetDlssQuality(int value)
        {
            DlssQualityIndex = value;
            ApplyDlssQuality();
        }

        /// <summary>
        /// Set the FSE Quality
        /// </summary>
        /// <param name="value"></param>
        public void SetFsrQuality(int value)
        {
            FsrQualityIndex = value;
            ApplyFsrQuality();
        }

        /// <summary>
        /// Sets the VSync value
        /// </summary>
        /// <param name="value"></param>
        public void SetVSync(bool value)
        {
            VSync = value;
            ApplyVSync();
        }


        /// <summary>
        /// Apply VSync
        /// </summary>
        private void ApplyVSync()
        {
            if (VSync)
            {
                Debug.Log("VSync Enabled");
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                Debug.Log("VSync Disabled");
                QualitySettings.vSyncCount = 0;
            }
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Applies dynamic resolution settings
        /// </summary>
        private void ApplyResolutionConfigurationToCamera()
        {
            if (!mainCamera)
                return;

            ResolutionConfiguration mode = Configurations[ResolutionConfigurationIndex];

            HDAdditionalCameraData HDCam = mainCamera.gameObject.GetComponent<HDAdditionalCameraData>();
            HDCam.antialiasing = mode.AntialiasingMode;
            HDCam.TAAQuality = mode.TAAQualityLevel;

            switch ((DynamicResolutionType)ResolutionConfigurationIndex)
            {
                case DynamicResolutionType.None:
                    HDCam.allowDeepLearningSuperSampling = false;
                    HDCam.allowDynamicResolution = false;
                    #if GPU_INSTANCER
                    SetGPUIOcclusionCullingState(true);
                    #endif
                    break;
                case DynamicResolutionType.DLSS:
                    HDCam.allowDynamicResolution = true;
                    HDCam.allowDeepLearningSuperSampling = true;
                    HDCam.deepLearningSuperSamplingUseCustomQualitySettings = true;
                    HDCam.deepLearningSuperSamplingQuality = mode.QualityMode;
                    #if GPU_INSTANCER
                    SetGPUIOcclusionCullingState(false);
                    #endif
                    break;
                case DynamicResolutionType.FSR:
                    HDCam.allowDynamicResolution = true;
                    HDCam.allowDeepLearningSuperSampling = false;
                    #if GPU_INSTANCER
                    SetGPUIOcclusionCullingState(false);
                    #endif
                    break;
            }
        }

#if GPU_INSTANCER
        /// <summary>
        /// Used to set the state (true or false) of Occlussion culling in GPU Instancer, if in use.
        /// Required when using DLSS, as it's not supported with Occlussion Culling
        /// </summary>
        /// <param name="state"></param>
        private void SetGPUIOcclusionCullingState(bool state)
        {
            List<GPUInstancerManager> allManagers = GPUInstancerAPI.GetActiveManagers();
            if (allManagers == null || allManagers.Count == 0)
            {
                return;
            }
            foreach (GPUInstancerManager manager in allManagers)
            {
                manager.isOcclusionCulling = state;
            }
        }
#endif

        /// <summary>
        ///
        /// </summary>
        public void ApplyResolutionConfiguration()
        {
            switch ((DynamicResolutionType)ResolutionConfigurationIndex)
            {
                case DynamicResolutionType.None:
                    SwitchToDLSSAndNativeMode();
                    break;
                case DynamicResolutionType.DLSS:
                    SwitchToDLSSAndNativeMode();
                    break;
                case DynamicResolutionType.FSR:
                    SwitchToFSRMode();
                    break;
            }
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply DLSS quality settings
        /// </summary>
        public void ApplyDlssQuality()
        {
            DLSSQuality dLSSQuality = (DLSSQuality)DlssQualityIndex;
            uint QVal = 2;
            switch (dLSSQuality)
            {
                case DLSSQuality.MaximumPerformance:
                    QVal = 0;
                    break;
                case DLSSQuality.Balanced:
                    QVal = 1;
                    break;
                case DLSSQuality.MaximumQuality:
                    QVal = 2;
                    break;
                case DLSSQuality.UltraPerformance:
                    QVal = 3;
                    break;
            }
            Configurations[ResolutionConfigurationIndex].QualityMode = QVal;
            ApplyResolutionConfigurationToCamera();
        }

        /// <summary>
        /// Apply FSR quality settings
        /// </summary>
        public void ApplyFsrQuality()
        {
            FSRQuality Q = (FSRQuality)FsrQualityIndex;
            Configurations[ResolutionConfigurationIndex].fsrQuality = Q;
            Configurations[ResolutionConfigurationIndex].DynamicResolutionScale = GetFSRDynamicScale(Q);
            ApplyResolution();
        }

        /// <summary>
        ///
        /// </summary>
        private void SwitchToDLSSAndNativeMode()
        {
            ApplyResolution();
            ApplyResolutionConfigurationToCamera();
        }

        /// <summary>
        ///
        /// </summary>
        private void SwitchToFSRMode()
        {
            Configurations[ResolutionConfigurationIndex].DynamicResolutionScale = GetFSRDynamicScale(Configurations[ResolutionConfigurationIndex].fsrQuality);
            ApplyResolution();
            ApplyResolutionConfigurationToCamera();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="quality"></param>
        /// <returns></returns>
        float GetFSRDynamicScale(FSRQuality quality)
        {
            float newScale = 1;

            switch (quality)
            {
                case FSRQuality.UltraQuality:
                    newScale = .76953125F;
                    break;
                case FSRQuality.HighQuality:
                    newScale = .73F;
                    break;
                case FSRQuality.Quality:
                    newScale = .66640625F;
                    break;
                case FSRQuality.Balanced:
                    newScale = .58828125F;
                    break;
                case FSRQuality.Performance:
                    newScale = .5f;
                    break;
            }
            return newScale;
        }

        /// <summary>
        ///
        /// </summary>
        void ApplyResolution()
        {
            DynamicResolutionHandler.SetDynamicResScaler(SetDynamicResolutionScale, DynamicResScalePolicyType.ReturnsMinMaxLerpFactor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        float SetDynamicResolutionScale()
        {
            return Configurations[ResolutionConfigurationIndex].DynamicResolutionScale;
        }

        public class ResolutionConfiguration
        {
            public string name;
            public DLSSQuality dLSSQuality;
            public uint QualityMode;
            public FSRQuality fsrQuality;
            public float DynamicResolutionScale;
            public HDAdditionalCameraData.AntialiasingMode AntialiasingMode;
            public HDAdditionalCameraData.TAAQualityLevel TAAQualityLevel;
        }

        public enum DynamicResolutionType
        {
            None,
            DLSS,
            FSR
        }

        public enum DLSSQuality
        {
            MaximumQuality,
            Balanced,
            MaximumPerformance,
            UltraPerformance
        }
        public enum FSRQuality
        {
            UltraQuality,
            HighQuality,
            Quality,
            Balanced,
            Performance
        }

        /// <summary>
        /// Apply the Quality Presets
        /// </summary>
        private void ApplyQualityPresets()
        {
            // Lookup quality settings
            string[] qualityNames = QualitySettings.names;
            int qualityIndex = Array.IndexOf(qualityNames, QualityPresetName);

            if (qualityIndex >= 0)
            {
                QualitySettings.SetQualityLevel(qualityIndex, true);
            }
            else
            {
                Debug.Log($"Quality Setting not found! {QualityPresetName}. Resetting to {defaultQualityPresetName}");
                QualityPresetName = defaultQualityPresetName;
                ApplyQualityPresets();
            }
        }
        /// <summary>
        /// Update and apply Display Resolution
        /// </summary>
        /// <param name="displayResIndexToSet"></param>
        public void SetDisplayResIndex(int displayResIndexToSet)
        {
            DisplayResIndex = displayResIndexToSet;
            ApplyDisplayResolution();
        }

        /// <summary>
        /// Update and apply Screen Brightness
        /// </summary>
        /// <param name="screenBrightnessToSet"></param>
        public void SetScreenBrightness(float screenBrightnessToSet)
        {
            ScreenBrightness = screenBrightnessToSet;
            ApplyScreenBrightness();
        }

        /// <summary>
        /// Update and apply Full Screen
        /// </summary>
        /// <param name="fullScreenToSet"></param>
        public void SetFullScreen(bool fullScreenToSet)
        {
            FullScreen = fullScreenToSet;
            ApplyFullScreen();
        }

        /// <summary>
        /// Apply current Screen Brightness
        /// </summary>
        private void ApplyScreenBrightness()
        {

        }

        /// <summary>
        /// Apply current Full Screen
        /// </summary>
        private void ApplyFullScreen()
        {
            Screen.fullScreenMode = FullScreenMode;
            Screen.fullScreen = FullScreen;
        }

        /// <summary>
        /// Apply the current Display Resolution settings
        /// </summary>
        private void ApplyDisplayResolution()
        {
            Resolution currentResolution = DisplayResArray[DisplayResIndex];
            if (FullScreen)
            {
                Debug.Log($"Setting screen resolution to: {currentResolution.width}x{currentResolution.height} with refresh rate {currentResolution.refreshRateRatio}. Full screen mode is {Screen.fullScreenMode}");
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreenMode, Screen.currentResolution.refreshRateRatio);
            }
            else
            {
                Debug.Log($"Setting screen resolution to: {currentResolution.width}x{currentResolution.height}. Full screen mode is {Screen.fullScreenMode}");
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreenMode);
            }

            Screen.fullScreen = FullScreen;
        }

        /// <summary>
        /// Get the Refresh Rate in the new structure
        /// </summary>
        /// <param name="frameRate"></param>
        /// <returns></returns>
        private RefreshRate GetRefreshRate(float frameRate)
        {
                var numerator = (uint) Math.Round(frameRate,6) * 1000000;
                uint denominator = 1000000;
                return new RefreshRate() {numerator = numerator, denominator = denominator};
        }
    }
}