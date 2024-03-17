using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    public class Structure : MonoBehaviour
    {
        // Public serializable properties
        [Header("Dimensions")]
        public float width;
        public float length;
        public float height;
        public float lengthCenterOffset = 0.0f;
#region UNITY_EVENTS
        /// <summary>
        /// Subscribe to events
        /// </summary>   
        private void OnEnable()
        {
            
        }
        
        /// <summary>
        /// Unsubscribe from events
        /// </summary>   
        private void OnDisable()
        {
            
        }

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            
        }
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            
        }
#endregion
    }
}
