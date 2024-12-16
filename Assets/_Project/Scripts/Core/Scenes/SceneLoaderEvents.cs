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
            AdditiveSceneLoader loader = FindAnyObjectByType<AdditiveSceneLoader>();
            if (loader)
            {
                loader.allScenesLoadedEvent.AddListener(AllScenesLoadedProxy);
                loader.faderStartedEvent.AddListener(FaderStartedProxy);
                loader.faderFinishedEvent.AddListener(FaderFinishedProxy);
            }
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