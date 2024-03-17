#if BEAUTIFY3
#if HDPipeline
using BeautifyHDRP;
#else
using BeautifyEffect;
#endif
using UnityEngine;

namespace DaftAppleGames.Common.Environment 
{
    public class BeautifyHelper : MonoBehaviour
    {
        [Header("Beautify Settings")]
        #if HDPipeline
        #else
        public BeautifyProfile[] profiles;
        #endif        
        private Beautify _beautify;

        private void Start()
        {
            FindBeautify();
        }

        /// <summary>
        /// Public method to set a new Beautify profile
        /// </summary>
        /// <param name="profileIndex"></param>
        public void EnableProfile(int profileIndex)
        {
#if HDPipeline
#else
            _beautify.profile = profiles[profileIndex];
#endif
        }
        
        // Public Instance for accessing the full API
        public Beautify BeautifyInstance => _beautify;

        /// <summary>
        /// Enables the Beautify component
        /// </summary>
        public void Enable()
        {
            if (_beautify)
            {
#if HDPipeline
                
#else
                _beautify.enabled = true;
#endif
            }
        }

        /// <summary>
        /// Disabled the Beautify component
        /// </summary>
        public void Disable()
        {
            if (_beautify)
            {
#if HDPipeline
      
#else
                _beautify.enabled = false;
#endif
            }
        }

        /// <summary>
        /// Find the Beautify component instance
        /// </summary>
        private void FindBeautify()
        {
            _beautify = FindObjectOfType<Beautify>();
        }
    }
}
#endif