using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;

namespace DaftAppleGames.Darskerry.Editor.BuildTool
{
    [Serializable]
    public class BuildTargetSettings
    {
        [BoxGroup("Build Settings")] public BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
        [BoxGroup("Build Settings")] public BuildOptions releaseBuildOptions = BuildOptions.CompressWithLz4HC;
        [BoxGroup("Build Settings")] public BuildOptions devBuildOptions = BuildOptions.Development &
                                                                             BuildOptions.AllowDebugging &
                                                                             BuildOptions.ConnectWithProfiler &
                                                                             BuildOptions.EnableDeepProfilingSupport;
        [BoxGroup("Build Settings")] public string gameFolder = @"E:\Dev\DAG\Itch Builds\Retro Racket Revolution\Retro Racket Revolution Windows";
        [BoxGroup("Build Settings")] public string fileName = @"Retro Racket Revolution.exe";

        [BoxGroup("Deploy Settings")] public string itchDeployBatchFullPath;
        [BoxGroup("Deploy Settings")] public string itchDeployAppName;
        [BoxGroup("Deploy Settings")] public string itchDeployAppStage;

        public string FullPath => Path.Join(gameFolder, fileName);
    }
}