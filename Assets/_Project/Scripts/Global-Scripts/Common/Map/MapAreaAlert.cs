#if INVECTOR_SHOOTER
using Invector.vCharacterController;
#endif
using UnityEngine;

namespace DaftAppleGames.Common.Map
{
    public class MapAreaAlert : MonoBehaviour
    {
        [Header("Settings")]
        public string settlementName;
        public bool alertOnFirstVisitOnly;
        public string newSettlementAlert = "You have discovered ";
        public AudioClip firstVisitSound;
        public AudioClip visitSound;

        private bool _hasBeenVisited;
        private AudioSource _audioSource;
        
        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _hasBeenVisited = false;
        }
        
        /// <summary>
        /// Manage when player enters the range of the settlement
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            
            Debug.Log($"Area Trigger: {other.name}");

            if (!_hasBeenVisited)
            {
                _hasBeenVisited = true;
                ShowAlertFirstVisit();
                return;
            }

            if (!alertOnFirstVisitOnly)
            {
                ShowAlert();
            }
        }

        /// <summary>
        /// Show the "First Discovered" alert
        /// </summary>
        private void ShowAlertFirstVisit()
        {
            _audioSource.PlayOneShot(firstVisitSound);
#if INVECTOR_SHOOTER
            vHUDController.instance.ShowText($"{newSettlementAlert}\n{settlementName}");
#endif
        }

        /// <summary>
        /// Show the "entered map area" alert
        /// </summary>
        private void ShowAlert()
        {
            _audioSource.PlayOneShot(visitSound);
#if INVECTOR_SHOOTER
            vHUDController.instance.ShowText(settlementName);
#endif
        }
    }
}
