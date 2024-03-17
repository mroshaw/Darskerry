using PixelCrushers;
using PixelCrushers.QuestMachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using QuestState = PixelCrushers.QuestMachine.QuestState;

namespace DaftAppleGames.Common.Quests
{
    public class QuestStateAllChangedEvents : MonoBehaviour, IMessageHandler
    {
        [BoxGroup("Events")] public UnityEvent questActiveEvent;
        [BoxGroup("Events")] public UnityEvent questSuccessEvent;
        [BoxGroup("Events")] public UnityEvent questFailedEvent;
        [BoxGroup("Events")] public UnityEvent questUpdatedEvent;
        /// <summary>
        /// Add listener to the QuestStateChanged message
        /// </summary>
        private void OnEnable()
        {
            // Listen for Quest State Changed messages:
            MessageSystem.AddListener(this, QuestMachineMessages.QuestStateChangedMessage, string.Empty);
        }

        /// <summary>
        /// Remove the listener
        /// </summary>
        private void OnDisable()
        {
            // Stop listening:
            MessageSystem.RemoveListener(this);
        }

        /// <summary>
        /// Public method, to be attached to an Event, to process the quest state change
        /// </summary>
        /// <param name="args"></param>
        public void OnMessage(MessageArgs args)
        {
            // Look for "main" quest changes
            if((args.values[0] == null))
            {
                switch (args.values[1].ToString())
                {
                    case nameof(QuestState.Active):
                        questActiveEvent.Invoke();
                        break;
                    case nameof(QuestState.Successful):
                        questSuccessEvent.Invoke();
                        break;
                    case nameof(QuestState.Failed):
                        questFailedEvent.Invoke();
                        break;
                }

                return;
            }

            // Look for "updated" quest changes
            string questNodeState = args.values[1].ToString();
            string questNodeName =args.values[0].ToString();



            if (questNodeState == "True" && !questNodeName.Contains("start") && !questNodeName.ToLower().Contains("success"))
            {
                questUpdatedEvent.Invoke();
            }
        }
    }
}
