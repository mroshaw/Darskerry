using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Buildings
{
    public enum DoorPivotSide { Left, Right };
    public class Door : MonoBehaviour
    {
        [BoxGroup("Open and Close Config")] public DoorPivotSide pivotSide;
        [BoxGroup("Open and Close Config")] public float openAngle = 130.0f;
        [BoxGroup("Open and Close Config")] public float openDuration = 3.0f;
        [BoxGroup("Open and Close Config")] public float stayOpenDuration = 5.0f;
        [BoxGroup("Open and Close Config")] public float closeDuration = 3.0f;
        [BoxGroup("Open and Close Config")] public bool autoOpen = false;

        [BoxGroup("Door Audio")] public AudioClip openAudioClip;
        [BoxGroup("Door Audio")] public AudioClip closingAudioClip;
        [BoxGroup("Door Audio")] public AudioClip closedAudioClip;

        [BoxGroup("Events")] public UnityEvent onDoorOpeningEvent;
        [BoxGroup("Events")] public UnityEvent onDoorOpenEvent;
        [BoxGroup("Events")] public UnityEvent onDoorClosingEvent;
        [BoxGroup("Events")] public UnityEvent onDoorClosedEvent;

        private bool _isMoving = false;
        private AudioSource _audioSource;

        /// <summary>
        /// Initialise the Door
        /// </summary>
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Public method to Open, pause, then close the door
        /// </summary>
        /// <param name="triggerLocation"></param>
        public void Open(DoorTriggerLocation triggerLocation)
        {
            if(_isMoving)
            {
                return;
            }
            if(triggerLocation==DoorTriggerLocation.Outside)
            {
                StartCoroutine(OpenAndCloseDoorAsync(-openAngle));
            }
            else
            {
                StartCoroutine(OpenAndCloseDoorAsync(openAngle));
            }

            if (_audioSource && openAudioClip)
            {
                _audioSource.PlayOneShot(openAudioClip);
            }
        }

        /// <summary>
        /// Async method to manage timings of opening and closing
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private IEnumerator OpenAndCloseDoorAsync(float angle)
        {
            onDoorOpeningEvent.Invoke();
            // Stop anything else from effecting the door until we're done
            _isMoving = true;
            float currTime = 0;

            // Reverse direction if right pivot
            if (pivotSide == DoorPivotSide.Right)
            {
                angle = -angle;
            }

            // Get start rotation and our target rotation for opening
            Quaternion startTotation = transform.localRotation;
            Quaternion totalRotation = Quaternion.Euler(0, angle, 0);
            Quaternion targetRotation = startTotation * totalRotation;

            // Play the open door sound
            if (_audioSource && openAudioClip)
            {
                _audioSource.PlayOneShot(openAudioClip);
            }

            // Rotate the door open over time
            while (currTime < openDuration)
            {
                transform.localRotation = Quaternion.Lerp(startTotation, targetRotation, currTime / openDuration);
                currTime += Time.deltaTime;
                yield return null;
            }
            transform.localRotation = targetRotation;
            onDoorOpenEvent.Invoke();

            // Hold the door open
            currTime = 0;
            while (currTime < stayOpenDuration)
            {
                currTime += Time.deltaTime;
                yield return null;
            }

            // Close the door
            onDoorClosingEvent.Invoke();
            currTime = 0;

            // Play the closing door sound
            if (_audioSource && closingAudioClip)
            {
                _audioSource.PlayOneShot(closingAudioClip);
            }
            // Rotate the door closed over time
            while (currTime < closeDuration)
            {
                transform.localRotation = Quaternion.Lerp(targetRotation, startTotation, currTime / closeDuration);
                currTime += Time.deltaTime;
                yield return null;
            }
            transform.localRotation = startTotation;

            // Play the close door sound
            if (_audioSource && closedAudioClip)
            {
                _audioSource.PlayOneShot(closedAudioClip);
            }
            // Allow interaction with the door once again
            _isMoving = false;
            onDoorClosedEvent.Invoke();
        }
    }
}