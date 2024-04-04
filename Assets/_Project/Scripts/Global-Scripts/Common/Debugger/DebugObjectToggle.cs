using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Debugger
{
    public class DebugObjectToggle : DebugBase
    {
        [BoxGroup("Settings")] public GameObject[] toggleGameObjects;

        /// <summary>
        /// Disables all objects
        /// </summary>
        [Button("Disable All")]
        public void DisableObjects()
        {
            SetAllObjectState(false);
        }

        /// <summary>
        /// Enables all objects
        /// </summary>
        [Button("Enable All")]
        public void EnableObjects()
        {
            SetAllObjectState(true);
        }

        /// <summary>
        /// Toggles all objects 
        /// </summary>
        [Button("Toggle All")]
        public void ToggleObjects()
        {
            ToggleAllObjects();
        }

        /// <summary>
        /// Sets all game objects to current state
        /// </summary>
        /// <param name="state"></param>
        private void SetAllObjectState(bool state)
        {
            foreach (GameObject currentGameObject in toggleGameObjects)
            {
                currentGameObject.SetActive(state);
            }
        }

        /// <summary>
        /// Toggles all object states (inactive if active, active if inactive)
        /// </summary>
        private void ToggleAllObjects()
        {
            foreach (GameObject currentGameObject in toggleGameObjects)
            {
                currentGameObject.SetActive(!currentGameObject.activeSelf);
            }
        }
    }
}
