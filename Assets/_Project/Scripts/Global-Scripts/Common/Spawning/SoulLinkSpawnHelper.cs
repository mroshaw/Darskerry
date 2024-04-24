#if SOULLINK_SPAWNER
using Magique.SoulLink;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Spawning
{
    public class SoulLinkSpawnHelper : MonoBehaviour
    {

        private SoulLinkSpawner _spawner;

        /// <summary>
        /// Init Soul Link spawner
        /// </summary>
        private void Start()
        {
            _spawner = GetComponent<SoulLinkSpawner>();
        }

        /// <summary>
        /// Sets the Player transform on the Spawner
        /// </summary>
        /// <param name="playerGameObject"></param>
        public void SetPlayer(GameObject playerGameObject)
        {
            SoulLinkSpawner.Instance.RegisterPlayer(playerGameObject.transform);
        }

    }
}
#endif