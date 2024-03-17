using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Time = UnityEngine.Time;

namespace DaftAppleGames.Common.Audio
{
    public enum BackgroundMusicType { Clip, Group }

    public class BackgroundMusicPlayer : MonoBehaviour
    {
        [BoxGroup("Play Settings")] public bool loopAll;
        [BoxGroup("Start Settings")] public bool playOnStart = true;
        [BoxGroup("Start Settings")] public float delayBeforeStart = 1.0f;
        [BoxGroup("Fade Settings")] public float fadeWhenSecondsLeft = 2.0f;
        [BoxGroup("Fade Settings")] public float delayInSecondsBetweenFades = 2.0f;
        [BoxGroup("Fade Settings")] public float fadeTimeInSeconds = 2.0f;

        [BoxGroup("Music Group Clips")] public List<BackgroundMusicGroup> backgroundMusicGroups;

        [FoldoutGroup("Events")] public UnityEvent<string> ClipStartedEvent;
        [FoldoutGroup("Events")] public UnityEvent<string> ClipFinishedEvent;
        [FoldoutGroup("Events")] public UnityEvent<string> ClipGroupStartedEvent;
        [FoldoutGroup("Events")] public UnityEvent<string> ClipGroupFinishedEvent;

        [FoldoutGroup("Debug")] [SerializeField] private string _currentClipName;
        [FoldoutGroup("Debug")] [SerializeField] public string _currentClipGroupName;
        [FoldoutGroup("Debug")] [SerializeField] private float _currentClipLength;
        [FoldoutGroup("Debug")] [SerializeField] private float _playedSoFar;
        [FoldoutGroup("Debug")] [SerializeField] private bool _inFade = false;
        [FoldoutGroup("Debug")] [SerializeField] private bool _isPlaying;

        private int _currentGroupIndex = 0;
        private int _currentGroupClipIndex = 0;

        private AudioSource _audioSource;

        // Start is called before the first frame update
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            // Set current to indexes
            _currentGroupIndex = 0;
            _currentGroupClipIndex = 0;
        
            _isPlaying = false;
            
            if (playOnStart)
            {
                StartCoroutine(PlayAfterDelay(delayBeforeStart));
            }
        }

