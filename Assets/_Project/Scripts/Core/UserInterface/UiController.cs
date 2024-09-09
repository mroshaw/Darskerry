using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    /// <summary>
    /// Singleton class to administer over active UI windows
    /// </summary>
    public class UiController : MonoBehaviour
    {
        // Singleton static instance
        private static UiController _instance;
        public static UiController Instance => _instance;

        private static List<BaseUiWindow> _windows = new();

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
        public bool IsAnyUiOpen => _windows.Any(window => window.IsUiOpen);

        /// <summary>
        /// Debug function to return list of open UI windows 
        /// </summary>
        /// <returns></returns>
        public string GetOpenUiName()
        {
            string result = "";
            
            foreach (BaseUiWindow window in _windows)
            {
                if (window.IsUiOpen)
                {
                    if (window.gameObject)
                    {
                        result += window.gameObject.name + "_";
                    }
                }
            }
            return result;
        }
        
        public void RegisterUiWindow(BaseUiWindow window)
        {
            _windows.Add(window);
        }

        public void UnRegisterUiWindow(BaseUiWindow window)
        {
            _windows.Remove(window);
        }
    }
}