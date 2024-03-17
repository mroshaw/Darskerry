#if PIXELCRUSHERS
using System;
using DaftAppleGames.Common.GameControllers;
using PixelCrushers;
using UnityEngine;

namespace DaftAppleGames.Common.LoadSaveGame
{
    public class GameControllerSaver : Saver
    {
        /// <summary>
        /// Class to store serialised summary of Game Controller properties
        /// </summary>
        [Serializable]
        public class GameControllerData
        {
            public CharSelection charSelected;
        }

        /// <summary>
        /// Serialize and save
        /// </summary>
        /// <returns></returns>
        public override string RecordData()
        {
            GameControllerData gameControllerData = new GameControllerData
            {
                charSelected = GameController.Instance.SelectedCharacter
            };
            return SaveSystem.Serialize(gameControllerData);
        }

        /// <summary>
        /// Deserialize and load
        /// </summary>
        /// <param name="saveDataString"></param>
        public override void ApplyData(string saveDataString)
        {
            Debug.Log("GameController: Applying game save data...");
        
            // Deserialize
            if (string.IsNullOrEmpty(saveDataString)) return; // No data to apply.
            GameControllerData gameControllerData = SaveSystem.Deserialize<GameControllerData>(saveDataString);
            if (gameControllerData == null) return; 

            // Update selected character
            GameController.Instance.SelectedCharacter = gameControllerData.charSelected;
            Debug.Log($"GameController: Applying game save data...{gameControllerData.charSelected}");
        }
    }
}
#endif