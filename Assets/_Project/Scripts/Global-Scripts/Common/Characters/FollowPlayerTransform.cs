using UnityEngine;

namespace DaftAppleGames.Common.Player
{
    public class FollowPlayerTransform : MonoBehaviour
    {
        private Transform Player;
        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            UpdateTransform(Player);
        }

        /// <summary>
        /// Updates the transforms position to the player postion
        /// </summary>
        /// <param name="player"></param>
        private void UpdateTransform(Transform player)
        {
            if (player != null)
            {
                gameObject.transform.SetPositionAndRotation(player.position, Quaternion.identity);
            }
        }
        /// <summary>
        /// Sets the new player transform
        /// </summary>
        /// <param name="player"></param>
        public void SetNewPlayer(Transform player)
        {
            Player = player;
        }
    }
}