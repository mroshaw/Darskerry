using UnityEngine;

namespace DaftAppleGames.Common.Audio
{
    /// <summary>
    /// Helper class to provide access to the UiAudioController singleton instance
    /// </summary>
    public class UiSoundManager : MonoBehaviour
    {
        /// <summary>
        /// Public method to play Clip click
        /// </summary>
        public void PlayClick()
        {
            UiAudioController.PlayClick();
        }

        /// <summary>
        /// Public method to play Big Click clip
        /// </summary>
        public void PlayBig()
        {
            UiAudioController.PlayBig();
        }

        /// <summary>
        /// Public method to play Cancel clip
        /// </summary>
        public void PlayCancel()
        {
            UiAudioController.PlayCancel();
        }

        /// <summary>
        /// Public method to play Back clip
        /// </summary>
        public void PlayBack()
        {
            UiAudioController.PlayBack();
        }

        /// <summary>
        /// Public method to play Positive clip
        /// </summary>
        public void PlayPositive()
        {
            UiAudioController.PlayPositive();
        }
    }
}