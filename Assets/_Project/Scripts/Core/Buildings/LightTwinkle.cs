using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    public class LightTwinkle : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Settings")] [SerializeField] private float baseIntensity = 0.0f;
        [BoxGroup("Settings")] [SerializeField] private Light torchLight;
        [BoxGroup("Settings")] [SerializeField] private Renderer fire;
        [BoxGroup("Settings")] [SerializeField] private ParticleSystem fireParticles;

        private Material _fireSource;

        #endregion

        #region Startup

        private void Start()
        {
            baseIntensity = torchLight.intensity;
        }
        #endregion

        #region Update Logic
        private void Update()
        {
            if (torchLight && fire)
            {
                torchLight.transform.position = (torchLight.transform.position*0.7f)+(fire.bounds.center*0.3f);
                torchLight.intensity = baseIntensity + fire.bounds.size.magnitude;
            }
            _fireSource =  fireParticles.GetComponent<Renderer>().material;

            if(_fireSource)
            {
                float val = 2.3f + fire.bounds.size.magnitude/3;
                _fireSource.SetVector("_EmissionColor", new Vector4(val, val,val, val));
            }

        }
        #endregion
    }
}