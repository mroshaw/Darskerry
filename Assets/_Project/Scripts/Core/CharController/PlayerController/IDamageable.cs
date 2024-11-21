using UnityEngine;

namespace DaftAppleGames.Darskerry.Core
{
    public interface IDamageable
    {
        public bool IsDead();
        public void TakeDamage(float damage);
        public void RestoreHealth(float health);
    }
}