using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    /// <summary>
    /// Simple Windmill blade spinner Monobehaviour
    /// </summary>
    public class Windmill : MonoBehaviour
    {
        [BoxGroup("General Settings")]
        public GameObject windMillBlades;
        [BoxGroup("Speed Settings")]
        public float rotateSpeed = 0.5f;

        public void Start()
        {
            if (!windMillBlades)
            {
                windMillBlades = this.gameObject;
            }

        }

        /// <summary>
        /// Rotate the windmill
        /// </summary>
        public void Update()
        {
            windMillBlades.transform.Rotate(rotateSpeed * Time.deltaTime, 0.0f, 0.0f, Space.Self);
        }
    }
}