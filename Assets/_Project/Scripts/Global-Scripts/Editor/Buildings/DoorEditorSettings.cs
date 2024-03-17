using DaftAppleGames.Common.Buildings;
using DaftAppleGames.Editor.Common;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Editor.Buildings
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "DoorEditorSettings", menuName = "Settings/Buildings/DoorEditor", order = 1)]
    public class DoorEditorSettings : EditorToolSettings
    {
        [Header("Search Strings")]
        public string[] nameSearchStrings;
        
        [Header("Door Settings")]
        public DoorPivotSide pivotSide = DoorPivotSide.Left;
        public bool swapTriggers = false;

        [Header("Door Trigger Settings")]
        public float colliderHeight = 1.0f; // y
        public float colliderWidth = 2.0f; // z
        public float colliderDepth = 1.0f; // x

        public float colliderxOffset = 1.0f;
        public float collideryOffset = 0.0f;
        public float colliderzOffset = 1.0f;

        public string[] triggerTags;
        
        [Header("Door Audio Settings")]
        public AudioMixerGroup audioMixerGroup;
        public AudioClip doorOpenClip;
        public AudioClip doorClosingClip;
        public AudioClip doorClosedClip;
        public bool autoOpen = true;

        [Header("Door Anim Settings")]
        public float openDuration = 1.5f;
        public float stayOpenDuration = 2.0f;
        public float closeDuration = 1.5f;
        public float openAngle = 100.0f;
    }
}