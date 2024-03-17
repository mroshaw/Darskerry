using Invector.vCharacterController;
using PixelCrushers;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Ui
{
    public class HudNotificationManager : MonoBehaviour
    {
        [BoxGroup("Audio")] public AudioClip newAudioClip;
        [BoxGroup("Audio")] public AudioClip successAudioClip;
        [BoxGroup("Audio")] public AudioClip failureAudioClip;

        [BoxGroup("Debug")] public string testMessage;

        private AudioSource _audioSource;

        /// <summary>
        /// Init the component
        /// </summary>
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        [Button("Test Notification")]
        private void TestNotification()
        {
            ShowNotification(testMessage);
        }

        /// <summary>
        /// Show the notification text
        /// </summary>
        /// <param name="notificationText"></param>
        public void ShowNotification(string notificationText)
        {
            vHUDController.instance.ShowText(notificationText, 3.0f, 1.0f);
        }

        /// <summary>
        /// New Quest notification
        /// </summary>
        /// <param name="args"></param>
        public void NewQuest(MessageArgs args)
        {
            ShowNotification("You have a new quest!");
            PlayNotificationAudioNew();
        }

        /// <summary>
        /// Play a "new" audio notification
        /// </summary>
        [Button("Test 'New' Audio")]
        public void PlayNotificationAudioNew()
        {
            _audioSource.PlayOneShot(newAudioClip);
        }

        /// <summary>
        /// Play a "success" audio notification
        /// </summary>
        [Button("Test 'Success' Audio")]
        public void PlayNotificationAudioSuccess()
        {
            _audioSource.PlayOneShot(successAudioClip);
        }

        /// <summary>
        /// Play a "failure" autio notification
        /// </summary>
        [Button("Test 'Failure' Audio")]
        public void PlayNotificationAudioFailure()
        {
            _audioSource.PlayOneShot(failureAudioClip);
        }
    }
}
