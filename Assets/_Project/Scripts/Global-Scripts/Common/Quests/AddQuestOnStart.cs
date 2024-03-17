#if PIXELCRUSHERS
using System.Collections;
using PixelCrushers.QuestMachine;
using UnityEngine;
using UnityEngine.Events;
using Quest = PixelCrushers.QuestMachine.Wrappers.Quest;
using QuestJournal = PixelCrushers.QuestMachine.Wrappers.QuestJournal;

namespace DaftAppleGames.Common.Quests 
{
    public class AddQuestOnStart : MonoBehaviour
    {
        [Header("Quest Journal Settings")]
        public QuestJournal questJournal;
        public Quest quest;
        public float startDelay = 0.0f;
        
        [Header("Dialogue Settings")]
        public bool showDialog;
        public UnityEvent dialogTriggerEvent;

        [Header("Other Settings")]
        public bool inactiveOnDone = true;
        
        private bool _isDone = false;
        
        /// <summary>
        /// Assign the quest, if not already done so.
        /// </summary>
        private void Start()
        {
            if (!_isDone)
            {
                StartCoroutine(StartQuestAsync());
                _isDone = true;
            }
        }

        /// <summary>
        /// Async method to start the quest after a set delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartQuestAsync()
        {
            yield return new WaitForSeconds(startDelay);
            questJournal.AddQuest(quest);
            quest.SetState(QuestState.Active);

            if (showDialog)
            {
                dialogTriggerEvent.Invoke();
            }

            if (inactiveOnDone)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
#endif