#if PIXELCRUSHERS
using System;
using DaftAppleGames.Common.GameControllers;
using PixelCrushers;

namespace DaftAppleGames.Common.LoadSaveGame
{
    public class SaveDetailsSaver : Saver
    {
        [Serializable]
        public class SaveGameDetailsData
        {
            public string description;
            public string date;
            public string time;
        }
        
        public override string RecordData()
        {
            var data = new SaveGameDetailsData();
            data.description = GameController.Instance.SelectedCharacter.ToString();
            data.date = DateTime.Now.ToString("dd/MMM/yyyy");
            data.time = DateTime.Now.ToString("h:mm tt");
            return SaveSystem.Serialize(data);
        }

        public override void ApplyData(string s)
        {
            // Do nothing. We only record the data for the save/load menu.
            // There's no need to re-apply it when loading a saved game.
        }
    }
}
#endif