using PixelCrushers;
using PixelCrushers.QuestMachine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using QuestState = PixelCrushers.QuestMachine.QuestState;

namespace DaftAppleGames.Common.Quests
{
    public class QuestStateChangedEvents : MonoBehaviour, IMessageHandler
    {
        [BoxGroup("Quest Settings")] public Quest[] associatedQuests;
        [BoxGroup("Events")] public UnityEvent startEvent;
        [BoxGroup("Events")] public UnityEvent questActiveEvent;
        [BoxGroup("Events")] public UnityEvent questSuccessEvent;
        [BoxGroup("Events")] public UnityEvent questFailedEvent;

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
        /// Call event when component starts, to allow initialisation
        /// </summary>
        private void Start()
        {
            startEvent.Invoke();
        }

        /// <summary>
        /// Public method, to be attached to an Event, to process the quest state change
        /// </summary>
        /// <param name="args"></param>
        public void OnMessage(MessageArgs args)
        {
            if (associatedQuests.Length == 0)
            {
                return;
            }
            if(args.values[0] == null)
            {
                foreach (Quest quest in associatedQuests)
                {
                    if (quest.id.ToString() == args.parameter.ToString())
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
                }
            }
        }
    }
}
