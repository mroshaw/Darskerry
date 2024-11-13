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
                Character = animator.transform.root.GetComponent<GameCharacter>();
            }

            if (AudioSource == null)
            {
                AudioSource = animator.transform.root.GetComponent<AudioSource>();
            }
        }
    }
}