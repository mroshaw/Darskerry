#if INVECTOR_SHOOTER
using Invector;
using Invector.vItemManager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Editor.AutoEditor.Characters
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Invetor Character Configuration
    /// </summary>
    public class BaseCharacterSettings : ScriptableObject
    {
        [Header("Tag and Layer")]
        public LayerMask layer;
        public string tag;
        
        [Header("Normal Movement")]
        public string characterName;
        public float walkSpeed;
        public float runSpeed;
        public float sprintSpeed;
        public float crouchSpeed;
        public float animSmooth;
        public float rotationSpeed;
        public float movementSmooth;
        public bool useLeanAnim = false;

        [Header("Strafe Movement")]
        public float strafeWalkSpeed;
        public float strafeRunSpeed;
        public float strafeSprintSpeed;
        public float strafeCrouchSpeed;

        public float leanSmooth;

        public bool useRootMotion;
        
        [Header("Stamina and Health")]
        public int maxHealth;
        public float startHealth;
        public int maxStamina;
        public float healthRecovery;
        public float healthRecoveryDelay;
        
        [Header("Ground Detection")]
        public LayerMask groundedLayer;
        public bool useSnapGround;
        public LayerMask stopMoveLayer;
        public bool useSlopeLimit;
        public float slopeLimit = 45.0f;
        public LayerMask stepOffsetLayer;
        public float stepOffsetMaxHeight = 0.5f;
        public float stepOffsetMinHeight = 0.0f;
        public float stepOffsetDistance = 0.1f;

        [Header("Swimming")]
        public float swimForwardSpeed;
        public float swimUpSpeed;
        public float swimDownSpeed;
        public float swimRotationSpeed;
        public float swimUpAndDownSmooth;
        public float swimStamina;
        public float swimHealthConsumption;

        [Header("Melee and Shooter")]
        public LayerMask damageLayer;
        public string[] ignoreTags;
        public LayerMask blockAimLayer;
        public bool infiniteAmmo;
        public bool useAmmoDisplay;
        public vAmmoListData ammoListData;
        public vItemListData inventoryItemListData;
        public List<GameObject> defaultDamageEffects;

        [Header("Character Setup")]
        public LayerMask headTrackObstacleLayer;
        public float headTrackDistanceToDetect;
        public vAudioSurface defaultAudioSurface;
        public List<vAudioSurface> customAudioSurfaces;
        public bool useFootsteps;

        [Header("Audio")]
        public AudioMixerGroup audioMixerGroup;
        public AudioClip[] hitAudioClips;
        public AudioClip[] deathAudioClips;
        
        [Header("iStep Settings")]
        public string forceActivateCrouchState = "Free Crouch";
        public float crouchCorrectionTollerance = 0.3f;
        public string invalidAnimationState1 = "JumpOver";
        public string invalidAnimationState2 = "StepUp";
        public float ikMaxCorrection = 0.7f;
        public float increaseMaxCorrectionDistance = 0.25f;
        public float checkGroundRadius = 0.35f;

    }
}
#endif
