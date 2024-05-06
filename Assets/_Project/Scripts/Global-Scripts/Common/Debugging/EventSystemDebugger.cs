
using UnityEngine;
using UnityEngine.EventSystems;

namespace DaftAppleGames.Common.Debugging
{
    public class EventSystemDebugger : MonoBehaviour
    {
        public GameObject currentSlectedGameObject;

        private void Update()
        {
            currentSlectedGameObject = EventSystem.current.currentSelectedGameObject;
        }
    }
}