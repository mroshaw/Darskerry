using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    public class WaterTrigger : MonoBehaviour
    {
        public bool IsInWater
        {
            get => _isInWater;
            set => _isInWater = value;
        }

        [SerializeField]
        private bool _isInWater = false;
    }
}
