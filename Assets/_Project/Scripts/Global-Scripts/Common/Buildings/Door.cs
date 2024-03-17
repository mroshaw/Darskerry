using System.Collections;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    public enum DoorPivotSide { Left, Right };
    public class Door : MonoBehaviour
    {
        [Header("Open and Close Config")]
        public DoorPivotSide pivotSide;
        public float openAngle = 130.0f;
        public float openDuration = 3.0f;
        public float stayOpenDuration = 5.0f;
        public float closeDuration = 3.0f;
        public bool autoOpen = false;

        [Header("Door Audio")]
        public AudioClip openAudioClip;
        public AudioClip closingAudioClip;
        public AudioClip closedAudioClip;

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

            // Hold the door open
            currTime = 0;
            while (currTime < stayOpenDuration)
            {
                currTime += Time.deltaTime;
                yield return null;
            }

            // Close the door
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
        }
    }
}