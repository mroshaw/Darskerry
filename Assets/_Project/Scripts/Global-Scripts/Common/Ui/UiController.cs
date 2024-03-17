using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Ui
{
    /// <summary>
    /// Singleton class to administer over active UI windows
    /// </summary>
    public class UiController : MonoBehaviour
    {
        // Singleton static instance
        private static UiController _instance;
        public static UiController Instance => _instance;

        [SerializeField]
        private static List<BaseUiWindow> _windows = new();

        /// <summary>
        /// Initialise the GameController Singleton instance
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            _windows = new();
        }

        
        /// <summary>
        /// Public getter to show if any Ui windows are currently open
        /// </summary>
        public bool AnyUiOpen
        {
            get
            {
                foreach (BaseUiWindow window in _windows)
                {
                    if (window.isUiOpen)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Debug function to return list of open UI windows 
        /// </summary>
        /// <returns></returns>
        public string GetOpenUiName()
        {
            string result = "";
            
            foreach (BaseUiWindow window in _windows)
            {
                // Debug.Log("In ForEach");
                if (window.isUiOpen)
                {
                    // Debug.Log("In WindowIsOpen");
                    if (window.gameObject)
                    {
                        result += window.gameObject.name + "_";
                    }
                }
            }

            return result;
        }
        
        /// <summary>
        /// Register a UI window
        /// </summary>
        /// <param name="window"></param>
        public void RegisterUiWindow(BaseUiWindow window)
        {
            _windows.Add(window);
        }

        /// <summary>
        /// De-register a Ui window
        /// </summary>
        /// <param name="window"></param>
        public void UnRegisterUiWindow(BaseUiWindow window)
        {
            _windows.Remove(window);
        }
        
    }
}
