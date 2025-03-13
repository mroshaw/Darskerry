#if GPU_INSTANCER
using GPUInstancer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry
{
    public class GPUInstancerReadyEvents : MonoBehaviour
    {
        [BoxGroup("Events")] public UnityEvent gpuInstancerReadyEvent;

        private bool _detailReady = false;
        private bool _treesReady = false;

        private void Awake()
        {
            GPUInstancerAPI.StartListeningGPUIEvent(GPUInstancerEventType.DetailInitializationFinished, DetailCallBackHandler);
            GPUInstancerAPI.StartListeningGPUIEvent(GPUInstancerEventType.TreeInitializationFinished, TreesCallBackHandler);
        }

        private void DetailCallBackHandler()
        {
            _detailReady = true;
        }

        private void TreesCallBackHandler()
        {
            _treesReady = true;
        }

        private void CheckGPUInstanceStatus()
        {
            if (_detailReady && _treesReady)
            {
                gpuInstancerReadyEvent.Invoke();
            }
        }
    }
}
#endif