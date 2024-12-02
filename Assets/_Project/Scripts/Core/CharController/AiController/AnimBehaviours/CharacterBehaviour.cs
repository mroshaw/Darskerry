using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.AnimBehaviours
{
    /// <summary>
    /// Base class for Character/Player based animation state behaviours
    /// </summary>
    public abstract class CharacterBehaviour : StateMachineBehaviour
    {
        #region Class Variables
        protected GameCharacter Character { get; private set; }
        protected AudioSource AudioSource { get; private set; }
        #endregion
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Character == null)
            {
                Character = animator.GetComponent<GameCharacter>();

                if (!Character)
                {
                    Debug.LogError($"No GameCharacter found on {animator.transform.root.gameObject.name}");
                }
            }

            if (AudioSource == null)
            {
                AudioSource = animator.GetComponent<AudioSource>();
                if (!AudioSource)
                {
                    Debug.LogError($"No AudioSource found on {animator.transform.root.gameObject.name}");
                }

            }
        }
    }
}