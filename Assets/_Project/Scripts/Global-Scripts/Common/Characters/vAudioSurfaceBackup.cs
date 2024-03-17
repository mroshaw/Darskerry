/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Invector
{
    public class vAudioSurface : ScriptableObject
    {
        public AudioMixerGroup audioMixerGroup;                 // The AudioSource that will play the clips.   
        public List<string> TextureOrMaterialNames;             // The tag on the surfaces that play these sounds.
        public List<AudioClip> audioClips;                      // The different clips that can be played on this surface.    

        [Header("Pool Prefabs")]
        public GameObject audioSourcePrefab;
        public GameObject particleFxPrefab;
        public GameObject stepPrefab;

        // Added for Object Pooling
        private ObjectPool<GameObject> _audioSourcePool;
        private ObjectPool<GameObject> _particlePool;
        private ObjectPool<GameObject> _stepMarkPool;

        private vFisherYatesRandom randomSource = new vFisherYatesRandom();       // For randomly reordering clips.   

        public LayerMask stepLayer;
        public float timeToDestroy = 5f;

        public vAudioSurface()
        {
            audioClips = new List<AudioClip>();
            TextureOrMaterialNames = new List<string>();
        }
        /// <summary>
        /// Spawn surface effect
        /// </summary>
        /// <param name="footStepObject">step object surface info</param>
        /// <param name="playSound">Spawn sound effect</param>
        /// <param name="spawnParticle">Spawn particle effect</param>
        /// <param name="spawnStepMark">Spawn step Mark effect</param>

#region Pooling
        private GameObject AudioSourceCreatePoolItem()
        {
            GameObject newAudioSourceGameObject = Instantiate(audioSourcePrefab);
            newAudioSourceGameObject.name = string.Format("AudioSourcePool({0})", Time.fixedTime);
            return newAudioSourceGameObject;
        }

        private void AudioSourceOnTakeFromPool(GameObject audioSourceGameObject)
        {
            audioSourceGameObject.SetActive(true);
        }

        private void AudioSourceOnReturnToPool(GameObject audioSourceGameObject)
        {
            if (audioSourceGameObject.gameObject.transform)
            {
                audioSourceGameObject.gameObject.transform.position = Vector3.zero;
                audioSourceGameObject.gameObject.transform.rotation = Quaternion.identity;
            }
            if (audioSourceGameObject.gameObject)
            {
                audioSourceGameObject.gameObject.SetActive(false);
            }
        }

        private void AudioSourceOnDestroyPoolObject(GameObject audioSourceGameObject)
        {
            Destroy(audioSourceGameObject.gameObject);
        }

        private IEnumerator AudioSourceReturnToPoolAsync(GameObject audioSourceGameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            _audioSourcePool.Release(audioSourceGameObject);
            yield break;
        }

        private IEnumerator ParticleReturnToPoolAsync(GameObject particleGameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            _particlePool.Release(particleGameObject);
            yield break;
        }

        private IEnumerator StepMarkReturnToPoolAsync(GameObject stepMarkGameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            _stepMarkPool.Release(stepMarkGameObject);
            yield break;
        }

        private GameObject ParticleCreatePoolItem()
        {
            GameObject gameObject = Instantiate(particleFxPrefab);
            gameObject.name = string.Format("ParticlePool({0})", Time.fixedTime);
            return gameObject;
        }

        private void ParticleOnTakeFromPool(GameObject particleGameObject)
        {
            particleGameObject.SetActive(true);
        }

        private void ParticleOnReturnToPool(GameObject particleGameObject)
        {
            particleGameObject.transform.position = Vector3.zero;
            particleGameObject.transform.rotation = Quaternion.identity;
            particleGameObject.SetActive(false);
        }

        private void ParticleOnDestroyPoolObject(GameObject particleGameObject)
        {
            Destroy(particleGameObject);
        }

        private GameObject StepMarkCreatePoolItem()
        {
            GameObject gameObject = Instantiate<GameObject>(stepPrefab);
            gameObject.name = string.Format("StepMarkPool({0})", Time.fixedTime);
            return gameObject;
        }

        private void StepMarkOnTakeFromPool(GameObject stepMarkGameObject)
        {
            stepMarkGameObject.SetActive(true);
        }

        private void StepMarkOnReturnToPool(GameObject stepMarkGameObject)
        {
            stepMarkGameObject.transform.position = Vector3.zero;
            stepMarkGameObject.transform.rotation = Quaternion.identity;
            stepMarkGameObject.SetActive(false);
        }

        private void StepMarkOnDestroyPoolObject(GameObject stepMarkGameObject)
        {
            Destroy(stepMarkGameObject);
        }

        public void OnEnable()
        {
            _audioSourcePool = new ObjectPool<GameObject>(new Func<GameObject>(this.AudioSourceCreatePoolItem), new Action<GameObject>(this.AudioSourceOnTakeFromPool), new Action<GameObject>(this.AudioSourceOnReturnToPool), new Action<GameObject>(this.AudioSourceOnDestroyPoolObject), false, 10, 20);
            _particlePool = new ObjectPool<GameObject>(new Func<GameObject>(this.ParticleCreatePoolItem), new Action<GameObject>(this.ParticleOnTakeFromPool), new Action<GameObject>(this.ParticleOnReturnToPool), new Action<GameObject>(this.ParticleOnDestroyPoolObject), false, 10, 20);
            _stepMarkPool = new ObjectPool<GameObject>(new Func<GameObject>(this.StepMarkCreatePoolItem), new Action<GameObject>(this.StepMarkOnTakeFromPool), new Action<GameObject>(this.StepMarkOnReturnToPool), new Action<GameObject>(this.StepMarkOnDestroyPoolObject), false, 10, 20);
        }
#endregion Pooling
        public virtual void SpawnSurfaceEffect(FootStepObject footStepObject)
        {
            
            // initialize variable if not already started
            if (randomSource == null)
            {
                randomSource = new vFisherYatesRandom();
            }          
            ///Create audio Effect
            if(footStepObject.spawnSoundEffect && audioSourcePrefab)
            {
                PlaySound(footStepObject);
            }
            ///Create particle Effect
            if (footStepObject.spawnParticleEffect && particleFxPrefab) // && footStepObject.ground && stepLayer.ContainsLayer(footStepObject.ground.gameObject.layer))
            {
                SpawnParticle(footStepObject);
            }
            ///Create Step Mark Effect
            if (footStepObject.spawnStepMarkEffect && stepPrefab)
            {
                StepMark(footStepObject);
            }          
        }

       
        /// <summary>
        /// Spawn Sound effect
        /// </summary>
        /// <param name="footStepObject">Step object surface info</param>      
        protected virtual void PlaySound(FootStepObject footStepObject)
        {
            if (audioClips == null || audioClips.Count == 0)
            {
                return;
            }
            GameObject audioSourceGameObject = _audioSourcePool.Get();
            AudioSource audioSource = audioSourceGameObject.GetComponent<AudioSource>();
            audioSource.gameObject.transform.position = footStepObject.sender.position;
            audioSource.gameObject.transform.rotation = Quaternion.identity;
            audioSource.gameObject.transform.SetParent(vObjectContainer.root, true);

            if (audioMixerGroup != null)
            {
                audioSource.outputAudioMixerGroup = audioMixerGroup;
            }
            int index = randomSource.Next(audioClips.Count);
            audioSource.PlayOneShot(audioClips[index], footStepObject.volume);
            CoroutineController.Start(AudioSourceReturnToPoolAsync(audioSourceGameObject, 5f));
        }
        /// <summary>
        /// Spawn Particle effect
        /// </summary>
        /// <param name="footStepObject">Step object surface info</param>
        protected virtual void SpawnParticle(FootStepObject footStepObject)
        {
            GameObject gameObject = _particlePool.Get();
            gameObject.transform.position = footStepObject.sender.position;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.SetParent(vObjectContainer.root, true);
            CoroutineController.Start(ParticleReturnToPoolAsync(gameObject, 5f));
        }
        /// <summary>
        /// Spawn Step Mark effect
        /// </summary>
        /// <param name="footStepObject">Step object surface info</param>
        protected virtual void StepMark(FootStepObject footStep)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(footStep.sender.transform.position + new Vector3(0f, 0.25f, 0f), Vector3.down, out raycastHit, 1f, this.stepLayer))
            {
                Quaternion lhs = Quaternion.FromToRotation(footStep.sender.up, raycastHit.normal);
                GameObject gameObject = _stepMarkPool.Get();
                gameObject.transform.position = raycastHit.point;
                gameObject.transform.rotation = lhs * footStep.sender.rotation;
                gameObject.transform.SetParent(vObjectContainer.root, true);
                CoroutineController.Start(StepMarkReturnToPoolAsync(gameObject, 5f));
            }
        }

        private class CoroutineController : MonoBehaviour
        {

            static CoroutineController _singleton;
            static Dictionary<string, IEnumerator> _routines = new Dictionary<string, IEnumerator>(100);

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            static void InitializeType()
            {
                _singleton = new GameObject($"#{nameof(CoroutineController)}").AddComponent<CoroutineController>();
                DontDestroyOnLoad(_singleton);
            }

            public static Coroutine Start(IEnumerator routine) => _singleton.StartCoroutine(routine);
            public static Coroutine Start(IEnumerator routine, string id)
            {
                var coroutine = _singleton.StartCoroutine(routine);
                if (!_routines.ContainsKey(id)) _routines.Add(id, routine);
                else
                {
                    _singleton.StopCoroutine(_routines[id]);
                    _routines[id] = routine;
                }
                return coroutine;
            }
            public static void Stop(IEnumerator routine) => _singleton.StopCoroutine(routine);
            public static void Stop(string id)
            {
                if (_routines.TryGetValue(id, out var routine))
                {
                    _singleton.StopCoroutine(routine);
                    _routines.Remove(id);
                }
                else Debug.LogWarning($"coroutine '{id}' not found");
            }
            public static void StopAll() => _singleton.StopAllCoroutines();

        }
    }

}
*/