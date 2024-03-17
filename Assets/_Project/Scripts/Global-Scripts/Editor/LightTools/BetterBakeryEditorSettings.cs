#if BAKERY_INCLUDED
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Editor.Common.LightTools
{
    public enum BakeryRenderMode { FullLighting, Indirect, Shadowmask, Subtractive, AmbientOcclusionOnly }
    public enum BakeryDirectionalMode { None, BakedNormalMaps, DominantDirection, RNM, SH, MonoSH }
    
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "BetterBakeryEditorSettings", menuName = "LightTools/Better Bakery Editor Settings", order = 1)]
    public class BetterBakeryEditorSettings : ScriptableObject
    {
        [BoxGroup("Render Mode")]
        public BakeryRenderMode UserRenderMode = BakeryRenderMode.FullLighting;
        [BoxGroup("Render Mode")]
        public BakeryDirectionalMode RenderDirMode = BakeryDirectionalMode.None;
        
        [BoxGroup("Light Mapping Tasks")]
        public bool ForceRebuildGeometry = true;
        [BoxGroup("Light Mapping Tasks")]
        public bool AdjustSamples = true;
        [BoxGroup("Light Mapping Tasks")]
        public bool UnloadScenes = true;
        [BoxGroup("Light Mapping Tasks")]
        public bool ForceRefresh = true;
        [BoxGroup("Light Mapping Tasks")]
        public bool Denoise = true;
        [BoxGroup("Light Mapping Tasks")]
        public bool Denoise2x = false;
        [BoxGroup("Light Mapping Tasks")]
        public bool FixSeams = true;
        
        [BoxGroup("Auto Atlasing")]
        public bool SplitByScene = false;
        [BoxGroup("Auto Atlasing")]
        public bool SplitByTag = false;
        [BoxGroup("Auto Atlasing")]        
        public bool UVPaddingMax = false;
        
        [FoldoutGroup("Advanced Settings")]public int Bounces = 5;
        [FoldoutGroup("Advanced Settings")]public int GISamples = 16;
        [FoldoutGroup("Advanced Settings")]public float GIBackFaceWeight = 0;
        [FoldoutGroup("Advanced Settings")]public int TileSize = 512;
        [FoldoutGroup("Advanced Settings")]public float Priority = 2;
        [FoldoutGroup("Advanced Settings")]public float TexelsPerUnit = 20;
        [FoldoutGroup("Advanced Settings")]public bool PerformRendering = true;

        [FoldoutGroup("Advanced Settings")]public bool DistanceShadowmask = false;
        [FoldoutGroup("Advanced Settings")]public int SettingsMode = 0;
        [FoldoutGroup("Advanced Settings")]public bool Encode = true;
        [FoldoutGroup("Advanced Settings")]public int EncodeMode = 0;
        [FoldoutGroup("Advanced Settings")]public bool OverwriteWarning = false;
        [FoldoutGroup("Advanced Settings")]public bool AutoAtlas = true;
        [FoldoutGroup("Advanced Settings")]public bool UnwrapUVs = true;
        [FoldoutGroup("Advanced Settings")]public bool ForceDisableUnwrapUVs = false;
        [FoldoutGroup("Advanced Settings")]public int MaxAutoResolution = 4096;
        [FoldoutGroup("Advanced Settings")]public int MinAutoResolution = 16;
        [FoldoutGroup("Advanced Settings")]public int GILODMode = 2;
        [FoldoutGroup("Advanced Settings")]public bool GILODModeEnabled = false;
        [FoldoutGroup("Advanced Settings")]public bool CheckOverlaps = false;
        [FoldoutGroup("Advanced Settings")]public bool SkipOutOfBoundsUVs = true;
        [FoldoutGroup("Advanced Settings")]public float HackEmissiveBoost = 1;
        [FoldoutGroup("Advanced Settings")]public float HackIndirectBoost = 1;
        [FoldoutGroup("Advanced Settings")]public string TempPath = "";
        [FoldoutGroup("Advanced Settings")]public string OutPath = "";
        [FoldoutGroup("Advanced Settings")]public bool UseScenePath = false;
        [FoldoutGroup("Advanced Settings")]public float HackAOIntensity = 0;
        [FoldoutGroup("Advanced Settings")]public int HackAOSamples = 16;
        [FoldoutGroup("Advanced Settings")]public float HackAORadius = 1;
        [FoldoutGroup("Advanced Settings")]public bool ShowAOSettings = false;
        [FoldoutGroup("Advanced Settings")]public bool ShowTasks = true;
        [FoldoutGroup("Advanced Settings")]public bool ShowTasks2 = false;
        [FoldoutGroup("Advanced Settings")]public bool ShowPaths = true;
        [FoldoutGroup("Advanced Settings")]public bool ShowNet = true;
        [FoldoutGroup("Advanced Settings")]public bool OcclusionProbes = false;
        [FoldoutGroup("Advanced Settings")]public bool TexelsPerMap = false;
        [FoldoutGroup("Advanced Settings")]public float TexelsColor = 1;
        [FoldoutGroup("Advanced Settings")]public float TexelsMask = 1;
        [FoldoutGroup("Advanced Settings")]public float TexelsDir = 1;
        [FoldoutGroup("Advanced Settings")]public bool ShowDirWarning = true;

        [FoldoutGroup("Advanced Settings")]public bool ShowCheckerSettings = false;
        [FoldoutGroup("Advanced Settings")]public bool SamplesWarning = true;
        [FoldoutGroup("Advanced Settings")]public bool SuppressPopups = false;
        [FoldoutGroup("Advanced Settings")]public bool PrefabWarning = true;
        [FoldoutGroup("Advanced Settings")]public bool PostPacking = true;
        [FoldoutGroup("Advanced Settings")]public bool HoleFilling = false;
        [FoldoutGroup("Advanced Settings")]public bool BeepOnFinish = false;
        [FoldoutGroup("Advanced Settings")]public bool ExportTerrainAsHeightmap = true;
        [FoldoutGroup("Advanced Settings")]public bool RTXMode = false;
        [FoldoutGroup("Advanced Settings")]public int LightProbeMode = 1;
        [FoldoutGroup("Advanced Settings")]public bool ClientMode = false;
        [FoldoutGroup("Advanced Settings")]public string ServerAddress = "127.0.0.1";
        [FoldoutGroup("Advanced Settings")]public int Unwrapper = 0;
        [FoldoutGroup("Advanced Settings")]public int DenoiserType = (int)ftGlobalStorage.DenoiserType.OpenImageDenoise;
        [FoldoutGroup("Advanced Settings")]public bool ExportTerrainTrees = false;
        [FoldoutGroup("Advanced Settings")]public bool ShowPerf = true;
        [FoldoutGroup("Advanced Settings")]public int SampleDiv = 1;
        [FoldoutGroup("Advanced Settings")]public ftGlobalStorage.AtlasPacker AtlasPacker = ftGlobalStorage.AtlasPacker.xatlas;
        [FoldoutGroup("Advanced Settings")]public bool BatchPoints = true;
        [FoldoutGroup("Advanced Settings")]public bool CompressVolumes = false;
        [FoldoutGroup("Advanced Settings")]public UnityEngine.Object Sector = null;
        [FoldoutGroup("Advanced Settings")]public bool RTPVExport = true;
        [FoldoutGroup("Advanced Settings")]public bool RTPVSceneView = false;
        [FoldoutGroup("Advanced Settings")]public bool RTPVHDR = false;
        [FoldoutGroup("Advanced Settings")]public int RTPVWidth = 640;
        [FoldoutGroup("Advanced Settings")]public int RTPVHeight = 360;

    }
}
#endif