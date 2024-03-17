using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Ui
{
    /// <summary>
    /// MonoBehaviour class to manage display and tracking of tutorials
    /// </summary>
    public class PointOfInterest : MonoBehaviour
    {
        // Public serializable properties
        [BoxGroup("General Settings")] public InfoPanelContent infoPanelContent;
        
        [SerializeField]
        [BoxGroup("Debug")] private bool _isRead = false;

        [BoxGroup("Events")]  public UnityEvent onOpenEvent;
        [BoxGroup("Events")] public UnityEvent onCloseEvent;
        
        public bool IsRead
        {
            set => _isRead = value;
            get => _isRead;
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
        public void ShowInfoPanel()
        {
            // Populate and show the InfoPanel
            _infoPanel.headingText.text = infoPanelContent.heading;
            _infoPanel.contentText.text = infoPanelContent.content;
            if (infoPanelContent.image != null)
            {
                _infoPanel.image.sprite = infoPanelContent.image;
            }
            _infoPanel.ShowUi();

            _isRead = true;
            onOpenEvent.Invoke();
        }

        /// <summary>
        /// Public method to show tutorial is complete
        /// </summary>
        public void CloseTutorial()
        {
            _infoPanel.HideUi();
            onCloseEvent.Invoke();
        }
    }
}
