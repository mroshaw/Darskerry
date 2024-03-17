using PixelCrushers.QuestMachine.Wrappers;
using PixelCrushers.Wrappers;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Quests
{
    public class JournalSaveHelper : MonoBehaviour
    {
        public QuestJournal journal;
        public void LoadSaveData()
        {
            if (SaveSystem.currentSavedGameData != null)
            {
                journal.ApplyData(SaveSystem.currentSavedGameData.GetData(journal.key));
            }
        }
    }
}
