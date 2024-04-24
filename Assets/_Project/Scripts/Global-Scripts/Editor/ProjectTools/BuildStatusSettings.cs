using System;
using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace DaftAppleGames.Editor.ProjectTools
{
    public enum BuildStage { Prototype, Alpha, Beta, EarlyAccess, Live }
    public enum BuildState { Success, SuccessWithWarnings, SuccessWithErrors, Failed }
    public enum DeployState { Success, Failed }

    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "BuildStatusSettings", menuName = "Daft Apple Games/Project/Build status settings", order = 1)]
    public class BuildStatusSettings : ScriptableObject
    {
        [BoxGroup("Build Settings")] public string buildName = "Darskerry Redemption";
        [BoxGroup("Build Settings")] public string buildOutputPath = "C:\\Games\\Darskerry Redemption\\Darskerry Redemption.exe";
        [BoxGroup("Build Settings")] public AdditiveSceneLoaderSettings gameScenes;
        [BoxGroup("Build Settings")] public AdditiveSceneLoaderSettings mainMenuScenes;
        [BoxGroup("Build Settings")] public string gameLightingScene = "GameLightingScene";
        [BoxGroup("Build Settings")] public string menuLightingScene = "MainMenuLightingScene";
        [BoxGroup("Build Settings")] public string gameTerrainScene = "GameWorldTerrainScene";
        [BoxGroup("Build Settings")] public string menuTerrainScene = "MainMenuTerrainScene";

        [BoxGroup("Deploy Settings")] public string itchDeployBatchFullPath;
        [BoxGroup("Deploy Settings")] public string itchDeployAppName;
        [BoxGroup("Deploy Settings")] public string itchDeployAppStage;

        [BoxGroup("Build Status")] public CustomVersion buildVersion;
        [BoxGroup("Build Status")] public CustomDateTime lastSuccessfulBuild;
        [BoxGroup("Build Status")] public CustomDateTime lastBuildAttempt;
        [BoxGroup("Build Status")] public BuildState lastBuildState;
        [BoxGroup("Build Status")] public string lastBuildLog;

        [BoxGroup("Deploy Status")] public CustomDateTime lastSuccessfulDeploy;
        [BoxGroup("Deploy Status")] public DeployState lastDeployState;
        [BoxGroup("Deploy Status")] public string lastDeployLog;
        [BoxGroup("Build Components")] public CustomDateTime lightingLastBake;
        [BoxGroup("Build Components")] public CustomDateTime navMeshLastBake;
        [BoxGroup("Build Components")] public CustomDateTime occlusionCullingLastBake;

        [BoxGroup("Manual Overrides")] public int majorVersionOverride;
        [BoxGroup("Manual Overrides")] public int minorVersionOverride;
        [BoxGroup("Manual Overrides")] public int patchVersionOverride;
        [BoxGroup("Manual Overrides")] public BuildStage buildStageOverride;

        [Button("Set Version")]
        private void SetVersion()
        {
            buildVersion.MajorVersionNumber = majorVersionOverride;
            buildVersion.MinorVersionNumber = minorVersionOverride;
            buildVersion.PatchVersionNumber = patchVersionOverride;
            buildVersion.BuildStage = buildStageOverride;
        }
    }

    [Serializable]
    public class CustomDateTime
    {
        public int Day;
        public int Month;
        public int Year;
        public int Hour;
        public int Minute;
        public int Second;

        /// <summary>
        /// Default constructor returns current date and time
        /// </summary>
        public CustomDateTime()
        {
            SetNow();
        }

        /// <summary>
        /// Set the given date and time to now
        /// </summary>
        public void SetNow()
        {
            DateTime now = DateTime.Now;
            Day = now.Day;
            Month = now.Month;
            Year = now.Year;
            Hour = now.Hour;
            Minute = now.Minute;
            Second = now.Second;
        }

        /// <summary>
        /// Returns a string version of the Date and Time
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Day:D2}/{Month:D2}/{Year:D2} {Hour:D2}:{Minute:D2}:{Second:D2}";
        }
    }

    [Serializable]
    public class CustomVersion
    {
        public int MajorVersionNumber
        {
            set
            {
                _majorVersionNumber = value;
                PlayerSettings.bundleVersion = ToString();
            }
            get => _majorVersionNumber;
        }

        public int MinorVersionNumber
        {
            set
            {
                _minorVersionNumber = value;
                PlayerSettings.bundleVersion = ToString();
            }
            get => _minorVersionNumber;
        }
        public int PatchVersionNumber
        {
            set
            {
                _patchVersionNumber = value;
                PlayerSettings.bundleVersion = ToString();
            }
            get => _patchVersionNumber;

        }

        public BuildStage BuildStage
        {
            set
            {
                _buildStage = value;
                PlayerSettings.bundleVersion = ToString();
            }
            get => _buildStage;
        }

        private int _majorVersionNumber;
        private int _minorVersionNumber;
        private int _patchVersionNumber;
        private BuildStage _buildStage;

        public CustomVersion(int majorVersionNumber, int minorVersionNumber, int patchVersionNumber, BuildStage buildStage)
        {
            MajorVersionNumber = majorVersionNumber;
            MinorVersionNumber = minorVersionNumber;
            PatchVersionNumber = patchVersionNumber;
            BuildStage = buildStage;
        }

        /// <summary>
        /// Sets the version number based on todays date, and sets the build stage
        /// </summary>
        /// <param name="buildStage"></param>
        public void SetVersionByDate(BuildStage buildStage)
        {
            SetVersionByDate();
            BuildStage = buildStage;
        }

        /// <summary>
        /// Sets the version number based on todays date, incrementing the patch version at the beginning of each month
        /// </summary>
        public void SetVersionByDate()
        {
            DateTime now = DateTime.Now;

            MajorVersionNumber = now.Year;
            if (now.Month != MinorVersionNumber)
            {
                PatchVersionNumber = 1;
                MinorVersionNumber = now.Month;
            }
            else
            {
                PatchVersionNumber++;
            }
        }

        /// <summary>
        /// Returns a string representation of the build version.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{MajorVersionNumber}.{MinorVersionNumber:D2}.{PatchVersionNumber} ({BuildStage.ToString()})";
        }

        /// <summary>
        /// Increments the Major version
        /// </summary>
        public void IncrementMajorVersion()
        {
            MajorVersionNumber++;
        }

        /// <summary>
        /// Increments the Minor version
        /// </summary>
        public void IncrementMinorVersion()
        {
            MinorVersionNumber++;
        }

        /// <summary>
        /// Increments the Patch version
        /// </summary>
        public void IncrementPatchVersion()
        {
            PatchVersionNumber++;
        }
    }
}