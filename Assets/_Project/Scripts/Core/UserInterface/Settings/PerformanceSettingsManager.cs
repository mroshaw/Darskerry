using Sirenix.OdinInspector;
using UnityEngine;

using UnityEngine.Rendering.HighDefinition;
#if GPU_INSTANCER
using System.Collections.Generic;
using GPUInstancer;
#endif

namespace DaftAppleGames.Darskerry.Core.UserInterface.Settings
{
    public class PerformanceSettingsManager : BaseSettingsManager, ISettings
    {
        [BoxGroup("Defaults")] public int defaultTextureResolutionIndex = 0;
        [BoxGroup("Defaults")] public int defaultAntiAliasingResolutionIndex = 0;
        [BoxGroup("Defaults")] public float defaultTerrainDetailLevel = 2.0f;
        [BoxGroup("Defaults")] public bool defaultEnableShadows = true;
        [BoxGroup("Defaults")] public int defaultAntiAliasingModeIndex = 1;
        [BoxGroup("Defaults")] public int defaultTargetFps = 1;
        [BoxGroup("Defaults")] public bool defaultEnableSSR = true;
        [BoxGroup("Defaults")] public bool defaultEnableSSRTransparent = true;
        [BoxGroup("Defaults")] public bool defaultEnableSSGI = true;
        [BoxGroup("Defaults")] public bool defaultEnableSSAO = true;
        [BoxGroup("Defaults")] public bool defaultEnableOverrides = false;
        [BoxGroup("Setting Keys")] private const string TextureResolutionIndexKey = "TextureResolution";
        [BoxGroup("Setting Keys")] private const string AntiAliasingResolutionIndexKey = "AntiAliasingResolution";
        [BoxGroup("Setting Keys")] private const string TerrainDetailLevelKey = "TerrainDetail";
        [BoxGroup("Setting Keys")] private const string EnableShadowsKey = "EnableShadows";
        [BoxGroup("Setting Keys")] private const string AntiAliasingModeKey = "AntiAliasingMode";
        [BoxGroup("Setting Keys")] private const string TargetFpsIndexKey = "TargetFps";
        [BoxGroup("Setting Keys")] private const string EnableSSRKey = "EnableSSR";
        [BoxGroup("Setting Keys")] private const string EnableSSRTransparentKey = "EnableSSRTransparent";
        [BoxGroup("Setting Keys")] private const string EnableSSGIKey = "EnableSSRTransparent";
        [BoxGroup("Setting Keys")] private const string EnableSSAOKey = "EnableSSAO";
        [BoxGroup("Setting Keys")] private const string EnableOverridesKey = "EnableOverrides";

        [BoxGroup("Core Game Objects")] public Light mainDirectionalLight;
        [BoxGroup("Core Game Objects")] public Camera mainCamera;

        private HDAdditionalCameraData _mainCameraSettings;

        public int TextureResolutionIndex { get; set; }
        public int AntiAliasingResolutionIndex { get; set; }
        public float TerrainDetailLevel { get; set; }
        public bool EnableShadows { get; set; }
        public int AntiAliasingModeIndex { get; set; }
        public int TargetFpsIndex { get; set; }
        public bool EnableSSR { get; set; }
        public bool EnableSSRTransparent { get; set; }
        public bool EnableSSGI { get; set; }
        public bool EnableSSAO { get; set; }
        public bool EnableOverrides { get; set; }
        public int[] TargetFpsOptions = { 30, 60 };

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
                mainCamera = Camera.main;
                if (mainCamera)
                {
                    _mainCameraSettings = mainCamera.GetComponent<HDAdditionalCameraData>();
                    _mainCameraSettings.customRenderingSettings = true;
                }
            }

            if (!mainDirectionalLight)
            {
                // mainDirectionalLight = PlayerCameraManager.Instance.MainDirectionalLight;
            }

