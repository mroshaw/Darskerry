#if PIXELCRUSHERS
using PixelCrushers.QuestMachine;
using UnityEngine;
using QuestJournal = PixelCrushers.QuestMachine.QuestJournal;

namespace DaftAppleGames.Common.Quests 
{
    public class JournalInput : MonoBehaviour
    {
        public KeyCode journalKey = KeyCode.J;
        public QuestJournal questJournal;
        public bool isShown = false;

        private void Start()
        {
            if (!questJournal)
            {
                questJournal = QuestMachine.GetQuestJournal();
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(journalKey))
            {
                if (!isShown)
                {
                    questJournal.ShowJournalUI();
                    isShown = true;
                }
                else
                {
                    questJournal.HideJournalUI();
                    isShown = false;
                }
            }
        }

        public void Show()
        {
            isShown = true;
        }

        public void Hide()
        {
            isShown = false;
        }
    }
}
#endif