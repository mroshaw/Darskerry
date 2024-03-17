using UnityEngine;

namespace DaftAppleGames.Common.AI
{
    public class LookAt : MonoBehaviour
    {

        public Transform head = null;
        public Vector3 lookAtTargetPosition;
        public float lookAtCoolTime = 0.2f;
        public float lookAtHeatTime = 0.2f;
        public bool looking = true;

        private Vector3 _lookAtPosition;
        private Animator _animator;
        private float _lookAtWeight = 0.0f;


        private void Start()
        {
            if (!head)
            {
                Debug.LogError("No head transform - LookAt disabled");
                enabled = false;
                return;
            }
            _animator = GetComponent<Animator>();
            lookAtTargetPosition = head.position + transform.forward;
            _lookAtPosition = lookAtTargetPosition;
        }

        private void OnAnimatorIK()
        {
            lookAtTargetPosition.y = head.position.y;
            float lookAtTargetWeight = looking ? 1.0f : 0.0f;

            Vector3 curDir = _lookAtPosition - head.position;
            Vector3 futDir = lookAtTargetPosition - head.position;

            curDir = Vector3.RotateTowards(curDir, futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
            _lookAtPosition = head.position + curDir;

            float blendTime = lookAtTargetWeight > _lookAtWeight ? lookAtHeatTime : lookAtCoolTime;
            _lookAtWeight = Mathf.MoveTowards(_lookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
            _animator.SetLookAtWeight(_lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
            _animator.SetLookAtPosition(_lookAtPosition);
        }
    }
}
