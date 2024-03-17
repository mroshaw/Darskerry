#if MAGICA
#if THE_VEGETATION_ENGINE
using System.Collections;
using MagicaCloth2;
using TheVegetationEngine;
using UnityEngine;

namespace DaftAppleGames.Common.Environment
{
    /// <summary>
    /// Component to synchronise Magica Cloth wind zone to TVE wind
    /// </summary>
    public class MagicaTveWindSync : MonoBehaviour
    {
        // Public serializable properties
        [Header("General Settings")]
        public float syncEverynSeconds;
        private MagicaWindZone _magicaWind;
        private float _modifier;
        
        private Coroutine _coroutine;
        
        /// <summary>
        /// Get necessary components
        /// </summary>
        private void Start()
        {
            _magicaWind = GetComponent<MagicaWindZone>();
            
            // Magic "top speed" is 30
            // TVE "top speed" is 1
            _modifier = 30f;
            
            // Start recursive sync call
            _coroutine = StartCoroutine(SyncWindAsync());
        }
        
        /// <summary>
        /// Async Coroutine to wait and then call the sync
        /// </summary>
        /// <returns></returns>
        private IEnumerator SyncWindAsync()
        {
            yield return new WaitForSeconds(syncEverynSeconds);
            SyncWind();
            StartCoroutine(SyncWindAsync());
        }

        /// <summary>
        /// /// Stop the main coroutine on Destroy
        /// </summary>
        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        /// <summary>
        /// Stop the main coroutine on Disable
        /// </summary>
        private void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        /// <summary>
        /// Sync the wind
        /// </summary>
        private void SyncWind()
        {
            if (TVEManager.Instance == null)
            {
                return;
            }
            _magicaWind.main = TVEManager.Instance.globalMotion.windPower * _modifier;
            _magicaWind.directionAngleX = TVEManager.Instance.globalMotion.mainDirection.transform.rotation.eulerAngles.x;
            _magicaWind.directionAngleY = TVEManager.Instance.globalMotion.mainDirection.transform.rotation.eulerAngles.y;
        }
    }
}
#endif
#endif