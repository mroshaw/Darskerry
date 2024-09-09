using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    public class NotificationPanel : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("UI Settings")] [SerializeField] private TMP_Text notificationText;
        [BoxGroup("Fade Settings")] [SerializeField] private float notificationVisibleTime = 2.0f;
        [BoxGroup("Fade Settings")] [SerializeField] private float notificationFadeTime = 2.0f;
        private Color _visibleColor;
        private Color _hiddenColor;

        private Queue<string> _notificationQueue = new();
        private bool _isShowingNotification;
        #endregion

        #region Startup
        private void Awake()
        {
            Color textColor = notificationText.color;
            _visibleColor = new Color(textColor.r, textColor.g, textColor.b, 1);
            _hiddenColor = new Color(textColor.r, textColor.g, textColor.b, 0);
        }
        #endregion

        #region Class methods
        public void Notify(string message)
        {
            _notificationQueue.Enqueue(message);
        }

        private void Update()
        {
            if (_notificationQueue.Count == 0 || _isShowingNotification)
            {
                return;
            }

            StartCoroutine(NotifyFade(_notificationQueue.Dequeue()));
        }

        /// <summary>
        /// Show the current notification queue, fade in and out
        /// </summary>
        /// <returns></returns>
        private IEnumerator NotifyFade(string message)
        {
            _isShowingNotification = true;
            notificationText.color = _hiddenColor;
            notificationText.text = message;

            // Fade text in
            float time = 0;
            while (time < notificationFadeTime)
            {
                notificationText.color = Color.Lerp(_hiddenColor, _visibleColor, time / notificationFadeTime);
                time += Time.deltaTime;
                yield return null;
            }
            notificationText.color = _visibleColor;

            // Wait
            yield return new WaitForSecondsRealtime(notificationVisibleTime);

            // Fade out
            time = 0;
            while (time < notificationFadeTime)
            {
                notificationText.color = Color.Lerp(_visibleColor, _hiddenColor, time / notificationFadeTime);
                time += Time.deltaTime;
                yield return null;
            }
            notificationText.color = _hiddenColor;
            notificationText.text = "";

            _isShowingNotification = false;
        }
        #endregion
    }
}