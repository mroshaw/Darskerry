using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftApplesGames.Common.Controller
{
    /// <summary>
    /// Helper component to allow scene objects to listen for
    /// global Additive Scene loader events
    /// </summary>
    public class SceneLoaderEvents : MonoBehaviour
    {
        // Public serializable properties
        [FoldoutGroup("Scene Loaded Events")]
        public UnityEvent<string> onSceneLoadedEvent;
        [FoldoutGroup("Scene Activated Events")]
        public UnityEvent<string> onSceneActivatedEvent;
        [FoldoutGroup("All Scenes Loaded Events")]
        public UnityEvent onAllScenesLoadedEvent;
        [FoldoutGroup("All Scenes Activated Events")]
        public UnityEvent onAllScenesActivatedEvent;

        #region UNITY_EVENTS
        /// <summary>
        /// Subscribe to events
        /// </summary>   
        private void Start()
        {
            AdditiveSceneLoadManager.Instance.onAllScenesLoadedEvent.AddListener(CallOnAllScenesLoaded);
            AdditiveSceneLoadManager.Instance.onAllScenesActivatedEvent.AddListener(CallOnAllScenesActivated);
            AdditiveSceneLoadManager.Instance.onSceneLoadedEvent.AddListener(CallOnSceneLoaded);
            AdditiveSceneLoadManager.Instance.onSceneActivatedEvent.AddListener(CallOnSceneActivated);
        }
        
        /// <summary>
        /// Unsubscribe from events
        /// </summary>   
        private void OnDestroy()
        {
            AdditiveSceneLoadManager.Instance.onAllScenesLoadedEvent.RemoveListener(CallOnAllScenesLoaded);
            AdditiveSceneLoadManager.Instance.onAllScenesActivatedEvent.RemoveListener(CallOnAllScenesActivated);
            AdditiveSceneLoadManager.Instance.onSceneLoadedEvent.RemoveListener(CallOnSceneLoaded);
            AdditiveSceneLoadManager.Instance.onSceneActivatedEvent.RemoveListener(CallOnSceneActivated);
        }
        #endregion
        /// <summary>
        /// Invoke events
        /// </summary>
        private void CallOnAllScenesLoaded()
        {
            onAllScenesLoadedEvent.Invoke();
        }

        /// <summary>
        /// Invoke events
        /// </summary>
        private void CallOnAllScenesActivated()
        {
            // Debug.Log($"SceneLoaderEvents: Calling AllScenesActivated for {gameObject.scene.name}");
            onAllScenesActivatedEvent.Invoke();
        }

        /// <summary>
        /// Invoke events
        /// </summary>
        /// <param name="sceneName"></param>
        private void CallOnSceneLoaded(string sceneName)
        {
            onSceneLoadedEvent.Invoke(sceneName);
        }

        /// <summary>
        /// Invoke events
        /// </summary>
        /// <param name="sceneName"></param>
        private void CallOnSceneActivated(string sceneName)
        {
            onSceneActivatedEvent.Invoke(sceneName);
        }
    }
}
