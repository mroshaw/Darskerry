using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Darskerry.Core.PlayerController.FootSteps
{
    public class FootstepManager : MonoBehaviour
    {
        #region Class Variables

        [BoxGroup("Settings")][SerializeField] private bool enableFootsteps;
        [BoxGroup("Settings")][SerializeField] private List<FootstepTrigger> footstepTriggers;
        [BoxGroup("Settings")][SerializeField] private FootstepSurface[] footstepSurfaces;
        [BoxGroup("Settings")][SerializeField] private FootstepSurface defaultSurface;
        [BoxGroup("Settings")][SerializeField] private LayerMask triggerLayerMask;
        [BoxGroup("Settings")][SerializeField] private AudioMixerGroup audioMixerGroup;

        [BoxGroup("Spawn Settings")] public bool alignToTerrainSlope;

        [BoxGroup("Pool Settings")][SerializeField] public PrefabPool particleFxPool;
        [BoxGroup("Pool Settings")][SerializeField] public PrefabPool decalPool;

        [BoxGroup("Debug")][SerializeField] public bool DebugTextureName { get; private set; }
        #endregion

        public bool FootstepsEnabled => enableFootsteps;

        #region Startup
        private void Awake()
        {
            // Register the triggers
            foreach (FootstepTrigger trigger in footstepTriggers)
            {
                if ((trigger))
                {
                    trigger.FootstepManager = this;
                }
            }
        }
        #endregion

        #region Class methods

        public FootstepSurface GetDefaultSurface()
        {
            return defaultSurface;
        }

        public FootstepSurface[] GetAllSurfaces()
        {
            return footstepSurfaces;
        }

        [Button("Create Triggers")]
        private void CreateFootstepTriggers()
        {
            if (!GetComponentInChildren<Animator>())
            {
                return;
            }

            footstepTriggers = new List<FootstepTrigger>
            {
                CreateFootstepTriggerOnFoot(HumanBodyBones.LeftFoot),
                CreateFootstepTriggerOnFoot(HumanBodyBones.RightFoot)
            };
        }

        private FootstepTrigger CreateFootstepTriggerOnFoot(HumanBodyBones footBone)
        {
            Animator animator = GetComponentInChildren<Animator>();
            Transform footTransform = animator.GetBoneTransform(footBone);

            FootstepTrigger existingTrigger = footTransform.gameObject.GetComponentInChildren<FootstepTrigger>();
            if (existingTrigger)
            {
                DestroyImmediate(existingTrigger.gameObject);
            }

            GameObject newTrigger = new GameObject($"Footstep Trigger {footBone}");
            newTrigger.transform.SetParent(footTransform);
            newTrigger.transform.localPosition = Vector3.zero;
            newTrigger.transform.localScale = Vector3.one;
            FootstepTrigger newFootStepTrigger = newTrigger.AddComponent<FootstepTrigger>();
            newFootStepTrigger.triggerLayers = triggerLayerMask;
            SphereCollider newSphereCollider = newTrigger.AddComponent<SphereCollider>();
            newSphereCollider.radius = 0.1f;
            newSphereCollider.isTrigger = true;
            newSphereCollider.center = new Vector3(0, -0.1f, 0.0f);
            AudioSource newAudioSource = newTrigger.AddComponent<AudioSource>();
            newAudioSource.loop = false;
            newAudioSource.outputAudioMixerGroup = audioMixerGroup;
            newAudioSource.spatialBlend = 1.0f;

            return newFootStepTrigger;
        }

        public void Enable()
        {
            foreach (FootstepTrigger trigger in footstepTriggers)
            {
                trigger.gameObject.SetActive(true);
            }
        }

        public void Disable()
        {
            foreach (FootstepTrigger trigger in footstepTriggers)
            {
                trigger.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}