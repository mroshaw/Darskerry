using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.CameraUtils
{
    public class UnparentGameObject : MonoBehaviour
    {
        [BoxGroup("Settings")] [SerializeField] private bool unParentOnStart = true;
        [BoxGroup("Settings")] [SerializeField] private bool unParentOnAwake = false;

        private void Start()
        {
            if (unParentOnStart)
            {
                gameObject.transform.SetParent(null);
            }
        }

        private void Awake()
        {
            if (unParentOnAwake)
            {
                gameObject.transform.SetParent(null);
            }
        }
    }
}