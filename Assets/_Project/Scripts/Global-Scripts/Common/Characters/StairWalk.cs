using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    public class StairWalk : MonoBehaviour
    {
        [Header("General Settings")]
        public bool isOnStairs = false;

        private Animator _animator;

        // Start is called before the first frame update
        public void Start()
        {
            _animator = GetComponentInParent<Animator>();
        }

        /// <summary>
        /// Trigger stair animation state when on stairs
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Stair"))
            {
                _animator.SetBool("IsOnStairs", true);
                isOnStairs = true;
            }
        }

        /// <summary>
        /// Revert animation state when off stairs
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Stair"))
            {
                _animator.SetBool("IsOnStairs", false);
                isOnStairs = false;
            }
        }
    }
}