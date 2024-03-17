#if INVECTOR_SHOOTER
using System.Collections.Generic;

using Invector;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vItemManager;
using Invector.vShooter;

using Sirenix.OdinInspector.Editor;
using UnityEngine;

#if FOOTIK
using HoaxGames;
#endif

namespace DaftAppleGames.Editor.AutoEditor.Characters
{
    /// <summary>
    /// Editor tool to configure a GameObject as an Invector Character
    /// </summary>
    public class BaseCharacterEditor : OdinEditorWindow
    {
        [Header("Character Settings")]
        public BaseCharacterSettings characterSettings;
        public GameObject playerGameObject;
        
        /// <summary>
        /// Base configure method
        /// </summary>
        public virtual void ConfigureCharacter()
        {
            if (characterSettings == null || playerGameObject == null)
            {
                Debug.Log("Error: No settings found!!!");
                return;
            }
        }

        /// <summary>
        /// Set up the gameobject layer and tag
        /// </summary>
        public void SetUpTagAndLayer()
        {
            playerGameObject.layer = characterSettings.layer;
            playerGameObject.tag = characterSettings.tag;
        }
        
        /// <summary>
        /// Set up swimming specific settings
        /// </summary>
        public void SetUpSwimming()
        {
            // Set up swimming properties
            LogStartComponent("vSwimming");
            vSwimming swimming = playerGameObject.GetComponent<vSwimming>();
            if (!swimming)
            {
                swimming = playerGameObject.AddComponent<vSwimming>();
                LogAddComponent("vSwimming");
            }

            swimming.swimForwardSpeed = characterSettings.swimForwardSpeed;
            swimming.swimUpSpeed = characterSettings.swimUpSpeed;
            swimming.swimDownSpeed = characterSettings.swimDownSpeed;
            swimming.swimRotationSpeed = characterSettings.swimRotationSpeed;

            swimming.swimUpDownSmooth = characterSettings.swimUpAndDownSmooth;
            swimming.stamina = characterSettings.swimStamina;
            swimming.healthConsumption = (int)characterSettings.swimHealthConsumption;
            LogDoneComponent("vSwimming");

        }

        /// <summary>
        /// Set up combat settings
        /// </summary>
        public void SetUpShooterAndMelee()
        {
            // Set up Melle and Shooter config
            LogStartComponent("vShooterManager");
            vShooterManager shooterManager = playerGameObject.GetComponent<vShooterManager>();
            if (!shooterManager)
            {
                shooterManager = playerGameObject.AddComponent<vShooterManager>();
                LogAddComponent("vShooterManager");
            }

            shooterManager.damageLayer = characterSettings.damageLayer;
            shooterManager.ignoreTags = characterSettings.ignoreTags;
            shooterManager.blockAimLayer = characterSettings.blockAimLayer;
            shooterManager.AllAmmoInfinity = characterSettings.infiniteAmmo;
            shooterManager.useAmmoDisplay = characterSettings.useAmmoDisplay;
            LogDoneComponent("vShooterManager");

            vAmmoManager ammoManager = playerGameObject.GetComponent<vAmmoManager>();
            if (!ammoManager)
            {
                ammoManager = playerGameObject.AddComponent<vAmmoManager>();
                LogAddComponent("vAmmoManager");
            }

            ammoManager.ammoListData = characterSettings.ammoListData;
        }

        /// <summary>
        /// Set up Head Tracking
        /// </summary>
        public void SetUpHeadTrack()
        {
            // Set up headtrack
            LogStartComponent("vHeadTrack");
            vHeadTrack headTrack = playerGameObject.GetComponent<vHeadTrack>();
            if (!headTrack)
            {
                headTrack = playerGameObject.AddComponent<vHeadTrack>();
                LogAddComponent("vHeadTrack");
            }

            headTrack.obstacleLayer = characterSettings.headTrackObstacleLayer;
            headTrack.distanceToDetect = characterSettings.headTrackDistanceToDetect;
            LogDoneComponent("vHeadTrack");
        }

