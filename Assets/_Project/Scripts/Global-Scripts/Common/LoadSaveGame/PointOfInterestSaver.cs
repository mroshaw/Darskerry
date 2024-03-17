#if PIXELCRUSHERS
using System;
using DaftAppleGames.Common.Ui;
using PixelCrushers;
using UnityEngine;

namespace DaftAppleGames.Common.LoadSaveGame
{
    public class PointOfInterestSaver : Saver
    {
        /// <summary>
        /// Class to store serialised tutorial state
        /// </summary>
        [Serializable]
        public class PointOfInterestData
        {
            public bool isRead;
        }

        /// <summary>
        /// Serialize and save
        /// </summary>
        /// <returns></returns>
        public override string RecordData()
        {
            PointOfInterestData pointOfInterestData = new PointOfInterestData
            {
                isRead = GetComponent<PointOfInterest>().IsRead
            };
            Debug.Log($"PointOfInterestSaver: Saving state of {gameObject.name} as {GetComponent<PointOfInterest>().IsRead}");
            return SaveSystem.Serialize(pointOfInterestData);
        }

        /// <summary>
        /// Deserialize and load
        /// </summary>
        /// <param name="saveDataString"></param>
        public override void ApplyData(string saveDataString)
        {
            // Deserialize
            if (string.IsNullOrEmpty(saveDataString)) return; // No data to apply.
            PointOfInterestData pointOfInterestData = SaveSystem.Deserialize<PointOfInterestData>(saveDataString);
            if (pointOfInterestData == null) return;

            Debug.Log($"TutorialSaver: Loading state of {gameObject.name} as {pointOfInterestData.isRead}");

            // Update selected character
            GetComponent<PointOfInterest>().IsRead = pointOfInterestData.isRead;
        }
    }
}
#endif