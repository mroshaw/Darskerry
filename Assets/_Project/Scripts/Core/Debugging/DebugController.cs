using System.Collections.Generic;
using DaftAppleGames.Darskerry.Core.DagCharacterController.Input;
using GPUInstancer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.Debugging
{
    [DefaultExecutionOrder(-1)]
    public class DebugController : MonoBehaviour
    {
        #region Class Variables
        [Header("Debug State")]
        [SerializeField] private bool gpuiToggleState;

        private DebugInput _debugInput;
        #endregion

        #region Startup
        private void Awake()
        {
            gpuiToggleState = true;
            _debugInput = GetComponent<DebugInput>();
        }
        #endregion

        #region Update Logic
        private void Update()
        {
            HandleDebugInput();
        }

        private void HandleDebugInput()
        {
            if (_debugInput.ToggleGPUIPressed)
            {
                gpuiToggleState = !gpuiToggleState;
                Debug.Log($"DebugController: toggling GPUI state to: {gpuiToggleState}");

                List<GPUInstancerManager> allManagers = GPUInstancerAPI.GetActiveManagers();
                if (allManagers == null || allManagers.Count == 0)
                {
                    return;
                }
                foreach (GPUInstancerManager manager in allManagers)
                {
                    manager.enabled = gpuiToggleState;
                }
            }
        }
        #endregion
    }
}