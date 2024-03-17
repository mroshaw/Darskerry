using UnityEngine;
using static DaftAppleGames.Common.Audio.AmbientAudioManager;

namespace DaftAppleGames.Common.Audio
{
    /// <summary>
    /// Helper behaviour to allow UnityEvents to call the
    /// Ambient Audio Manager
    /// </summary>
    public class AmbientAudioMethods : MonoBehaviour
    {
        /// <summary>
        /// Starts playing Ambient Audio, based on type
        /// </summary>
        /// <param name="ambientAudioType"></param>
        public void StartAmbientAudioByType(AmbientAudioType ambientAudioType)
        {
            AmbientAudioManager.Instance.StartAmbientAudio(ambientAudioType);
        }

        /// <summary>
        /// Starts playing Ambient Audio, based on name
        /// </summary>
        /// <param name="ambientAudioName"></param>
        public void StartAmbientAudioByName(string ambientAudioName)
        {
            AmbientAudioManager.Instance.StartAmbientAudio(LookUpTypeByName(ambientAudioName));
        }

        /// <summary>
        /// Stops playing Ambient Audio, based on type
        /// </summary>
        /// <param name="ambientAudioType"></param>
        public void StoptAmbientAudioByType(AmbientAudioType ambientAudioType)
        {
            AmbientAudioManager.Instance.StopAmbientAudio(ambientAudioType);
        }

        /// <Stops>
        /// Starts playing Ambient Audio, based on name
        /// </summary>
        /// <param name="ambientAudioName"></param>
        public void StopAmbientAudioByName(string ambientAudioName)
        {
            AmbientAudioManager.Instance.StopAmbientAudio(LookUpTypeByName(ambientAudioName));
        }

        /// <summary>
        /// Gets the AmbientAudioType by name. Useful for UnityEvents, where
        /// enums are not public
        /// </summary>
        /// <param name="ambientAudioName"></param>
        /// <returns></returns>
        private AmbientAudioType LookUpTypeByName(string ambientAudioName)
        {
            return (AmbientAudioType)System.Enum.Parse(typeof(AmbientAudioType), ambientAudioName);
        }
    }
}
