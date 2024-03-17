using DaftAppleGames.Common.Characters;
using UnityEngine;

namespace DaftAppleGames.Common.Environment
{
    public class Water : MonoBehaviour
    {
        /// <summary>
        /// Let WaterSplash component know that it's in water
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("WaterTrigger"))
            {
                WaterTrigger waterTrigger = other.gameObject.GetComponent<WaterTrigger>(); 
                if (waterTrigger)
                {
                    waterTrigger.IsInWater = true;
                }
            }
        }
        
        /// <summary>
        /// Let WaterSplash component know that it's out of water
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("WaterTrigger"))
            {
                WaterTrigger waterTrigger = other.gameObject.GetComponent<WaterTrigger>(); 
                if (waterTrigger)
                {
                    waterTrigger.IsInWater = false;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("WaterTrigger"))
            {
                WaterTrigger waterTrigger = other.gameObject.GetComponent<WaterTrigger>(); 
                if (waterTrigger)
                {
                    waterTrigger.IsInWater = true;
                }
            }
        }
        
    }
}
