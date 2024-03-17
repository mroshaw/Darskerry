using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.CameraTools
{
    public class RotateCameraAroundObject : MonoBehaviour
    {
        [BoxGroup("Rotate Settings")]
        public Transform targetTransform;
        [BoxGroup("Rotate Settings")]
        public float rotateSpeed = -1.5f;
        [BoxGroup("Rotate Settings")]
        public float cameraDistance = 270.0f;
        [BoxGroup("Rotate Settings")]
        public float startOffset = 0;
        [BoxGroup("Rotate Settings")]
        public bool rotateOnStart = true;
        
        [BoxGroup("Height Settings")]
        public bool useTargetHeight = true;
        [BoxGroup("Height Settings")]
        public float cameraHeightOffset = -5.0f;
        [BoxGroup("Height Settings")]
        public float cameraHeightGoal = 20.0f;

        [BoxGroup("Terrain Avoidance")]
        public bool maintainHeightAboveTerrain = false;
        [BoxGroup("Terrain Avoidance")]
        public float terrainHeightMin;
        [BoxGroup("Terrain Avoidance")]
        public float terrainHeightModifier;
        [BoxGroup("Terrain Avoidance")]
        public Transform terrainRaycastSource;

        [BoxGroup("Debug")]
        public bool showRay = true;

        private Camera _camera;
        private bool _running = false;

        private Collider _terrainCollider;

        
        /// <summary>
        /// Initialise the Camera
        /// </summary>
        private void Start()
        {
            _camera = GetComponent<Camera>();
            if(!_camera)
            {
                Debug.LogError("RotateCameraAroundObject: No Camera! Check your GameObject!");
            }
            
            AlignCamera();
            if (rotateOnStart)
            {
                _running = true;
            }
        }

        /// <summary>
        /// Rotate the camera
        /// </summary>
        void Update()
        {
            if(_running)
            {
                AlignAndRotate();
            }
        }
        
        /// <summary>
        /// Rotates the camera and re-aligns to look at the transform
        /// </summary>
        private void AlignAndRotate()
        {
            // Rotate around target object
            _camera.transform.RotateAround(targetTransform.position, Vector3.up, rotateSpeed * Time.deltaTime);
            
            // Maintain distance
            SetDistance();
            
            // Maintain height
            SetHeight(false);
            
            // Continue to face the target
            _camera.transform.LookAt(targetTransform.position);
        }

        /// <summary>
        /// Aligns our cameras
        /// </summary>
        private void AlignCamera()
        {
            // Set initial camera position
            _camera.transform.LookAt(targetTransform);
            SetDistance();
            SetHeight(true);
            if (startOffset != 0)
            {
                _camera.transform.Translate(Vector3.left * startOffset);
                _camera.transform.LookAt(targetTransform);
                SetDistance();
                SetHeight(true);
            }
        }

        /// <summary>
        /// Set the camera height
        /// </summary>
        private void SetHeight(bool immediate)
        {
            // Get the current and goal heights
            float currentHeight = _camera.transform.position.y;
            cameraHeightGoal = GetTargetHeight();
            
            // Calculate height delta
            float deltaHeight = cameraHeightGoal - currentHeight;
            
            // If there's a change required, make it over time
            if(deltaHeight != 0)
            {
                // Increase camera height
                Vector3 targetPosition = _camera.transform.position + (Vector3.up * deltaHeight);
                if (deltaHeight > 0.1 && !immediate)
                {
                    _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    _camera.transform.position = targetPosition;
                }
            }
        }

        /// <summary>
        /// Gets the goal camera height relative to the terrain
        /// </summary>
        /// <returns></returns>
        private float GetTerrainHeight()
        {
            // Create ray for raycast from detector transform
            Ray rayDown = new Ray();
            rayDown.origin = terrainRaycastSource.position;
            rayDown.direction = Vector3.down;
            
            // Draw debug ray, if required
            if (showRay)
            {
                Debug.DrawRay(terrainRaycastSource.position, Vector3.down * 1000.0f, Color.red);
            }

            if (!_terrainCollider && Terrain.activeTerrain)
            {
                _terrainCollider = Terrain.activeTerrain.GetComponent<Collider>();
            }

            if (_terrainCollider)
            {
                bool rayHit = _terrainCollider.Raycast(rayDown, out RaycastHit hit, 1000.0f);
            
                // If no hit, return 0. We'll ignore terrain
                if (!rayHit)
                {
                    return 0.0f;
                }
            
                // Get the height of the terrain and return
                float terrainHeight = Terrain.activeTerrain.SampleHeight(hit.point);
                return terrainHeight;
            }

            return 0.0f;
        }
        
        /// <summary>
        /// Make any changes required to the camera to maintain the specified distance
        /// </summary>
        private void SetDistance()
        {
            float distance = Vector3.Distance(_camera.transform.position, targetTransform.position);
            float deltaDistance = cameraDistance - distance;
            if (deltaDistance != 0)
            {
                _camera.transform.Translate(Vector3.back * deltaDistance);
            }
        }

        /// <summary>
        /// Gets the desired target height for the camera
        /// </summary>
        private float GetTargetHeight()
        {
            // If we're tracking terrain, see if we need to move
            if (maintainHeightAboveTerrain)
            {
                float terrainHeight = GetTerrainHeight();
                if (terrainHeight > cameraHeightGoal - terrainHeightMin)
                {
                    return terrainHeight + terrainHeightModifier;
                }
                return GetObjectHeight();
            }
            return GetObjectHeight();
        }

        /// <summary>
        /// Returns the camera height relative to the target object
        /// </summary>
        /// <returns></returns>
        private float GetObjectHeight()
        {
            float targetHeight = 0.0f;

            if (useTargetHeight)
            {
                targetHeight = targetTransform.position.y;
            }
            else
            {
                targetHeight = cameraHeightGoal;
            }

            if (cameraHeightOffset != 0.0f)
            {
                targetHeight += cameraHeightOffset;
            }

            return targetHeight;
        }

        /// <summary>
        /// Resume the rotation
        /// </summary>
        [Button("Resume")]
        public void Resume()
        {
            _running = true;
        }

        /// <summary>
        /// Pause the rotation
        /// </summary>
        [Button("Pause")]
        public void Pause()
        {
            _running = false;
        }
    }
}