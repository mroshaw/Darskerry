using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Characters.Animals
{
    public class Animal : Character
    {
        // Public serializable properties
        [BoxGroup("Hunger Settings")]
        [Tooltip("Once hunger is below this value, animal is classed as hungry")]
        public float hungerThreshold = 10.0f;
        [BoxGroup("Hunger Settings")]
        [Tooltip("Rate at which animal becomes hungry. The larger the value, the quicker the animal becomes hungry.")]
        public float hungerRate = 0.01f;

        [BoxGroup("Thirst Settings")]
        [Tooltip("Once thirst is below this value, animal is classed as thirsty")]
        public float thirstThreshold = 10.0f;
        [BoxGroup("Thirst Settings")]
        [Tooltip("Rate at which animal becomes thirsty. The larger the value, the quicker the animal becomes thirsty.")]
        public float thirstRate = 0.01f;

        // Public properties

        // Private fields
        private float _hunger = 100.0f;
        private float _thirst = 100.0f;

        #region UNITY_EVENTS
        /// <summary>
        /// Configure the component on start
        /// </summary>
        public override void Start()
        {
            base.Start();
        }
        #endregion

        /// <summary>
        /// Eat food to replenish hunger
        /// </summary>
        /// <param name="foodValue"></param>
        public void Eat(float foodValue)
        {
            _hunger += foodValue;

            if (_hunger > 100.0f)
            {
                _hunger = 100.0f;
            }
        }

        /// <summary>
        /// Drink to replenish thirst
        /// </summary>
        /// <param name="drinkValue"></param>
        public void Drink(float drinkValue)
        {
            _thirst += drinkValue;

            if (_thirst > 100.0f)
            {
                _thirst = 100.0f;
            }
        }

        /// <summary>
        /// Is Animal Hungry?
        /// </summary>
        /// <returns></returns>
        public bool IsHungry()
        {
            return _hunger < hungerThreshold;
        }

        /// <summary>
        /// Is Animal Thirsty?
        /// </summary>
        /// <returns></returns>
        public bool IsThirsty()
        {
            return _thirst < thirstThreshold;
        }
    }
}
