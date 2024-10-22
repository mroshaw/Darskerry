using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Scenes
{
    public class SceneLoaderEvents : MonoBehaviour
    {
        [BoxGroup("Events")] public UnityEvent allScenesLoadedEvent;
        [BoxGroup("Events")] public UnityEvent faderStartedEvent;
        [BoxGroup("Events")] public UnityEvent faderFinishedEvent;
        private void Start()
        {
            AdditiveSceneLoader.Instance.allScenesLoadedEvent.AddListener(AllScenesLoadedProxy);
            AdditiveSceneLoader.Instance.faderStartedEvent.AddListener(FaderStartedProxy);
            AdditiveSceneLoader.Instance.faderFinishedEvent.AddListener(FaderFinishedProxy);
        }

        private void AllScenesLoadedProxy()
        {
            allScenesLoadedEvent.Invoke();
        }

        private void FaderStartedProxy()
        {
            faderStartedEvent.Invoke();
        }

        private void FaderFinishedProxy()
        {
            faderFinishedEvent.Invoke();
        }
    }
}