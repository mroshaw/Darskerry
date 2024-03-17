using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    public class EyeSync : MonoBehaviour
    {
        [Header("Proxy Eyes Settings")]
        public Transform leftEye;
        public Transform rightEye;

        [Header("Blendshapes Settings")]
        public SkinnedMeshRenderer blendShapesSource;
        public string lookLeftBlendshape;
        public string lookRightBlendshape;
        public string lookUpBlendshape;
        public string lookDownBlendshape;

        [Header("Movement Settings")]
        public float movementMultiplier = 1.0f;
        
        private int leftBlendShapeIndex;
        private int rightBlendShapeIndex;
        private int upBlendShapeIndex;
        private int downBlendShapeIndex;

        private Vector3 _lookDir;
        private float _lookLeftRight;
        private float _lookUpDown;

        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            leftBlendShapeIndex = blendShapesSource.sharedMesh.GetBlendShapeIndex(lookLeftBlendshape);
            rightBlendShapeIndex = blendShapesSource.sharedMesh.GetBlendShapeIndex(lookRightBlendshape);
            upBlendShapeIndex = blendShapesSource.sharedMesh.GetBlendShapeIndex(lookUpBlendshape);
            downBlendShapeIndex = blendShapesSource.sharedMesh.GetBlendShapeIndex(lookDownBlendshape);

            Debug.Log($"{leftBlendShapeIndex}, {rightBlendShapeIndex}, {upBlendShapeIndex}, {downBlendShapeIndex}");
        }

        /// <summary>
        /// Sync the proxy eyeball rotations to the blend shapes
        /// </summary>
        void FixedUpdate()
        {
            //
            float localLeftRightRotation = leftEye.transform.localEulerAngles.y;
            float localUpDownRotation = leftEye.transform.localEulerAngles.x;

            if (localLeftRightRotation > 25.0f || localUpDownRotation > 25.0f)
            {
                return;
            }
            
            UpdateHorizontal(localLeftRightRotation);
            UpdateVertical(localUpDownRotation);
        }

        private void UpdateHorizontal(float yValue)
        {
            // Looking left
            if (yValue < 0)
            {
                blendShapesSource.SetBlendShapeWeight(rightBlendShapeIndex, 0);
                blendShapesSource.SetBlendShapeWeight(leftBlendShapeIndex, (-yValue / 25) * 100.0f);
                return;
            }
            
            // Looking right
            if (yValue > 0)
            {
                blendShapesSource.SetBlendShapeWeight(leftBlendShapeIndex, 0);
                blendShapesSource.SetBlendShapeWeight(rightBlendShapeIndex, (yValue / 25) * 100.0f);
                return;
            }
            
            // No LeftRight
            blendShapesSource.SetBlendShapeWeight(rightBlendShapeIndex, 0);
            blendShapesSource.SetBlendShapeWeight(leftBlendShapeIndex, 0);
        }

        private void UpdateVertical(float xValue)
        {
            // Look up
            if (xValue < 0)
            {
                blendShapesSource.SetBlendShapeWeight(downBlendShapeIndex, 0);
                blendShapesSource.SetBlendShapeWeight(upBlendShapeIndex, (-xValue / 25) * 100.0f);
                return;
            }

            // Look down
            if (xValue > 0)
            {
                blendShapesSource.SetBlendShapeWeight(downBlendShapeIndex, (xValue / 25) * 100.0f);
                blendShapesSource.SetBlendShapeWeight(upBlendShapeIndex, 0);
                return;
            }

            // No UpDown
            blendShapesSource.SetBlendShapeWeight(upBlendShapeIndex, 0);
            blendShapesSource.SetBlendShapeWeight(downBlendShapeIndex, 0);
        }
    }
}