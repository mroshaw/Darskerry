using System.Collections;
using System.Collections.Generic;
using DaftAppleGames.Common.Ui;
using DaftAppleGames.Common.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if PIXELCRUSHERS
using SaveSystem = PixelCrushers.Wrappers.SaveSystem;
#endif

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace DaftAppleGames.Common.GameControllers
{
    public enum SceneLoadStatus
    {
        None, Loading, Loaded, Activating, Activated
    }
    
    public enum FixedSceneType
    {
        MainMenuLoader,
        GameLoader,
        GameCompleteLoader
    }
    
    /// <summary>
    /// MonoBehaviour class to manage the additive / incremental loading of many scenes
    /// </summary>
    public class AdditiveSceneLoadManager : MonoBehaviour
    {
        /// <summary>
        /// Internal class for metadata related to each scene
        /// </summary>
        [BoxGroup("Behaviour")] public bool loadScenesOnStart = true;
        [BoxGroup("Behaviour")] public bool loadScenesOnAwake = false;
        [BoxGroup("Behaviour")] public bool useLoadingScene = true;
        [BoxGroup("Behaviour")] public bool showProgress = true;
        [BoxGroup("Behaviour")] public bool showRotatingLogo = true;

        [BoxGroup("Scenes")][InlineEditor()][SerializeField] public AdditiveSceneLoaderSettings additiveSceneSettings;

        [FoldoutGroup("UI Settings")] public Canvas loadingUiCanvas;
        [FoldoutGroup("UI Settings")] public bool showLoadingScreen = true;
        [FoldoutGroup("UI Settings")] public Slider progressSlider;
        [FoldoutGroup("UI Settings")] public TMP_Text progressText;
        [FoldoutGroup("UI Settings")] public Image rotatingLogo;

        [BoxGroup("Editor Settings")][Tooltip("When this scene is loaded, load all scenes listed in the AdditiveScenes list")] public bool loadScenesOnLoad;

        [BoxGroup("Event Settings")] public float eventDelay = 1.0f;
        [BoxGroup("Event Settings")] public float deathDelay = 1.0f;

        // public UnityEvent onSceneLoadedEvent;
        // public UnityEvent onSceneActivatedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent<string> onSceneLoadedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent<string> onSceneActivatedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent onAllScenesLoadedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent onAllScenesActivatedEvent;
        [FoldoutGroup("Fixed Scene Events")] public UnityEvent onFixedScenePreLoadEvent;
        [FoldoutGroup("Fixed Scene Events")] public UnityEvent onFixedScenePostLoadEvent;
        [FoldoutGroup("Fixed Scene Events")] public UnityEvent onThisSceneLoadedEvent;

        private List<AdditiveScene> _scenesToLoad;

        private bool _isLoading = false;
        private bool _hasLoaded = false;
        private bool _hasActivated = false;
        private bool _isLoadingFromSave = false;
        private int _loadSlot;

        private SceneFader _sceneFader;
        
        // Singleton static instance
        private static AdditiveSceneLoadManager _instance;
        public static AdditiveSceneLoadManager Instance => _instance;
        
        /// <summary>
        /// Setup the Scene Loader
        /// </summary>
        private void Awake()
        {
            // Set up singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            _sceneFader = GetComponent<SceneFader>();
            
            if (loadScenesOnAwake)
            {
                GetScenesToLoad();
                ProcessAllScenes(LoadSceneMode.Additive);
            }

            // Subscribe to SceneLoad load event
            SceneManager.sceneLoaded -= ThisSceneLoadedHandler;
            SceneManager.sceneLoaded += ThisSceneLoadedHandler;

        }

        /// <summary>
        /// Initialise the manager
        /// </summary>
        private void Start()
        {
            if (loadScenesOnStart)
            {
                GetScenesToLoad();
                ProcessAllScenes(LoadSceneMode.Additive);
            }
        }

        /// <summary>
        /// Handler for current scene loading event
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void ThisSceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
            onThisSceneLoadedEvent?.Invoke();
        }
        
        /// <summary>
        /// Init the scene load status
        /// </summary>
        private void InitSettings()
        {
            foreach (AdditiveScene additiveScene in additiveSceneSettings.additiveScenes)
            {
                additiveScene.LoadStatus = SceneLoadStatus.None;
                additiveScene.SceneOp = null;
            }
        }
        
        /// <summary>
        /// Public method to load the main game
        /// </summary>
        public void LoadGame()
        {
            LoadFixedScene(FixedSceneType.GameLoader);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("GameReloadProxyScene");
        }

        /// <summary>
        /// Public method to load the main menu
        /// </summary>
        public void LoadMainMenu()
        {
            LoadFixedScene(FixedSceneType.MainMenuLoader);
        }

        /// <summary>
        /// Public method to load the game complete scene
        /// </summary>
        public void LoadGameComplete()
        {
            
        }
        
        /// <summary>
        /// Loads the given fixed scene
        /// </summary>
        /// <param name="sceneType"></param>
        private void LoadFixedScene(FixedSceneType sceneType)
        {
            AdditiveScene newAdditiveScene = new AdditiveScene();
            foreach (FixedScene fixedScene in additiveSceneSettings.fixedScenes)
            {
                if (fixedScene.sceneType == sceneType)
                {
                    newAdditiveScene.sceneName = fixedScene.sceneName;
                    newAdditiveScene.isMainScene = true;
                    _scenesToLoad = new List<AdditiveScene> { newAdditiveScene };
                    onFixedScenePreLoadEvent.Invoke();
                    _sceneFader.FadeOut(ProcessFixedScene);
                    onFixedScenePostLoadEvent.Invoke();
                }
            }
        }

        /// <summary>
        /// Calls the routine but in Single mode
        /// </summary>
        private void ProcessFixedScene()
        {
            ProcessAllScenes(LoadSceneMode.Single);
        }
        
        /// <summary>
        /// Top level process orchestration
        /// </summary>
        private void ProcessAllScenes(LoadSceneMode loadSceneMode)
        {
            InitSettings();
#if PIXELCRUSHERS
            if (GameController.Instance.IsLoadingFromSave)
            {
                SaveSystem.LoadFromSlot(GameController.Instance.LoadSlot);
            }
#endif            
            if (_scenesToLoad.Count < 1)
            {
                AllScenesReady();
            }
            
            Debug.Log($"Total Additive Scenes:{additiveSceneSettings.additiveScenes.Count}");
            Debug.Log($"Total Scenes To Load:{_scenesToLoad.Count}");

            rotatingLogo.gameObject.SetActive(showRotatingLogo);
            if (showRotatingLogo)
            {
                rotatingLogo.GetComponent<SmoothSpinUiObject>().StartSpinning();
            }
            progressSlider.gameObject.SetActive(showProgress);

            // Set progress slider based on number of scenes actually loading
            progressSlider.maxValue = _scenesToLoad.Count;
            if (useLoadingScene)
            {
                loadingUiCanvas.gameObject.SetActive(showLoadingScreen);
            }
            _isLoading = true;
            Debug.Log("Main Scene Loaded...");
            
            StartCoroutine(ProcessAllScenesAsync(loadSceneMode));
            
        }
        
        /// <summary>
        /// Monitor progress and update progress bar while loading
        /// </summary>
        private void Update()
        {
            if (_isLoading && !_hasLoaded && !_hasActivated)
            {
                UpdateProgress();
            }
        }

        private IEnumerator ProcessAllScenesAsync(LoadSceneMode loadSceneMode=LoadSceneMode.Additive)
        {
            yield return null;

            // Load scenes
            LoadScenes(loadSceneMode);

            // Wait for all scenes to load
            while (!AllScenesInState(SceneLoadStatus.Loaded))
            {
                yield return null;
            }
            onAllScenesLoadedEvent.Invoke();
            
            // Activate scenes
            ActivateScenes();

            // Wait for all scenes to activate
            while (!AllScenesInState(SceneLoadStatus.Activated))
            {
                yield return null;
            }
            // Done!
            yield return new WaitForSeconds(1);
            AllScenesReady();
        }

        /// <summary>
        /// True if all scenes are in the given state, otherwise false
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool AllScenesInState(SceneLoadStatus status)
        {
            foreach (AdditiveScene additiveScene in _scenesToLoad)
            {
                if (additiveScene.LoadStatus != status)
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Iterate and load each Additive scene asynchronously
        /// </summary>
        private void LoadScenes(LoadSceneMode loadSceneMode)
        {
            foreach (AdditiveScene additiveScene in _scenesToLoad)
            {
#if UNITY_EDITOR
                Debug.Log($"Starting Editor async load of scene: {additiveScene.sceneName}...");
                Scene currentScene = EditorSceneManager.GetSceneByName(additiveScene.sceneName);
                if (!currentScene.IsValid())
                {
                    StartCoroutine(LoadSceneAsync(additiveScene, loadSceneMode));
                }
                else
                {
                    Debug.Log($"Already Loaded: {additiveScene.sceneName}");
                    additiveScene.LoadStatus = SceneLoadStatus.Loaded;
                }
#else                    
                StartCoroutine(LoadSceneAsync(additiveScene, loadSceneMode));
#endif
            }
        }
        
        /// <summary>
        /// Iterate and activate all scenes
        /// </summary>
        private void ActivateScenes()
        {
            foreach (AdditiveScene additiveScene in _scenesToLoad)
            {
                StartCoroutine(ActivateSceneAsync(additiveScene));
            }
        }
        
        /// <summary>
        /// Updates the actual list of scenes to load
        /// </summary>
        /// <returns></returns>
        public void GetScenesToLoad()
        {
            _scenesToLoad = new();
            
            foreach(AdditiveScene scene in additiveSceneSettings.additiveScenes)
            {
#if UNITY_EDITOR
                if (scene.inEditor)
                {
                    scene.LoadStatus = SceneLoadStatus.None;
                    scene.SceneOp = null;
                    _scenesToLoad.Add(scene);
                }
#else
                if (scene.inGame)
                {
                    scene.LoadStatus = SceneLoadStatus.None;
                    scene.SceneOp = null;
                    _scenesToLoad.Add(scene);
                }
#endif
            }
        }

        /// <summary>
        /// Load the given scene async, and remove from list once loaded. Do not activate
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public IEnumerator LoadSceneAsync(AdditiveScene additiveScene, LoadSceneMode loadSceneMode)
        {
            additiveScene.LoadStatus = SceneLoadStatus.Loading;

            // Wait until next frame
            yield return null;

            Debug.Log($"Async Load Scene: {additiveScene.sceneName}");
            //Begin to load the Scene
#if UNITY_EDITOR
            AsyncOperation asyncOperation =
                EditorSceneManager.LoadSceneAsync(additiveScene.sceneName, loadSceneMode);
#else
            AsyncOperation asyncOperation =
                SceneManager.LoadSceneAsync(additiveScene.sceneName, loadSceneMode);
#endif
            asyncOperation.allowSceneActivation = false;
            additiveScene.SceneOp = asyncOperation;

            // When the load is still in progress, output the Text and progress bar
            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }

            additiveScene.LoadStatus = SceneLoadStatus.Loaded;
            onSceneLoadedEvent.Invoke(additiveScene.sceneName);
            Debug.Log($"Async Load Scene DONE: {additiveScene.sceneName}");
            yield return null;
        }

        /// <summary>
        /// Wait for the scene to load and then activate
        /// </summary>
        /// <param name="additiveScene"></param>
        /// <returns></returns>
        private IEnumerator ActivateSceneAsync(AdditiveScene additiveScene)
        {
            // Wait until next frame
            yield return null;
            additiveScene.LoadStatus = SceneLoadStatus.Activating;
            Debug.Log($"Async Activate Scene: {additiveScene.sceneName}");

            if (additiveScene.SceneOp != null)
            {
                // Activate the scene
                additiveScene.SceneOp.allowSceneActivation = true;
                
                // Wait for scene to fully activate
                while (!additiveScene.SceneOp.isDone)
                {
                    yield return null;
                }
            }
            
            additiveScene.LoadStatus = SceneLoadStatus.Activated;
            onSceneActivatedEvent.Invoke(additiveScene.sceneName);
            Debug.Log($"Async Activate Scene DONE: {additiveScene.sceneName}");
            
            if (additiveScene.isMainScene)
            {
                Debug.Log($"Setting Main Scene: {additiveScene.sceneName}");
#if UNITY_EDITOR                
                EditorSceneManager.SetActiveScene(SceneManager.GetSceneByName(additiveScene.sceneName));
#else
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(additiveScene.sceneName));
#endif
            }
        }
        
        /// <summary>
        /// Update the progress slider
        /// </summary>
        private void UpdateProgress()
        {
            float totalProgress = 0.0f;
            foreach (AdditiveScene additiveScene in _scenesToLoad)
            {
                if (additiveScene.SceneOp != null)
                {
                    totalProgress += additiveScene.SceneOp.progress;
                }
                // progressText.text = sceneOp.ToString();
            }
            progressSlider.value = totalProgress;
        }
        
        /// <summary>
        /// Wait defined number of seconds before wrapping up
        /// </summary>
        /// <returns></returns>
        private IEnumerator FinishWithDelay()
        {
            yield return new WaitForSeconds(eventDelay);
            AllScenesReady();
        }

        /// <summary>
        /// Kills self after delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator DieWithDelay()
        {
            yield return new WaitForSeconds(deathDelay);
            Destroy(gameObject);
        }

        /// <summary>
        /// Final call, wrap up the load process
        /// </summary>
        private void AllScenesReady()
        {
            // Merge Light Probes
            LightProbes.Tetrahedralize();
        
            Debug.Log("All Scenes Activated!");
            loadingUiCanvas.gameObject.SetActive(false);
            _isLoading = false;
            
#if PIXELCRUSHERS
            if (_isLoadingFromSave)
            {
                SaveSystem.LoadFromSlot(_loadSlot);
            }
#endif
            onAllScenesActivatedEvent.Invoke();
            
            // All done, so destroy myself
            // StartCoroutine(DieWithDelay());
        }
    }
}