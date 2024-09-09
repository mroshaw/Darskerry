using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    public class InfoPanel : BaseUiWindow
    {
        // Public serializable properties
        [BoxGroup("UI Settings")] public TMP_Text headingText;
        [BoxGroup("UI Settings")] public TMP_Text contentText;
        [BoxGroup("UI Settings")] public Image image;

        [BoxGroup("UI Settings")] public Image[] inlineImages;

        [Header("UI Proxy Events")]
        public UnityEvent onContinueButtonClickedEvent;
        
        /// <summary>
        /// Proxy for the Continue button click event
        /// </summary>
        public void ContinueButtonClickedProxy()
        {
            onContinueButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Sets the heading text
        /// </summary>
        /// <param name="text"></param>
        public void SetHeadingText(string text)
        {
            headingText.text = text;
        }

        /// <summary>
        /// Sets the content text
        /// </summary>
        /// <param name="text"></param>
        public void SetContentText(string text)
        {
            contentText.text = text;
        }

        /// <summary>
        /// Set the content image
        /// </summary>
        /// <param name="sprite"></param>
        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}