using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    public class Door : MonoBehaviour
    {
        [BoxGroup("Settings")] [SerializeField] public float openAngle = 110.0f;
        [BoxGroup("Settings")] [SerializeField] public float openingTime = 2.0f;
        [BoxGroup("Settings")] [SerializeField] public float stayOpenTime = 5.0f;
        [BoxGroup("Settings")] [SerializeField] public float closingTime = 2.0f;

        [BoxGroup("Audio")] [SerializeField] public AudioClip[] openingClips;
        [BoxGroup("Audio")] [SerializeField] public AudioClip[] closingClips;
        [BoxGroup("Audio")] [SerializeField] public AudioClip[] closedClips;

        [FoldoutGroup("Events")] public UnityEvent openingStartEvent;
        [FoldoutGroup("Events")] public UnityEvent openingEndEvent;
        [FoldoutGroup("Events")] public UnityEvent closingStartEvent;
        [FoldoutGroup("Events")] public UnityEvent closingEndEvent;

        private List<Transform> _blockers;

        private AudioSource _audioSource;

        public bool IsOpen { get; private set; }
        private bool _isMoving;

        private Quaternion _doorClosedRotation;
        private Quaternion _doorOpenRotation;

        private void OnEnable()
        {
            _isMoving = false;
            IsOpen = false;
            StopAllCoroutines();
        }

        private void Start()
        {
            _doorClosedRotation = transform.localRotation;
            _doorOpenRotation = gameObject.transform.rotation * Quaternion.Euler(gameObject.transform.up * openAngle);
            _audioSource = GetComponent<AudioSource>();
        }

        public void AddBlocker(Transform blocker)
        {
            _blockers.Add(blocker);
        }

        public void RemoveBlocker(Transform blocker)
        {
            _blockers.Remove(blocker);
        }

        private bool CanClose()
        {
            return _blockers.Count == 0;
        }

        [Button("Open and Close Door")]
        public void OpenAndCloseDoor()
        {
            if (_isMoving || IsOpen)
            {
                return;
            }

            StartCoroutine(OpenAndCloseDoorAsync());
        }

        [Button("Open Door")]
        public void OpenDoor()
        {
            if (_isMoving || IsOpen)
            {
                return;
            }

            StartCoroutine(OpenDoorAsync());
        }

        private IEnumerator OpenDoorAsync()
        {
            _isMoving = true;
            PlayRandomClip(openingClips);
            openingStartEvent.Invoke();
            float timer = 0;
            Quaternion startValue = transform.localRotation;

            while (timer < openingTime)
            {
                transform.rotation = Quaternion.Lerp(startValue, _doorOpenRotation, timer / openingTime);
                timer += Time.deltaTime;
                yield return null;
            }

            _isMoving = false;
            IsOpen = true;
            openingEndEvent.Invoke();
        }


        [Button("Close Door")]
        public void CloseDoor()
        {
            if (_isMoving || !IsOpen)
            {
                return;
            }

            StartCoroutine(CloseDoorAsync());
        }

        private IEnumerator CloseDoorAsync()
        {
            // Door closes
            PlayRandomClip(closingClips);
            closingStartEvent.Invoke();
            float timer = 0;
            Quaternion startValue = transform.localRotation;
            while (timer < closingTime)
            {
                transform.rotation = Quaternion.Lerp(startValue, _doorClosedRotation, timer / closingTime);
                timer += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = _doorClosedRotation;
            _isMoving = false;
            IsOpen = false;
            PlayRandomClip(closedClips);
            closingEndEvent.Invoke();
        }

        private IEnumerator OpenAndCloseDoorAsync()
        {
            // Door opening
            yield return OpenDoorAsync();

            // Door stays open
            yield return new WaitForSeconds(stayOpenTime);

            // Door closes
            yield return CloseDoorAsync();
        }

        private void PlayRandomClip(AudioClip[] clips)
        {
            if (!_audioSource || clips.Length == 0)
            {
                return;
            }

            _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }
}