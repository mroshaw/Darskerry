using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    public class WaterSplash : MonoBehaviour
    {
        // Public serializable properties
        [Header("General Settings")]
        public List<AudioClip> splashClips;
        public GameObject splashFx;

        [Header("Raycast Settings")]
        public Transform originTransform;
        public float raycastLength = 2.0f;
        public LayerMask waterLayer;

        private GameObject _audioSourceGo;
        private AudioSource _audioSource;
        private int _numClips;
        private WaterTrigger _waterTrigger;
        
        private void Start()
        {
            _numClips = splashClips.Count;
            _audioSource = GetComponent<AudioSource>();
            _waterTrigger = transform.root.GetComponentInChildren<WaterTrigger>();

            if (!_waterTrigger)
            {
                Debug.Log("Can't find WaterTrigger!");
            }
        }

        /// <summary>
        /// If is in water, play a random splash clip and spawn FX on surface
        /// </summary>
        /// <param name="other"></param>
        public void CheckWaterSplash(Collider other)
        {
            if (_waterTrigger.IsInWater)
            {
                PlayRandomClip();
                SpawnFxOnSurface();
            }
        }

        /// <summary>
        /// Spawn splash effects on surface
        /// </summary>
        private void SpawnFxOnSurface()
        {
            if (Physics.Raycast(originTransform.position, Vector3.down, out RaycastHit hit,
                    raycastLength, waterLayer))
            {
                GameObject newSplashFx = Instantiate(splashFx, hit.point, hit.collider.gameObject.transform.rotation, hit.collider.gameObject.transform);
                newSplashFx.name = "SplashFx";
            }
        }
        
        /// <summary>
        /// Play random audio clip
        /// </summary>
        private void PlayRandomClip()
        {
            /*
            _audioSourceGo = vAudioSurfacePooling.AudioSourcePool.Get();
            _audioSourceGo.GetComponent<AudioSource>().PlayOneShot(splashClips[GetRandomClipIndex()]);
            vAudioSurfacePooling.AudioSourceReturnToPool(_audioSourceGo);
            */
            _audioSource.PlayOneShot(splashClips[GetRandomClipIndex()]);
        }
        
        /// <summary>
        /// Get random clip index
        /// </summary>
        /// <returns></returns>
        private int GetRandomClipIndex()
        {
            System.Random random = new System.Random();
            return random.Next(0, _numClips);
        }
    }
}