        /// <summary>
        /// Begin playing after given number of seconds
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator PlayAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            Play();
            yield break;
        }
        
        /// <summary>
        /// Wait until clip is almost finished, fade out and fade in next
        /// </summary>
        private void Update()
        {
            if (!_isPlaying || _inFade)
            {
                return;
            }
            _playedSoFar = _audioSource.time;

            if (_audioSource.time + fadeWhenSecondsLeft >= _currentClipLength)
            {
                Debug.Log("BackgroundMusicPlayer: Update: Current clip almost finished...");

                // Decide what to do next

                // Check if current group is finished
                bool groupClipsFinished = _currentGroupClipIndex == backgroundMusicGroups[_currentGroupIndex].GroupClips.Count -1;

                // Check if we've played the last group
                bool lastGroup = _currentGroupIndex == backgroundMusicGroups.Count - 1;

                Debug.Log($"BackgroundMusicPlayer: Update: GroupClipsFinished={groupClipsFinished}, LastGroup={lastGroup}");

                // If we've reached the end of all clips in all groups
                if (lastGroup && groupClipsFinished)
                {
                    if (loopAll)
                    {
                        // Go back to the start
                        Debug.Log("BackgroundMusicPlayer: Update: Restarting all groups");
                        _currentGroupIndex = 0;
                        _currentGroupClipIndex = 0;
                        FadeOut(true, 0, 0);
                        return;
                    }
                    else
                    {
                        // We're done
                        Debug.Log("BackgroundMusicPlayer: Update: No more clips or groups. Done.");
                        _currentGroupIndex = 0;
                        _currentGroupClipIndex = 0;
                        FadeOut(false, _currentGroupIndex, _currentGroupIndex);
                        _isPlaying = false;
                    }
                }

                // If we've reached the end of the current group, start playing the next group
                if (groupClipsFinished)
                {
                    Debug.Log("BackgroundMusicPlayer: Update: Group clips finished. Moving on to next group");
                    _currentGroupIndex++;
                    _currentGroupClipIndex = 0;
                    FadeOut(true, _currentGroupIndex, _currentGroupClipIndex);
                    return;
                }

                // Play the next clip in the group
                Debug.Log("BackgroundMusicPlayer: Update: Playing next clip in group");
                _currentGroupClipIndex++;
                FadeOut(true, _currentGroupIndex, _currentGroupClipIndex);
            }
        }

        /// <summary>
        /// Start playing the default clip
        /// </summary>
        public void Play()
        {
            FadeIn(_currentGroupIndex, _currentGroupClipIndex);
        }

        /// <summary>
        /// Start playing the given group - by index
        /// </summary>
        /// <param name="groupIndex"></param>
        public void PlayByGroupIndex(int groupIndex)
        {
            _currentGroupIndex = groupIndex;
            _currentGroupClipIndex = 0;
            FadeOut(true, _currentGroupIndex, _currentGroupClipIndex);
        }

        /// <summary>
        /// Start playing the given group - by name
        /// </summary>
        /// <param name="groupName"></param>
        public void PlayByGroupName(string groupName)
        {
            int index = 0;
            foreach (BackgroundMusicGroup group in backgroundMusicGroups)
            {
                if (group.GroupName.ToLower() == groupName.ToLower())
                {
                    PlayByGroupIndex(index);
                }

                index++;
            }
            Debug.Log($"BackgroundMusicPlayer: PlayByGroupName: Specified group not found: {groupName}");
        }

        /// <summary>
        /// Fade in the current clip
        /// </summary>
        /// <param name="groupIndex"></param>
        /// <param name="groupClipIndex"></param>
        private void FadeIn(int groupIndex, int groupClipIndex)
        {
            StartCoroutine(FadeInAsync(groupIndex, groupClipIndex));
        }

        /// <summary>
        /// Async coroutine to fade in an audiosource fromn 0 volume.
        /// </summary>
        /// <param name="groupIndex"></param>
        /// <param name="groupClipIndex"></param>
        /// <returns></returns>
        private IEnumerator FadeInAsync(int groupIndex, int groupClipIndex)
        {
            _inFade = true;
            _audioSource.Stop();
            _audioSource.volume = 0;
            _audioSource.clip = backgroundMusicGroups[groupIndex].GroupClips[groupClipIndex].AudioClip;
            _currentClipLength = _audioSource.clip.length;
            _audioSource.Play();
            _isPlaying = true;

            _currentClipGroupName = backgroundMusicGroups[groupIndex].GroupName;
            _currentClipName = backgroundMusicGroups[groupIndex].GroupClips[groupClipIndex].ClipName;

            float time = 0.0f;

            // Fade in to target volume
            while (time < fadeTimeInSeconds)
            {
                _audioSource.volume = Mathf.Lerp(0, 1.0f, time / fadeTimeInSeconds);
                time += Time.deltaTime;
                yield return null;
            }

            _audioSource.volume = 1.0f;
            _inFade = false;
        }

        /// <summary>
        /// Fade out the current clip and start a new one, if specified.
        /// </summary>
        /// <param name="playNext"></param>
        /// <param name="nextGroupIndex"></param>
        /// <param name="nextGroupClipIndex"></param>
        private void FadeOut(bool playNext, int nextGroupIndex, int nextGroupClipIndex)
        {
            StartCoroutine(FadeOutAsync(playNext, nextGroupIndex, nextGroupClipIndex));
        }

        /// <summary>
        /// Async Coroutine to fade an audiosource to 0 volume over time
        /// and start a new clip, if specified
        /// </summary>
        /// <param name="playNext"></param>
        /// <param name="nextGroupIndex"></param>
        /// <param name="nextGroupClipIndex"></param>
        /// <returns></returns>
        private IEnumerator FadeOutAsync(bool playNext, int nextGroupIndex, int nextGroupClipIndex)
        {
            _inFade = true;
            float startVolume = _audioSource.volume;
            float time = 0.0f;

            // Fade Out
            while (time < fadeTimeInSeconds)
            {
                _audioSource.volume = Mathf.Lerp(startVolume, 0.0f, time / fadeTimeInSeconds);
                time += Time.deltaTime;
                yield return null;
            }

            // If nothing left to do, exit
            if (!playNext)
            {
                _isPlaying = false;
                _inFade = false;
                yield break;
            }

            // Wait before fading in the new clip
            yield return new WaitForSecondsRealtime(delayInSecondsBetweenFades);

            // Fade in the next clip
            FadeIn(nextGroupIndex, nextGroupClipIndex);
        }

        [Serializable]
        public class BackgroundMusicGroup
        {
            [BoxGroup("Group Details")] public string GroupName;
            [BoxGroup("Group Clips")] public List<BackgroundMusicClip> GroupClips;
            [FoldoutGroup("Events")] public UnityEvent ClipStartEvent;
            [FoldoutGroup("Events")] public UnityEvent ClipFinishEvent;
        }

        [Serializable]
        public class BackgroundMusicClip
        {
            [BoxGroup("Clip Details")] public string ClipName;
            [BoxGroup("Clip Details")] public AudioClip AudioClip;
            [FoldoutGroup("Events")] public UnityEvent ClipStartEvent;
            [FoldoutGroup("Events")] public UnityEvent ClipFinishEvent;

            /// <summary>
            /// Clip Started
            /// </summary>
            public void ClipStarted()
            {
                ClipStartEvent.Invoke();
            }

            /// <summary>
            /// Clip Finished
            /// </summary>
            public void ClipFinished()
            {
                ClipFinishEvent.Invoke();
            }
        }
    }
}