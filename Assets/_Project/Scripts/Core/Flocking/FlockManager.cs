using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using DaftAppleGames.Attributes;
#endif

namespace DaftAppleGames.Flocking
{
    public class FlockManager : MonoBehaviour
    {
        #region Class Variables
        [SerializeField] private int numberOfBirds;
        [SerializeField] private GameObject birdPrefab;
        [SerializeField] private float flockRadius;

        [SerializeField] [Range(0f, 1f)] private float cohesionRuleWeight;
        [SerializeField] [Range(0f, 1f)] private float alignmentRuleWeight;
        [SerializeField] [Range(0f, 1f)] private float separationRuleWeight;
        [SerializeField] [Range(0f, 1f)] private float borderAvoidanceRuleWeight;

        private Bird[] birds;

        internal Vector3 AverageHeader => _averageHeading;
        internal Vector3 CenterOfMass => _centerOfMass;
        private Vector3 _averageHeading;
        private Vector3 _centerOfMass;

        #endregion

        #region Startup
        /// <summary>
        /// Subscribe to events
        /// </summary>   
        private void OnEnable()
        {
            
        }

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            birds = new Bird[numberOfBirds];
        }

        private void Start()
        {
            for (int i = 0; i < numberOfBirds; i++)
            {
                Vector3 birdPosition = Random.insideUnitSphere * flockRadius;
                Vector3 birdRotation = Random.insideUnitSphere * 20;
                Bird bird = Instantiate(birdPrefab, birdPosition, Quaternion.Euler(birdRotation), transform).GetComponent<Bird>();
                bird.SetFlockManager(this);
                bird.SetWeights(separationRuleWeight, cohesionRuleWeight, alignmentRuleWeight, borderAvoidanceRuleWeight);
                bird.SetFlockOrigin(transform.position);
                bird.SetFlockRadius(flockRadius);

                birds[i] = bird;
            }
        }
        #endregion

        #region Update
        private void Update()
        {
            Vector3 subBirdPosition = Vector3.zero;
            Vector3 sumHeading = Vector3.zero;

            foreach (Bird bird in birds)
            {
                Transform birdTransform = bird.transform;
                subBirdPosition += birdTransform.position;
                sumHeading += birdTransform.forward.normalized;
            }

            _centerOfMass = subBirdPosition / numberOfBirds;
            _averageHeading = sumHeading / numberOfBirds;
        }

        private void LateUpdate()
        {
            
        }
        #endregion

        #region Class methods
        public Vector3 GetAverageHeading()
        {
            return _averageHeading;
        }

        public Vector3 GetCenterofMass()
        {
            return _centerOfMass;
        }
        #endregion
    }
}