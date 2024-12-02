using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public abstract class Spawner : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Debug")] [SerializeReference] private List<ISpawnable> spawnables;
        #endregion

        #region Class Methods

        public void RegisterSpawnable(ISpawnable spawnable)
        {
            spawnables.Add(spawnable);
            spawnable.Spawner = this;
        }

        public void UnregisterSpawnable(ISpawnable spawnable)
        {
            spawnables.Remove(spawnable);
            spawnable.Spawner = null;
        }

        #endregion
    }
}