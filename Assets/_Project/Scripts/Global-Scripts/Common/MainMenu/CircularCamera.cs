using UnityEngine;

namespace DaftAppleGames.Common.MainMenu
{

    public class CircularCamera : MonoBehaviour
    {
        [Header("Settings")]
        public Transform centerTransform;
        public float rotationspeed = 0.1f;
        public float radius = 50.0f;
        public float elevationOffset = 0;

        private Vector3 positionOffset;
        private float angle;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            positionOffset.Set(
            Mathf.Cos(angle) * radius,
                elevationOffset,
                Mathf.Sin(angle) * radius
            );
            transform.position = centerTransform.position + positionOffset;
            angle += Time.deltaTime * rotationspeed;
            transform.LookAt(centerTransform.position);
        }
    }
}