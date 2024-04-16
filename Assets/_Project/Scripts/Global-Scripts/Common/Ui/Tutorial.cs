using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Ui
{
    /// <summary>
    /// MonoBehaviour class to manage display and tracking of tutorials
    /// </summary>
    public class Tutorial : MonoBehaviour
    {
        // Public serializable properties
        [BoxGroup("General Settings")] public InfoPanelContent infoPanelContent;
        [BoxGroup("General Settings")] public float delay;
        
        [SerializeField]
        [BoxGroup("Debug")] private bool _isDone = false;

        [BoxGroup("Events")]  public UnityEvent onTutorialOpenEvent;
        [BoxGroup("Events")] public UnityEvent onTutorialCloseEvent;
        
        public bool IsDone
        {
            set => _isDone = value;
            get => _isDone;
        }
        
        // Private fields
        private InfoPanel _infoPanel;

        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            _infoPanel = GetComponentInChildren<InfoPanel>();
        }

        /// <summary>
        /// Displays the specified tutorial.
        /// </summary>
        /// <param name="force"></param>
        public void ShowTutorial(bool force)
        {
            StartCoroutine(ShowTutorialAsync(force));
            onTutorialOpenEvent.Invoke();
        }

        /// <summary>
        /// Public method to show tutorial is complete
        /// </summary>
        public void CloseTutorial()
        {
            onTutorialCloseEvent.Invoke();
        }
        
        /// <summary>
        /// Async coroutine wrapper for ShowTutorialAfterDelay
        /// </summary>
        /// <param name="force"></param>
        /// <returns></returns>
        private IEnumerator ShowTutorialAsync(bool force)
        {
            yield return new WaitForSeconds(delay);
            // Only show tutorial if it's not been done, or if we choose to force it
            if (!IsDone || force)
            {
                // Populate and show the InfoPanel
                _infoPanel.SetHeadingText(infoPanelContent.heading);
                _infoPanel.SetContentText(infoPanelContent.content);
                if (infoPanelContent.image != null)
                {
                    _infoPanel.SetImage(infoPanelContent.image);
                }
                _infoPanel.ShowUi();
                IsDone = true;
            }            
        }
    }
}