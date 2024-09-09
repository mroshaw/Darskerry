using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    public enum TutorialTriggerAction {Enter, Exit}
    public class TutorialTrigger : MonoBehaviour
    {
        // Public serializable properties
        [Header("Tutorial Trigger Settings")]
        public TutorialTriggerAction triggerAction;
        public string triggerTag = "Player";

        [Header("Tutorial Settings")]
        public bool force = false;
        
        private Tutorial _tutorial;

        /// <summary>
        /// Init the component
        /// </summary>
        private void Start()
        {
            _tutorial = GetComponent<Tutorial>();
        }
        
        /// <summary>
        /// Trigger on enter, if configured
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (triggerAction == TutorialTriggerAction.Enter && other.gameObject.CompareTag(triggerTag))
            {
                StartTutorial();
            }
        }

        /// <summary>
        /// Trigger on exit, if configured
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit(Collider other)
        {
            if (triggerAction == TutorialTriggerAction.Exit && other.gameObject.CompareTag(triggerTag))
            {
                StartTutorial();
            }
        }

        /// <summary>
        /// Starts the tutorial
        /// </summary>
        private void StartTutorial()
        {
            _tutorial.ShowTutorial(force);
        }
    }
}