using System.Collections;
using Invector.vShooter;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Ui
{
    /// <summary>
    /// MonoBehaviour class to manage display and tracking of tutorials
    /// </summary>
    public class PointOfInterest : MonoBehaviour
    {
        // Public serializable properties
        [BoxGroup("General Settings")] public InfoPanelContent infoPanelContent;
        [BoxGroup("General Settings")] public float fadeDuration = 2.0f;
        [SerializeField]
        [BoxGroup("Debug")] private bool _isRead = false;

        [BoxGroup("Settings")] public GameObject pointMarkerGameObject;
        [BoxGroup("Settings")] public GameObject undiscoveredMinimapMarker;
        [BoxGroup("Settings")] public GameObject discoveredMinimapMarker;

        [BoxGroup("Events")]  public UnityEvent onStartReadingEvent;
        [BoxGroup("Events")] public UnityEvent onFinishedReadingEvent;

        private AudioSource _markerAudioSource;
        private ParticleSystem[] _markerParticleSystems;

        public bool IsRead
        {
            set
            {
                _isRead = value;
                if (_isRead)
                {
                    pointMarkerGameObject.SetActive(false);
                    undiscoveredMinimapMarker.SetActive(false);
                    discoveredMinimapMarker.SetActive(true);
                }
            }
            get => _isRead;
        }

        // Private fields
        private InfoPanel _infoPanel;

        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            _infoPanel = GetComponentInChildren<InfoPanel>();
            _markerAudioSource = pointMarkerGameObject.GetComponent<AudioSource>();
            _markerParticleSystems = pointMarkerGameObject.GetComponentsInChildren<ParticleSystem>();
        }

        /// <summary>
        /// Displays the specified tutorial.
        /// </summary>
        public void Read()
        {
            // Populate and show the InfoPanel
            _infoPanel.headingText.text = infoPanelContent.heading;
            _infoPanel.contentText.text = infoPanelContent.content;
            if (infoPanelContent.image != null)
            {
                _infoPanel.image.sprite = infoPanelContent.image;
            }
            _infoPanel.ShowUi();
            onStartReadingEvent.Invoke();
        }

        /// <summary>
        /// Public method to close panel
        /// </summary>
        public void FinishedReading()
        {
            onFinishedReadingEvent.Invoke();
            // If first time reading, fade out the Fade Marker audio
            if (!IsRead)
            {
                StartCoroutine(FadeMarker());
            }
        }

        /// <summary>
        /// Fades the attached marker audio
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeMarker()
        {
            // Fade the particle systems
            foreach (ParticleSystem currParticleSystem in _markerParticleSystems)
            {
                ParticleSystem.MainModule main = currParticleSystem.main;
                main.loop = false;
            }

            // Fade the Audio
            float timeElapsed = 0.0f;
            float startVolume = _markerAudioSource.volume;
            while (timeElapsed < fadeDuration)
            {
                _markerAudioSource.volume = Mathf.Lerp(startVolume, 0, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            IsRead = true;
        }
    }
}