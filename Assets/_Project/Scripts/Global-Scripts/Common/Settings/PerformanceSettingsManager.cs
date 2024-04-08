using System;
using DaftAppleGames.Common.GameControllers;
using DaftAppleGames.Common.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif
#if HDRPPACKAGE_EXIST
using UnityEngine.Rendering.HighDefinition;
using static DaftAppleGames.Common.Settings.PerformanceSettingsManager;
#endif
#if GPU_INSTANCER
using GPUInstancer;
#endif

namespace DaftAppleGames.Common.Settings
{
    public class PerformanceSettingsManager : BaseSettingsManager, ISettings
    {
        [BoxGroup("Performance Defaults")] public int defaultTextureResolutionIndex = 0;
        [BoxGroup("Performance Defaults")] public int defaultAntiAliasingResolutionIndex = 0;
        [BoxGroup("Performance Defaults")] public string defaultQualityPresetName = "Low";
        [BoxGroup("Performance Defaults")] public float defaultTerrainDetailLevel = 2.0f;
        [BoxGroup("Performance Defaults")] public bool defaultEnableShadows = true;
        [BoxGroup("Performance Defaults")] public int defaultAntiAliasingModeIndex = 1;
        [BoxGroup("Performance Defaults")] public bool defaultVSync = true;
        [BoxGroup("Performance Defaults")] public int defaultResolutionConfiguration = 1;
        [BoxGroup("Performance Defaults")] public int defaultDlssQualityIndex = 0;
        [BoxGroup("Performance Defaults")] public int defaultFsrQualityIndex = 0;
        [BoxGroup("Performance Defaults")] public int defaultTargetFps = 1;

        [BoxGroup("Setting Keys")] private const string TextureResolutionIndexKey = "TextureResolution";
        [BoxGroup("Setting Keys")] private const string AntiAliasingResolutionIndexKey = "AntiAliasingResolution";
        [BoxGroup("Setting Keys")] private const string QualityPresetNameKey = "QualityPreset";
        [BoxGroup("Setting Keys")] private const string TerrainDetailLevelKey = "TerrainDetail";
        [BoxGroup("Setting Keys")] private const string EnableShadowsKey = "EnableShadows";
        [BoxGroup("Setting Keys")] private const string AntiAliasingModeKey = "AntiAliasingMode";
        [BoxGroup("Setting Keys")] private const string VSyncKey = "VSync";
        [BoxGroup("Setting Keys")] private const string ResolutionConfigurationKey = "EnableDLSS";
        [BoxGroup("Setting Keys")] private const string DlssQualityIndexKey = "DlssQuality";
        [BoxGroup("Setting Keys")] private const string FsrQualityIndexKey = "FsrQuality";
        [BoxGroup("Setting Keys")] private const string TargetFpsIndexKey = "TargetFps";

        [BoxGroup("Core Game Objects")] public Light mainDirectionalLight;
        [BoxGroup("Core Game Objects")] public Camera mainCamera;
        public int TextureResolutionIndex { get; set; }
        public int AntiAliasingResolutionIndex { get; set; }
        public string QualityPresetName { get; set; }
        public float TerrainDetailLevel { get; set; }
        public bool EnableShadows { get; set; }
        public int AntiAliasingModeIndex { get; set; }
        public bool VSync { get; set; }
        public int ResolutionConfigurationIndex {get; set; }
        public int DlssQualityIndex { get; set; }
        public int FsrQualityIndex { get; set; }
        public int TargetFpsIndex { get; set; }

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

        public int[] TargetFpsOptions = { 30, 60, 90, 120, 144 };

        /// <summary>
        /// Set up the component
        /// </summary>
        public override void Awake()
        {

            base.Awake();
        }

        public override void Start()
        {
            if (!mainCamera)
            {
                mainCamera = PlayerCameraManager.Instance.MainCamera;
            }

            if (!mainDirectionalLight)
            {
                mainDirectionalLight = PlayerCameraManager.Instance.MainDirectionalLight;
            }

            base.Start();
        }

