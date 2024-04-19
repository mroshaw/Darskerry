using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DaftAppleGames.Common.GameControllers;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace DaftAppleGames.Editor.ProjectTools
{
    public class BuildToolsEditorWindow : OdinEditorWindow
    {
        // Display Editor Window
        [MenuItem("Daft Apple Games/Tools/Project/Build tool")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BuildToolsEditorWindow));
        }

        [BoxGroup("Build Status")] [ReadOnly] public string buildVersion;
        [BoxGroup("Build Details")] [ReadOnly] public string lastSuccessfulBuild;
        [BoxGroup("Build Details")] [ReadOnly] public string lastBuildAttempt;
        [BoxGroup("Build Details")] [ReadOnly] public BuildState lastBuildResult;
        [BoxGroup("Deploy Details")] [ReadOnly] public string lastSuccessfulDeploy;
        [BoxGroup("Deploy Details")] [ReadOnly] public DeployState lastDeployState;
        [BoxGroup("Build Components")] [ReadOnly] public string lightingLastBake;
        [BoxGroup("Build Components")] [ReadOnly] public string navMeshLastBake;
        [BoxGroup("Build Components")] [ReadOnly] public string occlusionCullingLastBake;

        [BoxGroup("Build Control")] public BuildStage buildStage = BuildStage.Alpha;
        [BoxGroup("Build Control")] [DisableIf("@IsMinorBuild || IsPatchBuild")] public bool IsMajorBuild;
        [BoxGroup("Build Control")] [DisableIf("@IsMajorBuild || IsPatchBuild")] public bool IsMinorBuild;
        [BoxGroup("Build Control")] [DisableIf("@IsMajorBuild || IsMinorBuild")] public bool IsPatchBuild = true;

        private BuildStatusSettings _buildStatus;

        /// <summary>
        /// Initialise the build status when window is enabled
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            LoadBuildStatus();
        }

        /// <summary>
        /// Loads the latest build status settings
        /// </summary>
        private void LoadBuildStatus()
        {
            _buildStatus = (BuildStatusSettings)AssetDatabase.LoadAssetAtPath("Assets/_Project/Settings/Project/BuildStatusSettings.asset",
                typeof(BuildStatusSettings));

            RefreshBuildStatus();
        }

        /// <summary>
        /// Refresh the window properties
        /// </summary>
        private void RefreshBuildStatus()
        {
            buildVersion = _buildStatus.buildVersion.ToString();
            lastSuccessfulBuild = _buildStatus.lastSuccessfulBuild.ToString();
            lastBuildAttempt = _buildStatus.lastBuildAttempt.ToString();
            lastBuildResult = _buildStatus.lastBuildState;
            lightingLastBake = _buildStatus.lightingLastBake.ToString();
            navMeshLastBake = _buildStatus.navMeshLastBake.ToString();
            occlusionCullingLastBake = _buildStatus.occlusionCullingLastBake.ToString();

            lastSuccessfulDeploy = _buildStatus.lastSuccessfulDeploy.ToString();
            lastDeployState = _buildStatus.lastDeployState;
        }

        [BoxGroup("BUILD")] [Button("BUILD ONLY", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void BuildOnlyButton()
        {
            LoadBuildStatus();
            SaveAllScenes();
            BuildPlayer();
        }

        [BoxGroup("BUILD")] [Button("BAKE AND BUILD", ButtonSizes.Large), GUIColor(1, 1, 0)]
        private void BakeAndBuildButton()
        {
            LoadBuildStatus();
            SaveAllScenes();
            BakeLighting();
            BakeNavMesh();
            BakeOcclusion();
            BuildPlayer();
        }

        /// <summary>
        /// Run the built game executable, to test prior to deployment to itch.io
        /// </summary>
        [BoxGroup("DEPLOY")] [Button("RUN GAME", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void RunGame()
        {            Thread newThread = new Thread(RunGameInThread);
            newThread.Start();

        }

        /// <summary>
        /// Start the deployment process on a new thread, to prevent lock up of the Unity UI
        /// </summary>
        [BoxGroup("DEPLOY")] [Button("DEPLOY TO ITCH", ButtonSizes.Large), GUIColor(1, 0, 0)]
        private void DeployToItch()
        {
            Thread newThread = new Thread(DeployToItchInThread);
            newThread.Start();
        }

        private void RunGameInThread()
        {
            Process process = new Process();

            // Redirect the output stream of the child process.
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = _buildStatus.buildOutputPath;
            int exitCode = -1;
            string output = null;

            try {
                process.Start();

                // do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // process.WaitForExit();

                // read the output stream first and then wait.
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e) {
                Debug.LogError("Run error" + e.ToString()); // or throw new Exception
            }
            finally {
                exitCode = process.ExitCode;

                process.Dispose();
                process = null;
            }
        }

        /// <summary>
        /// Deploy the build to itch.io
        /// </summary>
        private void DeployToItchInThread()
        {
            Process process = new Process();

            // Redirect the output stream of the child process.
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = _buildStatus.itchDeployBatchFullPath;
            process.StartInfo.Arguments = $"{_buildStatus.itchDeployAppName}  {_buildStatus.itchDeployAppStage}";
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(_buildStatus.itchDeployBatchFullPath);

            int exitCode = -1;
            string output = null;

            try {
                process.Start();

                // do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // process.WaitForExit();

                // read the output stream first and then wait.
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e) {
                Debug.LogError("Run error" + e.ToString()); // or throw new Exception
            }
            finally {
                exitCode = process.ExitCode;

                process.Dispose();
                process = null;
            }

            Debug.Log($"Process completed with code: {exitCode}");
            Debug.Log($"Process output: {output}");

            _buildStatus.lastDeployState = exitCode == 0 ? DeployState.Success : DeployState.Failed;
            if (exitCode == 0)
            {
                _buildStatus.lastDeployState = DeployState.Success;
                _buildStatus.lastSuccessfulDeploy.SetNow();

            }
            else
            {
                _buildStatus.lastDeployState = DeployState.Failed;
            }
        }

        [BoxGroup("COMPONENTS")] [Button("Bake Lighting")]
        private void BakeLightsButton()
        {
            BakeLighting();
        }

        [BoxGroup("COMPONENTS")] [Button("Bake NavMesh")]
        private void BakeNavmeshButton()
        {
            BakeNavMesh();
        }

        [BoxGroup("COMPONENTS")] [Button("Bake Occlusion")]
        private void BakeOcclusionButton()
        {
            BakeOcclusion();
        }

        /// <summary>
        /// Saves all open scenes
        /// </summary>
        private void SaveAllScenes()
        {
            int numScenes = EditorSceneManager.sceneCount;
            for (int currSceneIndex = 0; currSceneIndex < numScenes; currSceneIndex++)
            {
                EditorSceneManager.SaveScene(SceneManager.GetSceneAt(currSceneIndex));
            }
        }

        /// <summary>
        /// Call the aSync build player co-coroutine
        /// </summary>
        private void BuildPlayer()
        {
            EditorCoroutineUtility.StartCoroutine(BuildPlayerAsync(), this);
        }

        /// <summary>
        /// Async build player method, so we can monitor progress and update status
        /// </summary>
        /// <returns></returns>
        private IEnumerator BuildPlayerAsync()
        {
            _buildStatus.lastBuildAttempt.SetNow();
            List<string> allScenePaths = new List<string>();
            allScenePaths.AddRange(GetScenePaths(_buildStatus.gameScenes, true));
            allScenePaths.AddRange(GetScenePaths(_buildStatus.mainMenuScenes, true));

            BuildReport buildReport = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,_buildStatus.buildOutputPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
            while (BuildPipeline.isBuildingPlayer)
            {
                yield return null;
            }

            // Determine status of build and update the scriptable object instance and UI
            if (buildReport.summary.result == BuildResult.Succeeded)
            {
                // Set the build state and update the version number
                _buildStatus.lastSuccessfulBuild.SetNow();
                _buildStatus.buildVersion.SetVersionByDate(buildStage);
                if (buildReport.summary is { totalErrors: 0, totalWarnings: 0 })
                {
                    _buildStatus.lastBuildState = BuildState.Success;
                }
                else if (buildReport.summary.totalErrors > 0)
                {
                    _buildStatus.lastBuildState = BuildState.SuccessWithErrors;
                }
                else if (buildReport.summary.totalWarnings > 0)
                {
                    _buildStatus.lastBuildState = BuildState.SuccessWithWarnings;
                }
            }
            else
            {
                _buildStatus.lastBuildState = BuildState.Failed;
            }

            RefreshBuildStatus();
        }

        /// <summary>
        /// Bake all light probes in all open scenes
        /// </summary>
        private void BakeLighting()
        {
            SaveAllScenes();
            EditorCoroutineUtility.StartCoroutine(BakeLightingAsync(), this);
        }

        /// <summary>
        /// Runs and waits for individual async lighting bake jobs across game and main menu scenes
        /// </summary>
        /// <returns></returns>
        private IEnumerator BakeLightingAsync()
        {
            Debug.Log("BuildTools: Bake Lighting...");
            Debug.Log("BuildTools: Bake Lighting in Main Menu scenes...");
            Lightmapping.BakeMultipleScenes(GetScenePathsAsArray(_buildStatus.mainMenuScenes,false));
            while (Lightmapping.isRunning)
            {
                yield return null;
            }
            Debug.Log("BuildTools: Bake Lighting in Game scenes...");
            Lightmapping.BakeMultipleScenes(GetScenePathsAsArray(_buildStatus.gameScenes,false));
            while (Lightmapping.isRunning)
            {
                yield return null;
            }
            _buildStatus.lightingLastBake.SetNow();
            RefreshBuildStatus();
            Debug.Log("BuildTools: Bake Lighting done.");

        }

        /// <summary>
        /// Sets the active scene
        /// </summary>
        /// <param name="sceneName"></param>
        private void SetActiveScene(string sceneName)
        {
            int numScenes = EditorSceneManager.sceneCount;
            Scene currScene;
            for (int currSceneIndex = 0; currSceneIndex < numScenes; currSceneIndex++)
            {
                currScene = SceneManager.GetSceneAt(currSceneIndex);
                if (currScene.name == sceneName)
                {
                    EditorSceneManager.SetActiveScene(currScene);
                }
            }
        }

        /// <summary>
        /// Bake NavMeshs in all open scenes
        /// </summary>
        private void BakeNavMesh()
        {
            Debug.Log("BuildTools: Bake NavMesh...");
            // NavMeshBuilder.BuildNavMeshForMultipleScenes(GetScenePaths());
            NavMeshSurface[] allNavMeshSurfaces = (NavMeshSurface[])FindObjectsByType(typeof(NavMeshSurface), FindObjectsSortMode.None);

            foreach(NavMeshSurface surface in allNavMeshSurfaces)
            {
                Debug.Log($"NavMesh: Baking surface {surface.gameObject.name} for agent {surface.agentTypeID}");
                surface.BuildNavMesh();
            }
            _buildStatus.navMeshLastBake.SetNow();
            RefreshBuildStatus();
            Debug.Log("BuildTools: Bake NavMesh done.");
        }

        /// <summary>
        /// Bake Occlusion Culling areas
        /// </summary>
        private void BakeOcclusion()
        {
            Debug.Log("BuildTools: Bake Occlusion...");
            StaticOcclusionCulling.Compute();
            _buildStatus.occlusionCullingLastBake.SetNow();
            RefreshBuildStatus();
            Debug.Log("BuildTools: Bake Occlusion done.");
        }

        /// <summary>
        /// Returns scene paths as an array of strings
        /// </summary>
        /// <param name="additiveSceneSettings"></param>
        /// <param name="relativeToAssets"></param>
        /// <returns></returns>
        private string[] GetScenePathsAsArray(AdditiveSceneLoaderSettings additiveSceneSettings, bool relativeToAssets)
        {
            return GetScenePaths(additiveSceneSettings, relativeToAssets).ToArray();
        }

        /// <summary>
        /// Derive the scene paths of all scenes
        /// </summary>
        /// <returns></returns>
        private List<string> GetScenePaths(AdditiveSceneLoaderSettings additiveSceneSettings, bool relativeToAssets)
        {
            List<string> paths = new();
            foreach (AdditiveScene additiveScene in additiveSceneSettings.additiveScenes)
            {
                string fullPath;
                if (relativeToAssets)
                {
                    fullPath = $"Assets/{additiveSceneSettings.scenePath}/{additiveScene.sceneName}.unity";
                }
                else
                {
                    fullPath = $"{Application.dataPath}/{additiveSceneSettings.scenePath}/{additiveScene.sceneName}.unity";
                }
                paths.Add(fullPath);
            }

            return paths;
        }
    }
}