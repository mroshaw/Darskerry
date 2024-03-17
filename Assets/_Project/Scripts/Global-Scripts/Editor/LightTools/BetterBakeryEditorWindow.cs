#if BAKERY_INCLUDED
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

using UnityEngine;

namespace DaftAppleGames.Editor.Common.LightTools
{
    public class BetterBakeryEditorWindow : OdinEditorWindow
    {
        [MenuItem("Window/Lighting/Better Bakery Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BetterBakeryEditorWindow));
        }
    
        // UI layout
        [InlineEditor()]
        [FoldoutGroup("Bakery Settings")]
        public BetterBakeryEditorSettings bakerySettings;
    
        [Button("RENDER", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void RenderButton()
        {
            ftLightmapsStorage storage = ftRenderLightmap.FindRenderSettingsStorage();
            ApplySettings(storage, bakerySettings);
            
            ftRenderLightmap bakery = ftRenderLightmap.instance != null ? ftRenderLightmap.instance : new ftRenderLightmap(); bakery.LoadRenderSettings();
            bakery.RenderButton();
        }

        /// <summary>
        /// Copy settings to storage
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="settings"></param>
        private void ApplySettings(ftLightmapsStorage storage, BetterBakeryEditorSettings settings)
        {
            storage.renderSettingsBounces = settings.Bounces;
            storage.renderSettingsGISamples = settings.GISamples;
            storage.renderSettingsGIBackFaceWeight = settings.GIBackFaceWeight;
            storage.renderSettingsTileSize = settings.TileSize;
            storage.renderSettingsPriority = settings.Priority;
            storage.renderSettingsTexelsPerUnit = settings.TexelsPerUnit;
            storage.renderSettingsForceRefresh = settings.ForceRefresh;
            storage.renderSettingsForceRebuildGeometry = settings.ForceRebuildGeometry;
            storage.renderSettingsPerformRendering = settings.PerformRendering;
            storage.renderSettingsUserRenderMode = (int)settings.UserRenderMode;
            storage.renderSettingsDistanceShadowmask = settings.DistanceShadowmask;
            storage.renderSettingsSettingsMode = settings.SettingsMode;
            storage.renderSettingsFixSeams = settings.FixSeams;
            storage.renderSettingsDenoise = settings.Denoise;
            storage.renderSettingsDenoise2x = settings.Denoise2x;
            storage.renderSettingsEncode = settings.Encode;
            storage.renderSettingsEncodeMode = settings.EncodeMode;
            storage.renderSettingsOverwriteWarning = settings.OverwriteWarning;
            storage.renderSettingsAutoAtlas = settings.AutoAtlas;
            storage.renderSettingsUnwrapUVs = settings.UnwrapUVs;
            storage.renderSettingsForceDisableUnwrapUVs = settings.ForceDisableUnwrapUVs;
            storage.renderSettingsMaxAutoResolution = settings.MaxAutoResolution;
            storage.renderSettingsMinAutoResolution = settings.MinAutoResolution;
            storage.renderSettingsUnloadScenes = settings.UnloadScenes;
            storage.renderSettingsAdjustSamples = settings.AdjustSamples;
            storage.renderSettingsGILODMode = settings.GILODMode;
            storage.renderSettingsGILODModeEnabled = settings.GILODModeEnabled;
            storage.renderSettingsCheckOverlaps = settings.CheckOverlaps;
            storage.renderSettingsSkipOutOfBoundsUVs = settings.SkipOutOfBoundsUVs;
            storage.renderSettingsHackEmissiveBoost = settings.HackEmissiveBoost;
            storage.renderSettingsHackIndirectBoost = settings.HackIndirectBoost;
            storage.renderSettingsTempPath = settings.TempPath;
            storage.renderSettingsOutPath = settings.OutPath;
            storage.renderSettingsUseScenePath = settings.UseScenePath;
            storage.renderSettingsHackAOIntensity = settings.HackAOIntensity;
            storage.renderSettingsHackAOSamples = settings.HackAOSamples;
            storage.renderSettingsHackAORadius = settings.HackAORadius;
            storage.renderSettingsShowAOSettings = settings.ShowAOSettings;
            storage.renderSettingsShowTasks = settings.ShowTasks;
            storage.renderSettingsShowTasks2 = settings.ShowTasks2;
            storage.renderSettingsShowPaths = settings.ShowPaths;
            storage.renderSettingsShowNet = settings.ShowNet;
            storage.renderSettingsOcclusionProbes = settings.OcclusionProbes;
            storage.renderSettingsTexelsPerMap = settings.TexelsPerMap;
            storage.renderSettingsTexelsColor = settings.TexelsColor;
            storage.renderSettingsTexelsMask = settings.TexelsMask;
            storage.renderSettingsTexelsDir = settings.TexelsDir;
            storage.renderSettingsShowDirWarning = settings.ShowDirWarning;
            storage.renderSettingsRenderDirMode = (int)settings.RenderDirMode;
            storage.renderSettingsShowCheckerSettings = settings.ShowCheckerSettings;
            storage.renderSettingsSamplesWarning = settings.SamplesWarning;
            storage.renderSettingsSuppressPopups = settings.SuppressPopups;
            storage.renderSettingsPrefabWarning = settings.PrefabWarning;
            storage.renderSettingsSplitByScene = settings.SplitByScene;
            storage.renderSettingsSplitByTag = settings.SplitByTag;
            storage.renderSettingsUVPaddingMax = settings.UVPaddingMax;
            storage.renderSettingsPostPacking = settings.PostPacking;
            storage.renderSettingsHoleFilling = settings.HoleFilling;
            storage.renderSettingsBeepOnFinish = settings.BeepOnFinish;
            storage.renderSettingsExportTerrainAsHeightmap = settings.ExportTerrainAsHeightmap;
            storage.renderSettingsRTXMode = settings.RTXMode;
            storage.renderSettingsLightProbeMode = settings.LightProbeMode;
            storage.renderSettingsClientMode = settings.ClientMode;
            storage.renderSettingsServerAddress = settings.ServerAddress;
            storage.renderSettingsUnwrapper = settings.Unwrapper;
            storage.renderSettingsDenoiserType = settings.DenoiserType;
            storage.renderSettingsExportTerrainTrees = settings.ExportTerrainTrees;
            storage.renderSettingsShowPerf = settings.ShowPerf;
            storage.renderSettingsSampleDiv = settings.SampleDiv;
            storage.renderSettingsAtlasPacker = settings.AtlasPacker;
            storage.renderSettingsBatchPoints = settings.BatchPoints;
            storage.renderSettingsCompressVolumes = settings.CompressVolumes;
            storage.renderSettingsSector = settings.Sector;
            storage.renderSettingsRTPVExport = settings.RTPVExport;
            storage.renderSettingsRTPVSceneView = settings.RTPVSceneView;
            storage.renderSettingsRTPVHDR = settings.RTPVHDR;
            storage.renderSettingsRTPVWidth = settings.RTPVWidth;
            storage.renderSettingsRTPVHeight = settings.RTPVHeight;
       }
    }
}
#endif