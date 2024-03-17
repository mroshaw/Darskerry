using PixelCrushers;
using PixelCrushers.QuestMachine;
using UnityEngine;
using Sirenix.OdinInspector;
using Quest = PixelCrushers.QuestMachine.Wrappers.Quest;

namespace DaftAppleGames.Common.Quests
{
    public class QuestTrigger : MonoBehaviour
    {
        [BoxGroup("Quest Settings")] public Quest triggerQuest;
        [BoxGroup("Quest Settings")] public StringField triggerQuestState;
        [BoxGroup("Behaviour")] public bool useTriggerCollider = true;
        [BoxGroup("Behaviour")] public bool deactivateOnTrigger = true;

        /// <summary>
        /// If triggering is enabled, and player is the trigger, then update the Quest Node State
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (!useTriggerCollider)
            {
                return;
            }
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                UpdateQuestNodeState();
            }
        }

        /// <summary>
        /// Update the quest node state
        /// </summary>
        public void UpdateQuestNodeState()
        {
            // Check that quest is active
            if (QuestMachine.GetQuestNodeState(triggerQuest.id, triggerQuestState) == QuestNodeState.Active)
            {
                QuestMachine.SetQuestNodeState(triggerQuest.id, triggerQuestState, QuestNodeState.True);

                if (deactivateOnTrigger)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
