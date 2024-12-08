using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public interface ISpawnListener
    {
        public abstract void OnSpawn(GameObject gameObject);

        public abstract void OnDespawn(GameObject gameObject);
    }
}