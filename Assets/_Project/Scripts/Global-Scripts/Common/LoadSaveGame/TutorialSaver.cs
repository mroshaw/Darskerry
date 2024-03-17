#if PIXELCRUSHERS
using System;
using DaftAppleGames.Common.Ui;
using PixelCrushers;
using UnityEngine;

namespace DaftAppleGames.Common.LoadSaveGame
{
    public class TutorialSaver : Saver
    {
        /// <summary>
        /// Class to store serialised tutorial state
        /// </summary>
        [Serializable]
        public class TutorialData
        {
            public bool tutorialDone;
        }

        /// <summary>
        /// Serialize and save
        /// </summary>
        /// <returns></returns>
        public override string RecordData()
        {
            TutorialData tutorialData = new TutorialData
            {
                tutorialDone = GetComponent<Tutorial>().IsDone
            };
            Debug.Log($"TutorialSaver: Saving state of {gameObject.name} as {GetComponent<Tutorial>().IsDone}");
            return SaveSystem.Serialize(tutorialData);
        }

        /// <summary>
        /// Deserialize and load
        /// </summary>
        /// <param name="saveDataString"></param>
        public override void ApplyData(string saveDataString)
        {
            // Deserialize
            if (string.IsNullOrEmpty(saveDataString)) return; // No data to apply.
            TutorialData tutorialData = SaveSystem.Deserialize<TutorialData>(saveDataString);
            if (tutorialData == null) return;

            Debug.Log($"TutorialSaver: Loading state of {gameObject.name} as {tutorialData.tutorialDone}");

            // Update selected character
            GetComponent<Tutorial>().IsDone = tutorialData.tutorialDone;
        }
    }
}
#endif