            base.Start();
        }

        /// <summary>
        /// Setup any lists or arrays
        /// </summary>
        public override void InitSettings()
        {
            // Populate the "Screen Resolution" list
        }

        /// <summary>
        /// Save settings to PlayerPrefs
        /// </summary>
        public override void SaveSettings()
        {
            SettingsUtils.SaveIntSetting(TextureResolutionIndexKey, TextureResolutionIndex);
            SettingsUtils.SaveIntSetting(AntiAliasingResolutionIndexKey, AntiAliasingResolutionIndex);
            SettingsUtils.SaveFloatSetting(TerrainDetailLevelKey, TerrainDetailLevel);
            SettingsUtils.SaveBoolSetting(EnableShadowsKey, EnableShadows);
            SettingsUtils.SaveIntSetting(AntiAliasingModeKey, AntiAliasingModeIndex);
            SettingsUtils.SaveIntSetting(TargetFpsIndexKey, TargetFpsIndex);
            SettingsUtils.SaveBoolSetting(EnableSSRKey, EnableSSR);
            SettingsUtils.SaveBoolSetting(EnableSSRTransparentKey, EnableSSRTransparent);
            SettingsUtils.SaveBoolSetting(EnableSSGIKey, EnableSSGI);
            SettingsUtils.SaveBoolSetting(EnableSSAOKey, EnableSSAO);
            SettingsUtils.SaveBoolSetting(EnableOverridesKey, EnableOverrides);
            base.SaveSettings();
        }

        /// <summary>
        /// Load settings from PlayerPrefs
        /// </summary>
        public override void LoadSettings()
        {
            TextureResolutionIndex =
                SettingsUtils.LoadIntSetting(TextureResolutionIndexKey, defaultTextureResolutionIndex);
            AntiAliasingResolutionIndex =
                SettingsUtils.LoadIntSetting(AntiAliasingResolutionIndexKey, defaultAntiAliasingResolutionIndex);

            TerrainDetailLevel = SettingsUtils.LoadFloatSetting(TerrainDetailLevelKey, defaultTerrainDetailLevel);
            EnableShadows = SettingsUtils.LoadBoolSetting(EnableShadowsKey, defaultEnableShadows);
            AntiAliasingModeIndex = SettingsUtils.LoadIntSetting(AntiAliasingModeKey, defaultAntiAliasingModeIndex);
            TargetFpsIndex = SettingsUtils.LoadIntSetting(TargetFpsIndexKey, defaultTargetFps);
            EnableSSR = SettingsUtils.LoadBoolSetting(EnableSSRKey, defaultEnableSSR);
            EnableSSRTransparent = SettingsUtils.LoadBoolSetting(EnableSSRTransparentKey, defaultEnableSSRTransparent);
            EnableSSGI = SettingsUtils.LoadBoolSetting(EnableSSGIKey, defaultEnableSSGI);
            EnableSSAO = SettingsUtils.LoadBoolSetting(EnableSSAOKey, defaultEnableSSAO);
            EnableOverrides = SettingsUtils.LoadBoolSetting(EnableOverridesKey, defaultEnableOverrides);
            base.LoadSettings();
        }

        /// <summary>
        /// Apply all current settings
        /// </summary>
        public override void ApplySettings()
        {
            if (EnableOverrides)
            {
                ApplyTerrainDetailLevel();
                ApplyAntiAliasingMode();
                ApplyAntiAliasingResolution();
                ApplyTextureResolution();
                ApplyTargetFps();
                ApplySSR();
                ApplySSRTransparent();
                ApplySSGI();
                ApplySSAO();
            }
            base.ApplySettings();
        }

        /// <summary>
        /// Set the advanced overrides
        /// </summary>
        /// <param name="value"></param>
        public void SetOverrides(bool value)
        {
            EnableOverrides = value;
            ApplySettings();
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
        /// Sets the Terrain Details
        /// </summary>
        /// <param name="value"></param>
        public void SetTerrainDetailLevel(float value)
        {
            TerrainDetailLevel = value;
            ApplyTerrainDetailLevel();
        }


        /// <summary>
        /// Set the SSR Transparent state
        /// </summary>
        /// <param name="value"></param>
        public void SetSSRTransparent(bool value)
        {
            EnableSSRTransparent = value;
            ApplySSRTransparent();
        }

        /// <summary>
        /// Set the SSGI state
        /// </summary>
        /// <param name="value"></param>
        public void SetSSGI(bool value)
        {
            EnableSSGI = value;
            ApplySSGI();
        }

        /// <summary>
        /// Set the SSAO state
        /// </summary>
        /// <param name="value"></param>
        public void SetSSAO(bool value)
        {
            EnableSSAO = value;
            ApplySSAO();
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
        /// Set the SSR state
        /// </summary>
        private void ApplySSR()
        {
            SetFrameSettingState(FrameSettingsField.SSR, EnableSSR);
            SetFrameSettingState(FrameSettingsField.TransparentSSR, EnableSSRTransparent);

            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Set the SSR Transparent state
        /// </summary>
        private void ApplySSRTransparent()
        {
            SetFrameSettingState(FrameSettingsField.TransparentSSR, EnableSSRTransparent);
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Set the Global GI state
        /// </summary>
        private void ApplySSGI()
        {
            SetFrameSettingState(FrameSettingsField.SSGI, EnableSSGI);
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Set the SAO state
        /// </summary>
        private void ApplySSAO()
        {
            SetFrameSettingState(FrameSettingsField.SSAO, EnableSSAO);
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Sets the given boolean custom frame setting to the given state
        /// </summary>
        /// <param name="field"></param>
        /// <param name="state"></param>
        private void SetFrameSettingState(FrameSettingsField field, bool state)
        {
            // Enable custom frame settings
            HDAdditionalCameraData cameraData = mainCamera.GetComponent<HDAdditionalCameraData>();
            cameraData.customRenderingSettings = true;

            // Toggle SSGI in frame settings
            FrameSettings customFrameSettings;
            customFrameSettings = cameraData.renderingPathCustomFrameSettings;
            customFrameSettings.SetEnabled(field, state);

            // Apply frame settings to Main Camera
            cameraData.renderingPathCustomFrameSettings = customFrameSettings;
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
        /// Set the SSR state
        /// </summary>
        /// <param name="value"></param>
        public void SetSSR(bool value)
        {
            EnableSSR = value;
            ApplySSR();
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
        }

        /// <summary>
        /// Apply the shadow enable settings
        /// </summary>
        private void ApplyEnableShadows()
        {
            if(!mainDirectionalLight)
            {
                // mainDirectionalLight = GameUtils.FindMainDirectionalLight();
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
        }

        /// <summary>
        /// Applies target framerate settings
        /// </summary>
        private void ApplyTargetFps()
        {
            Application.targetFrameRate = TargetFpsOptions[TargetFpsIndex];
        }
    }
}