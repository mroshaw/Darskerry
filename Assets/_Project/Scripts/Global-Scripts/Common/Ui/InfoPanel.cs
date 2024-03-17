using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Ui
{
    public class InfoPanel : BaseUiWindow
    {
        // Public serializable properties
        [Header("UI Settings")]
        public TMP_Text headingText;
        public TMP_Text contentText;
        public Image image;

        [Header("UI Proxy Events")]
        public UnityEvent onContinueButtonClickedEvent;
        
        /// <summary>
        /// Proxy for the Continue button click event
        /// </summary>
        public void ContinueButtonClickedProxy()
        {
            onContinueButtonClickedEvent.Invoke();
        }
    }
}