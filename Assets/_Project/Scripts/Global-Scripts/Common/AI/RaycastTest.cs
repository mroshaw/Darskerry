using UnityEngine;

namespace DaftAppleGames.Common.AI 
{
    public class RaycastTest : MonoBehaviour
    {
        [Header("Debug")]
        public string colliderName;
        public LayerMask ignoreLayers;
        private void Start()
        {
            
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.forward, new Color(1, 0.549f, 0));
            
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 30.0f, ~ignoreLayers))
            {
                colliderName = hit.collider.gameObject.name;
            }
        }
    }
}
