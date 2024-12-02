using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public class SpawnObject : MonoBehaviour
    {
        #region Class Variables
        [FoldoutGroup("Events")] public UnityEvent SpawnEvent;
        [FoldoutGroup("Events")] public UnityEvent DespawnEvent;
        #endregion

        #region Startup
        #endregion

        #region Class Methods

        public void Spawn()
        {
            SpawnEvent.Invoke();
        }

        public void Despawn()
        {
            DespawnEvent.Invoke();
        }
        #endregion
    }
}