        /// <summary>
        /// Save settings to PlayerPrefs
        /// </summary>
        public override void SaveSettings()
        {
            SettingsUtils.SaveIntSetting(TextureResolutionIndexKey, TextureResolutionIndex);
            SettingsUtils.SaveIntSetting(AntiAliasingResolutionIndexKey, AntiAliasingResolutionIndex);
            SettingsUtils.SaveStringSetting(QualityPresetNameKey, QualityPresetName);
            SettingsUtils.SaveFloatSetting(TerrainDetailLevelKey, TerrainDetailLevel);
            SettingsUtils.SaveBoolSetting(EnableShadowsKey, EnableShadows);
            SettingsUtils.SaveIntSetting(AntiAliasingModeKey, AntiAliasingModeIndex);
            SettingsUtils.SaveBoolSetting(VSyncKey, VSync);
            SettingsUtils.SaveIntSetting(ResolutionConfigurationKey, ResolutionConfigurationIndex);
            SettingsUtils.SaveIntSetting(DlssQualityIndexKey, DlssQualityIndex);
            SettingsUtils.SaveIntSetting(FsrQualityIndexKey, FsrQualityIndex);
            SettingsUtils.SaveIntSetting(TargetFpsIndexKey, TargetFpsIndex);
            base.SaveSettings();
        }

        /// <summary>
        /// Load settings from PlayerPrefs
        /// </summary>
        public override void LoadSettings()
        {
            TextureResolutionIndex = SettingsUtils.LoadIntSetting(TextureResolutionIndexKey, defaultTextureResolutionIndex);
            AntiAliasingResolutionIndex = SettingsUtils.LoadIntSetting(AntiAliasingResolutionIndexKey, defaultAntiAliasingResolutionIndex);
            QualityPresetName = SettingsUtils.LoadStringSetting(QualityPresetNameKey, defaultQualityPresetName);
            TerrainDetailLevel = SettingsUtils.LoadFloatSetting(TerrainDetailLevelKey, defaultTerrainDetailLevel);
            EnableShadows = SettingsUtils.LoadBoolSetting(EnableShadowsKey, defaultEnableShadows);
            AntiAliasingModeIndex = SettingsUtils.LoadIntSetting(AntiAliasingModeKey, defaultAntiAliasingModeIndex);
            VSync = SettingsUtils.LoadBoolSetting(VSyncKey, defaultVSync);
            ResolutionConfigurationIndex =
                SettingsUtils.LoadIntSetting(ResolutionConfigurationKey, defaultResolutionConfiguration);
            DlssQualityIndex = SettingsUtils.LoadIntSetting(DlssQualityIndexKey, defaultDlssQualityIndex);
            FsrQualityIndex = SettingsUtils.LoadIntSetting(FsrQualityIndexKey, defaultFsrQualityIndex);
            TargetFpsIndex = SettingsUtils.LoadIntSetting(TargetFpsIndexKey, defaultTargetFps);
            base.LoadSettings();
        }

        /// <summary>
        /// Apply all current settings
        /// </summary>
        public override void ApplySettings()
        {
            ApplyQualityPresets();
            ApplyTerrainDetailLevel();
            ApplyAntiAliasingMode();
            ApplyAntiAliasingResolution();
            ApplyTextureResolution();
            ApplyVSync();
            ApplyResolutionConfiguration();
            ApplyTargetFps();
            ApplyDlssQuality();
            ApplyFsrQuality();
            base.ApplySettings();
        }
        
        /// <summary>
        /// Update and apply changes to shadow enabled
        /// </summary>
        /// <param name="value"></param>
        public void SetEnableShadows(bool value)
        {
            EnableShadows = value;
            ApplyEnableShadows();
        }

        /// <summary>
        /// Updates and applies the Texture Resolution
        /// </summary>
        /// <param name="value"></param>
        public void SetTextureResolution(int value)
        {
            TextureResolutionIndex = value;
            ApplyTextureResolution();
        }

        /// <summary>
        /// Updates and applies Anti Aliasing
        /// </summary>
        /// <param name="value"></param>
        public void SetAntiAliasing(int value)
        {
            AntiAliasingResolutionIndex = value;
            ApplyAntiAliasingResolution();
        }

