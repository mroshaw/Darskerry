using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    public class InteriorController : MonoBehaviour
    {
        #region Class Variables

        [BoxGroup("Interior")] [SerializeField] private GameObject interiorGameObject;
        [BoxGroup("Interior")] [SerializeField] private bool hideInteriorOnStart = true;
        #endregion

        #region Startup
        private void Start()
        {
            if (hideInteriorOnStart)
            {
                HideInterior();
            }
        }
        #endregion

        #region Update Logic
        #endregion

        #region Class Methods

        [Button("Show Interior")]
        public void ShowInterior()
        {
            SetInteriorState(true);
        }

        [Button("Hide Interior")]
        public void HideInterior()
        {
            SetInteriorState(false);
        }

        public void SetInteriorState(bool state)
        {
            interiorGameObject.SetActive(state);
        }
        #endregion
    }
}