        /// <summary>
        /// Set up Inventory
        /// </summary>
        public void SetUpInventory()
        {
            // Set up Inventory
            LogStartComponent("vItemManager");
            vItemManager itemManager = playerGameObject.GetComponent<vItemManager>();
            if (!itemManager)
            {
                itemManager = playerGameObject.AddComponent<vItemManager>();
                LogAddComponent("vItemManager");
            }

            itemManager.itemListData = characterSettings.inventoryItemListData;
            LogDoneComponent("vItemManager");

        }

        /// <summary>
        /// Set up Audio settings
        /// </summary>
        public void SetUpAudio()
        {
            // Set up Audio
            LogStartComponent("AudioSource");
            AudioSource audioSource = playerGameObject.GetComponent<AudioSource>();
            if (!audioSource)
            {
                audioSource = playerGameObject.AddComponent<AudioSource>();
                LogAddComponent("AudioSource");
            }

            audioSource.outputAudioMixerGroup = characterSettings.audioMixerGroup;
            
            LogDoneComponent("AudioSource");
        }

        /// <summary>
        /// Set up damage and injury settings
        /// </summary>
        public void SetUpDamage()
        {
            // Set up damage effects
            LogStartComponent("vHitDamageParticles");
            vHitDamageParticle hitDamage = playerGameObject.GetComponent<vHitDamageParticle>();
            if (!hitDamage)
            {
                hitDamage = playerGameObject.AddComponent<vHitDamageParticle>();
                LogAddComponent("vHitDamageParticle");
            }

            hitDamage.defaultDamageEffects.Clear();
            hitDamage.defaultDamageEffects = new List<GameObject>(characterSettings.defaultDamageEffects);
            LogDoneComponent("vHitDamageParticles");
        }

        /// <summary>
        /// Set up the vFootStep system
        /// </summary>
        public void SetUpFootSteps()
        {
            // Set up footsteps
            LogStartComponent("vFootStep");
            vFootStep footStep = playerGameObject.GetComponent<vFootStep>();
            if (!footStep)
            {
                footStep = playerGameObject.AddComponent<vFootStep>();
                LogAddComponent("vFootStep");
            }

            footStep.defaultSurface = characterSettings.defaultAudioSurface;
            footStep.customSurfaces = new List<vAudioSurface>(characterSettings.customAudioSurfaces);
            footStep.enabled = characterSettings.useFootsteps;
            LogDoneComponent("vFootStep");
        }

        /// <summary>
        /// Set up the FootIK system
        /// </summary>
        public void SetUpFootIk()
        {
#if FOOTIK
            // Set up Footstep
            LogStartComponent("FootIK");
            FootIK footIK = playerGameObject.GetComponent<FootIK>();
            if (!footIK)
            {
                footIK = playerGameObject.AddComponent<FootIK>();
                LogAddComponent("FootIK");
            }

            float test;
            string test2;

            test2 = characterSettings.forceActivateCrouchState;
            test = characterSettings.crouchCorrectionTollerance;
            test2 = characterSettings.invalidAnimationState1;
            test2 = characterSettings.invalidAnimationState2;
            test = characterSettings.ikMaxCorrection;
            test = characterSettings.increaseMaxCorrectionDistance;
            test = characterSettings.checkGroundRadius;
#endif
        }

        /// <summary>
        /// Private logger - Start Component config
        /// </summary>
        /// <param name="componentName"></param>
        public void LogStartComponent(string componentName)
        {
            Debug.Log($"Configuring: {componentName}...");
        }

        /// <summary>
        /// Private logger - Done Component config
        /// </summary>
        /// <param name="componentName"></param>
        public void LogDoneComponent(string componentName)
        {
            Debug.Log($"Done configuring: {componentName}.");
        }

        /// <summary>
        /// Private logger - Add Component config
        /// </summary>
        /// <param name="componentName"></param>
        public void LogAddComponent(string componentName)
        {
            Debug.Log($"Adding missing component: {componentName}.");
        }

        /// <summary>
        /// Find the "Head" transform
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public Transform FindEyes(GameObject gameObject)
        {
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.ToLower().Contains("eye"))
                {
                    return child;
                }
            }
            return null;
        }
    }
}
#endif
