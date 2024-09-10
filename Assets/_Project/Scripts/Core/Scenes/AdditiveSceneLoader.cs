using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace DaftAppleGames.Darskerry.Core.Scenes
{
    public enum SceneLoadStatus
    {
        None, Loading, Loaded, Activating, Activated
    }
    
    /// <summary>
    /// MonoBehaviour class to manage the additive / incremental loading of many scenes
    /// </summary>
  public class AdditiveSceneLoader : MonoBehaviour
    {
        /// <summary>
        /// Internal class for metadata related to each scene
        /// </summary>
        [BoxGroup("Behaviour")] public bool loadScenesOnStart = true;
        [BoxGroup("Behaviour")] public bool loadScenesOnAwake ;
        [BoxGroup("Scenes")] [TableList] public List<AdditiveScene> additiveScenes;

        [FoldoutGroup("Additive Scene Events")] public UnityEvent allScenesLoadStartedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent sceneLoadedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent sceneActivatedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent allScenesLoadedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent allScenesActivatedEvent;
        [FoldoutGroup("Additive Scene Events")] public UnityEvent<float> allScenesLoadProgressUpdateEvent;
        [FoldoutGroup("This Scene Events")] public UnityEvent thisSceneLoadedEvent;

        private bool _isLoading;
        private bool _hasLoaded = false;
        private bool _hasActivated = false;

        // Singleton static instance
        private static AdditiveSceneLoader _instance;
        public static AdditiveSceneLoader Instance => _instance;

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

            if (loadScenesOnAwake)
            {
                LoadAllScenes(LoadSceneMode.Additive);
            }
        }

        private void Start()
        {
            if (loadScenesOnStart)
            {
                LoadAllScenes(LoadSceneMode.Additive);
            }
        }

        private void ThisSceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
            thisSceneLoadedEvent?.Invoke();
        }

#if UNITY_EDITOR
        private void EditorSceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"AdditiveSceneManager: Editor Scene Loaded detected: {scene.name}");
        }
#endif
        /// <summary>
        /// Init the scene load status
        /// </summary>
        private void InitLoadStatus()
        {
            foreach (AdditiveScene additiveScene in additiveScenes)
            {
                additiveScene.LoadStatus = SceneLoadStatus.None;
                additiveScene.SceneOp = null;
            }
        }

        /// <summary>
        /// Top level process orchestration
        /// </summary>
        private void LoadAllScenes(LoadSceneMode loadSceneMode)
        {
            allScenesLoadStartedEvent.Invoke();
            InitLoadStatus();

            Debug.Log($"Total Additive Scenes:{additiveScenes.Count}");

            StartCoroutine(LoadAllScenesAsync(loadSceneMode));
            
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

        private IEnumerator LoadAllScenesAsync(LoadSceneMode loadSceneMode=LoadSceneMode.Additive)
        {
            _isLoading = true;

            // Load scenes
            LoadScenes(loadSceneMode);

            // Wait for all scenes to load
            while (!AllScenesInState(SceneLoadStatus.Loaded))
            {
                yield return null;
            }
            AllScenesLoaded();
            
            // Activate scenes
            ActivateScenes();

            // Wait for all scenes to activate
            while (!AllScenesInState(SceneLoadStatus.Activated))
            {
                yield return null;
            }
            // Done!
            yield return new WaitForSeconds(1);
            AllScenesActivated();
        }

        /// <summary>
        /// True if all scenes are in the given state, otherwise false
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool AllScenesInState(SceneLoadStatus status)
        {
            foreach (AdditiveScene additiveScene in additiveScenes)
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
            foreach (AdditiveScene additiveScene in additiveScenes)
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
            foreach (AdditiveScene additiveScene in additiveScenes)
            {
                StartCoroutine(ActivateSceneAsync(additiveScene));
            }
        }

        /// <summary>
        /// Load the given scene async, and remove from list once loaded. Do not activate
        /// </summary>
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
            sceneLoadedEvent.Invoke();
            Debug.Log($"Async Load Scene DONE: {additiveScene.sceneName}");
            yield return null;
        }

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
            sceneActivatedEvent.Invoke();
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
        
        private void UpdateProgress()
        {
            float totalProgress = 0.0f;
            foreach (AdditiveScene additiveScene in additiveScenes)
            {
                if (additiveScene.SceneOp != null)
                {
                    totalProgress += additiveScene.SceneOp.progress;
                }
            }
            allScenesLoadProgressUpdateEvent.Invoke(totalProgress);
        }

        private void AllScenesLoaded()
        {
            Debug.Log("All Scenes Loaded!");
            allScenesLoadedEvent.Invoke();
        }

        private void AllScenesActivated()
        {
            Debug.Log("All Scenes Activated!");
            _isLoading = false;
            
            allScenesActivatedEvent.Invoke();
        }
    }
}