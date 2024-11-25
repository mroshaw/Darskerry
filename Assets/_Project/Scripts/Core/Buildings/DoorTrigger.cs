using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    [RequireComponent(typeof(BoxCollider))]
    public class DoorTrigger : MonoBehaviour
    {
        private Door _door;

        private void OnEnable()
        {
            _door = GetComponentInParent<Door>();
        }

        private void Start()
        {
            // Move from parent so not animated with the door
            GameObject doorParent = _door.transform.parent.gameObject;
            gameObject.transform.SetParent(doorParent.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            OpenDoor();
        }

        private void OnTriggerExit(Collider other)
        {
            CloseDoor();
        }

        [Button("Open and Close Door")]
        private void OpenAndCloseDoor()
        {
            _door.OpenAndCloseDoor();
        }


        [Button("Open Door")]
        private void OpenDoor()
        {
            _door.OpenDoor();
        }

        [Button("Close Door")]
        private void CloseDoor()
        {
            _door.CloseDoor();
        }
    }
}