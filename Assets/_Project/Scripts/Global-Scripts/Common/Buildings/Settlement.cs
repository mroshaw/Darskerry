using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Buildings
{
    public class Settlement : MonoBehaviour
    {
        // Public serializable properties
        [FoldoutGroup("General Settings")]
        public string settlementName;
        [FoldoutGroup("General Settings")]
        public GameObject[] buildingsAndProps;
        [FoldoutGroup("General Settings")]
        public GameObject[] npcs;
        [FoldoutGroup("General Settings")]
        public GameObject[] animals;
        [FoldoutGroup("Enabled At Start")]
        public bool enabledAtStart =  false;
        
        [FoldoutGroup("Events")]
        public UnityEvent onEnterSettlementEvent;
        [FoldoutGroup("Events")]
        public UnityEvent onExitSettlementEvent;
        
        #region UNITY_EVENTS
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            SetSettlementState(enabledAtStart);
        }

        /// <summary>
        /// Sets all settlement object active state
        /// </summary>
        /// <param name="state"></param>
        private void SetSettlementState(bool state)
        {
            GameObject[] allObjects = buildingsAndProps.Concat(npcs).Concat(animals).ToArray();
            foreach (GameObject item in allObjects)
            {
                item.SetActive(state);
            }
        }

        /// <summary>
        /// Enable the settlement when player is close enough
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                SetSettlementState(true);
            }
        }

        /// <summary>
        /// Disable the settlement when player is far enough away
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                SetSettlementState(false);
            }
        }
        #endregion
    }
}
