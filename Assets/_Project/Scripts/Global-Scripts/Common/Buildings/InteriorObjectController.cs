using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Buildings
{
    public class InteriorObjectController : MonoBehaviour
    {

        [FoldoutGroup("Interior Game Object")] public GameObject[] interiorGameObject;

        /// <summary>
        /// Enables all interior objects
        /// </summary>
        public void EnableAll()
        {
            SetState(true);
        }

        /// <summary>
        /// Disables all interior objects
        /// </summary>
        public void DisableAll()
        {
            SetState(false);
        }

        /// <summary>
        /// Sets all object state to given state
        /// </summary>
        /// <param name="state"></param>
        public void SetState(bool state)
        {
            foreach (GameObject obj in interiorGameObject)
            {
                obj.SetActive(state);
            }
        }
    }
}
