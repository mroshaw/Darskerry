using DaftAppleGames.Common.AI;
using UnityEngine;

namespace DaftAppleGames.Common.CameraTools
{
    public class AddCameraToLivingBirds : MonoBehaviour
    {
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Start()
        {
            BirdAiController birdAiController = GetComponent<BirdAiController>();
            birdAiController.currentCamera = Camera.main;
        }
    }
}
