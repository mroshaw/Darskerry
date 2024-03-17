using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    public enum DoorTriggerLocation { Inside, Outside };

    public class DoorTrigger : MonoBehaviour
    {
        [BoxGroup("Door Configuration ")] public DoorTriggerLocation doorTriggerLocation;
        [BoxGroup("Door Configuration ")] public LayerMask triggerLayerMask;
        [BoxGroup("Door Configuration ")] public string[] triggerTags;
        private Door _door;

        /// <summary>
        /// Initialise the Door Trigger
        /// </summary>
        private void Start()
        {
            _door = GetComponentInParent<Door>();
            if (triggerLayerMask == 0)
            {
                triggerLayerMask = LayerMask.GetMask("Player");
            }
        }

        /// <summary>
        /// Open the Door when the Player enters the Trigger area
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"Door Trigger: {other.tag}, {other.gameObject.layer} {other.name}");

            foreach (string triggerTag in triggerTags)
            {
                if(other.CompareTag(triggerTag) && _door.autoOpen)
                {
                    bool inLayer = ((triggerLayerMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer);
                    if(inLayer)
                    {
                        _door.Open(doorTriggerLocation);
                    }
                }
            }
        }
    }
}