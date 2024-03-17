using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Characters
{
    public class TestTriggerCollider : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger: {other.tag}, {other.gameObject.layer} {other.name}");
        }
    }
}
