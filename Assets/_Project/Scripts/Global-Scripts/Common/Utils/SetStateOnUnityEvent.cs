using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class SetStateOnUnityEvent : MonoBehaviour
    {
        public enum UnityEventType { OnEnable, Awake, Start, OnDisable }

        [BoxGroup("Settings")] public bool gameObjectState;
        [BoxGroup("Settings")] public UnityEventType unityEventType;
        [BoxGroup("Objects")] public bool includeThisGameObject = true;
        [BoxGroup("Objects")] public GameObject[] objects;

        /// <summary>
        /// Set state on Enable
        /// </summary>
        private void OnEnable()
        {
            if (unityEventType == UnityEventType.OnEnable)
            {
                SetStates();
            }
        }

        /// <summary>
        /// Set state on Awake
        /// </summary>
        public void Awake()
        {
            if (unityEventType == UnityEventType.Awake)
            {
                SetStates();
            }
        }

        /// <summary>
        /// Set state on Start
        /// </summary>
        private void Start()
        {
            if (unityEventType == UnityEventType.Start)
            {
                SetStates();
            }
        }

        /// <summary>
        /// Set state on Start
        /// </summary>
        private void OnDisable()
        {
            if (unityEventType == UnityEventType.OnDisable)
            {
                SetStates();
            }
        }

        /// <summary>
        /// Sets the state of the given GameObjects
        /// </summary>
        private void SetStates()
        {
            foreach (GameObject currGameObject in objects)
            {
                currGameObject.SetActive(gameObjectState);
            }

            if (includeThisGameObject)
            {
                gameObject.SetActive(gameObjectState);
            }
        }
    }
}