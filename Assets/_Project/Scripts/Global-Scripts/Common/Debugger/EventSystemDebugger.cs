
using UnityEngine;
using UnityEngine.EventSystems;

namespace DaftAppleGames.Common.Debugger
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