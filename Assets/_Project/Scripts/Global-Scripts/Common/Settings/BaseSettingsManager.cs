using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Settings
{
    public class BaseSettingsManager : MonoBehaviour
    {
        // Public serializable properties
        [BoxGroup("General Settings")] public bool applyOnStart = false;
        [BoxGroup("General Settings")] public bool applyOnAwake = true;

        [FoldoutGroup("Events")]
        public UnityEvent onSettingsAppliedEvent;
        [FoldoutGroup("Events")]
        public UnityEvent onSettingsLoadedEvent;
        [FoldoutGroup("Events")]
        public UnityEvent onSettingsSavedEvent;
        
        /// <summary>
        /// Awake event
        /// </summary>
        public virtual void Awake()
        {
            InitSettings();
            LoadSettings();
            
            if (applyOnAwake)
            {
                LoadAndApplySettings();
            }
        }

        /// <summary>
        /// Start event
        /// </summary>
        public virtual void Start()
        {
            if (applyOnStart)
            {
                LoadAndApplySettings();
            }
        }

        /// <summary>
        /// Do any setting initialisation
        /// </summary>
        public virtual void InitSettings()
        {
            
        }
        
        /// <summary>
        /// Apply all settings
        /// </summary>
        public virtual void ApplySettings()
        {
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Save all settings
        /// </summary>
        public virtual void SaveSettings()
        {
            onSettingsSavedEvent.Invoke();
        }

        /// <summary>
        /// Load all settings
        /// </summary>
        public virtual void LoadSettings()
        {
            onSettingsLoadedEvent.Invoke();
        }
        
        /// <summary>
        /// Load then apply settings
        /// </summary>
        public virtual void LoadAndApplySettings()
        {
            LoadSettings();
            ApplySettings();
        }
    }
}
