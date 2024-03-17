using MalbersAnimations.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    public class LookAtPlayer : MonoBehaviour
    {

        [BoxGroup("Settings")] public Aim aim;

        private void Start()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                LookAtPlayerTarget playerTarget = other.gameObject.GetComponent<LookAtPlayerTarget>();
                if (playerTarget)
                {
                    aim.AimTarget = playerTarget.lookAtTarget;
                }
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                aim.AimTarget = null;
            }
        }
    }


}