        /// <summary>
        /// Update and apply antialiasing mode
        /// </summary>
        /// <param name="value"></param>
        public void SetAntiAliasingMode(int value)
        {
            AntiAliasingModeIndex = value;
            ApplyAntiAliasingMode();
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
        /// Sets the Terrain Details
        /// </summary>
        /// <param name="value"></param>
        public void SetTerrainDetailLevel(float value)
        {
            TerrainDetailLevel = value;
            ApplyTerrainDetailLevel();
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
        /// Set the target FPS
        /// </summary>
        /// <param name="value"></param>
        public void SetTargetFps(int value)
        {
            TargetFpsIndex = value;
            ApplyTargetFps();
        }

        /// <summary>
        /// Apply the Texture Resolution
        /// </summary>
        private void ApplyTextureResolution()
        {
            QualitySettings.globalTextureMipmapLimit = TextureResolutionIndex;
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply the Anti Aliasing
        /// </summary>
        private void ApplyAntiAliasingResolution()
        {
            switch(AntiAliasingResolutionIndex)
            {
                case 0:
                    QualitySettings.antiAliasing = 8;
                    break;
                case 1:
                    QualitySettings.antiAliasing = 4;
                    break;
                case 2:
                    QualitySettings.antiAliasing = 0;
                    break;
            }
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply the Anti Aliasing Mode
        /// </summary>
        private void ApplyAntiAliasingMode()
        {
#if UNITY_POST_PROCESSING_STACK_V2
            if (mainCamera)
            {
                PostProcessLayer postProcess = mainCamera.GetComponent<PostProcessLayer>();
                if (!postProcess)
                {
                    return;
                }

                switch (AntiAliasingModeIndex)
                {
                    case 0:
                        postProcess.antialiasingMode = PostProcessLayer.Antialiasing.None;
                        break;
                    case 1:
                        postProcess.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                        postProcess.fastApproximateAntialiasing.fastMode = true;
                        break;
                    case 2:
                        postProcess.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                        break;
                    case 3:
                        postProcess.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                        break;
                }
                onSettingsAppliedEvent.Invoke();
                Debug.Log($"PerformanceSettings: AntiAliasingMode is now {postProcess.antialiasingMode}");
            }
#else
            HDAdditionalCameraData additionalCameraData = mainCamera.GetComponent<HDAdditionalCameraData>();
            additionalCameraData.antialiasing = (HDAdditionalCameraData.AntialiasingMode)AntiAliasingModeIndex;
            Debug.Log($"PerformanceSettings: AntiAliasingMode is now {additionalCameraData.antialiasing}");
#endif
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
            
            // Override presets with what's selected
            // Quality presets change the anti-aliasing and texture resolution. Update those settings appropriately
            ApplyTextureResolution();
            ApplyAntiAliasingResolution();
            
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply the Terrain settings
        /// </summary>
        private void ApplyTerrainDetailLevel()
        {
            Terrain[] allActiveTerrains = Terrain.activeTerrains;
            foreach (Terrain currTerrain in allActiveTerrains)
            {
                currTerrain.detailObjectDensity = TerrainDetailLevel;
            }
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply the shadow enable settings
        /// </summary>
        private void ApplyEnableShadows()
        {
            if(!mainDirectionalLight)
            {
                mainDirectionalLight = GameUtils.FindMainDirectionalLight();
            }
#if HDRPPACKAGE_EXIST
            HDAdditionalLightData hdLightData = mainDirectionalLight.GetComponent<HDAdditionalLightData>();
            if(EnableShadows)
            {
                hdLightData.SetShadowUpdateMode(ShadowUpdateMode.EveryFrame);
            }
            else
            {
                hdLightData.SetShadowUpdateMode(ShadowUpdateMode.OnEnable);
            }
#else
            if (mainDirectionalLight)
            {
                if (EnableShadows)
                {
                    mainDirectionalLight.shadows = LightShadows.Hard;
                }
                else
                {
                    mainDirectionalLight.shadows = LightShadows.None;
                }
            }
#endif
            onSettingsAppliedEvent.Invoke();      
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
        /// Applies target framerate settings
        /// </summary>
        private void ApplyTargetFps()
        {
            Application.targetFrameRate = TargetFpsOptions[TargetFpsIndex];
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
                    ApplyAntiAliasingMode();
                    ApplyAntiAliasingResolution();
                    break;
                case DynamicResolutionType.DLSS:
                    HDCam.allowDynamicResolution = true;
                    HDCam.allowDeepLearningSuperSampling = true;
                    HDCam.deepLearningSuperSamplingUseCustomQualitySettings = true;
                    HDCam.deepLearningSuperSamplingQuality = mode.QualityMode;
                    break;
                case DynamicResolutionType.FSR:
                    HDCam.allowDynamicResolution = true;
                    HDCam.allowDeepLearningSuperSampling = false;
                    break;
            }
        }

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
            onSettingsAppliedEvent.Invoke();
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
            onSettingsAppliedEvent.Invoke();
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
            ApplyResolutionConfigurationToCamera();
            Configurations[ResolutionConfigurationIndex].DynamicResolutionScale = GetFSRDynamicScale(Configurations[ResolutionConfigurationIndex].fsrQuality);
            ApplyResolution();
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
    }
}