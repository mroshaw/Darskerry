using UnityEngine;
using System.Collections;

namespace DaftAppleGames.Common.AI
{
    public class BirdAiCrowProximity : MonoBehaviour
    {
        /// <summary>
        /// Trigger message if crow is close
        /// </summary>
        /// <param name="col"></param>
        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Bird")
            {
                col.SendMessage("CrowIsClose");
            }
        }
    }
}