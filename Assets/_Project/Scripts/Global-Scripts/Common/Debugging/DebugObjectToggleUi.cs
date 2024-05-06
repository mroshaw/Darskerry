using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace DaftAppleGames.Common.Debugging
{
    public class DebugObjectToggleUi : DebugBaseUi
    {
        [BoxGroup("Settings")] public string debugObjectToggleObjectName;

        private DebugObjectToggle _debugObjectToggle;

        /// <summary>
        /// Set up the component
        /// </summary>
        public override void Start()
        {
            base.Start();

            if (string.IsNullOrEmpty(debugObjectToggleObjectName))
            {
                Debug.LogError($"DebugObjectToggleUi: Please set the debugObjectToggleObject property on {gameObject.name}!");
            }
            else
            {
                _debugObjectToggle = (DebugObjectToggle)base.FindDebugObject<DebugObjectToggle>(debugObjectToggleObjectName);
            }
        }

        /// <summary>
        /// Find the named DebugObject component
        /// </summary>
        private void FindDebugObject()
        {
            DebugObjectToggle[] allObjectToggles = FindObjectsOfType<DebugObjectToggle>();
            foreach (DebugObjectToggle toggle in allObjectToggles)
            {
                if (toggle.gameObject.name == debugObjectToggleObjectName)
                {
                    _debugObjectToggle = toggle;
                    return;
                }
            }

            Debug.LogError($"DebugObjectToggleUi: could not find object named {debugObjectToggleObjectName} in loaded scenes! Please check: {gameObject.name}!");
        }

        /// <summary>
        /// Handle for the enable button
        /// </summary>
        public void EnableObjectsProxy()
        {
            _debugObjectToggle.EnableObjects();
        }

        /// <summary>
        /// Handle for the disable button
        /// </summary>
        public void DisableObjectsProxy()
        {
            _debugObjectToggle.DisableObjects();
        }

        /// <summary>
        /// Handler for the toggle button
        /// </summary>
        public void ToggleObjectsProxy()
        {
            _debugObjectToggle.ToggleObjects();
        }

    }
}