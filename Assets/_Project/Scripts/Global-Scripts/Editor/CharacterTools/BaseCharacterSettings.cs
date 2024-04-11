#if INVECTOR_SHOOTER
using Invector;
using Invector.vItemManager;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Editor.CharacterTools
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Invcetor Character Configuration
    /// </summary>
    public class BaseCharacterSettings : ScriptableObject
    {
        [BoxGroup("Tag and Layer")] public LayerMask layer;
        [BoxGroup("Tag and Layer")]public string tag;
        
        [BoxGroup("Normal Movement")] public string characterName;
        [BoxGroup("Normal Movement")] public float walkSpeed;
        [BoxGroup("Normal Movement")] public float runSpeed;
        [BoxGroup("Normal Movement")] public float sprintSpeed;
        [BoxGroup("Normal Movement")] public float crouchSpeed;
        [BoxGroup("Normal Movement")] public float animSmooth;
        [BoxGroup("Normal Movement")] public float rotationSpeed;
        [BoxGroup("Normal Movement")] public float movementSmooth;
        [BoxGroup("Normal Movement")] public bool useLeanAnim = false;

        [BoxGroup("Strafe Movement")] public float strafeWalkSpeed;
        [BoxGroup("Strafe Movement")] public float strafeRunSpeed;
        [BoxGroup("Strafe Movement")] public float strafeSprintSpeed;
        [BoxGroup("Strafe Movement")] public float strafeCrouchSpeed;
        [BoxGroup("Strafe Movement")] public float leanSmooth;
        [BoxGroup("Strafe Movement")] public bool useRootMotion;
        
        [BoxGroup("Stamina and Health")] public int maxHealth;
        [BoxGroup("Stamina and Health")] public float startHealth;
        [BoxGroup("Stamina and Health")] public int maxStamina;
        [BoxGroup("Stamina and Health")] public float healthRecovery;
        [BoxGroup("Stamina and Health")] public float healthRecoveryDelay;
        
        [BoxGroup("Ground Detection")] public LayerMask groundedLayer;
        [BoxGroup("Ground Detection")] public bool useSnapGround;
        [BoxGroup("Ground Detection")] public LayerMask stopMoveLayer;
        [BoxGroup("Ground Detection")] public bool useSlopeLimit;
        [BoxGroup("Ground Detection")] public float slopeLimit = 45.0f;
        [BoxGroup("Ground Detection")] public LayerMask stepOffsetLayer;
        [BoxGroup("Ground Detection")] public float stepOffsetMaxHeight = 0.5f;
        [BoxGroup("Ground Detection")] public float stepOffsetMinHeight = 0.0f;
        [BoxGroup("Ground Detection")] public float stepOffsetDistance = 0.1f;

        [BoxGroup("Swimming")] public float swimForwardSpeed;
        [BoxGroup("Swimming")] public float swimUpSpeed;
        [BoxGroup("Swimming")] public float swimDownSpeed;
        [BoxGroup("Swimming")] public float swimRotationSpeed;
        [BoxGroup("Swimming")] public float swimUpAndDownSmooth;
        [BoxGroup("Swimming")] public float swimStamina;
        [BoxGroup("Swimming")] public float swimHealthConsumption;

        [BoxGroup("Melee and Shooter")] public LayerMask damageLayer;
        [BoxGroup("Melee and Shooter")] public string[] ignoreTags;
        [BoxGroup("Melee and Shooter")] public LayerMask blockAimLayer;
        [BoxGroup("Melee and Shooter")] public bool infiniteAmmo;
        [BoxGroup("Melee and Shooter")] public bool useAmmoDisplay;
        [BoxGroup("Melee and Shooter")] public vAmmoListData ammoListData;
        [BoxGroup("Melee and Shooter")] public vItemListData inventoryItemListData;
        [BoxGroup("Melee and Shooter")] public List<GameObject> defaultDamageEffects;

        [BoxGroup("Character Setup")] public LayerMask headTrackObstacleLayer;
        [BoxGroup("Character Setup")] public float headTrackDistanceToDetect;
        [BoxGroup("Character Setup")] public vAudioSurface defaultAudioSurface;
        [BoxGroup("Character Setup")] public List<vAudioSurface> customAudioSurfaces;
        [BoxGroup("Character Setup")] public bool useFootsteps;

        [BoxGroup("Audio")] public AudioMixerGroup audioMixerGroup;
        [BoxGroup("Audio")] public AudioClip[] hitAudioClips;
        [BoxGroup("Audio")] public AudioClip[] deathAudioClips;
        
        [BoxGroup ("iStep Settings")] public string forceActivateCrouchState = "Free Crouch";
        [BoxGroup ("iStep Settings")] public float crouchCorrectionTollerance = 0.3f;
        [BoxGroup ("iStep Settings")] public string invalidAnimationState1 = "JumpOver";
        [BoxGroup ("iStep Settings")] public string invalidAnimationState2 = "StepUp";
        [BoxGroup ("iStep Settings")] public float ikMaxCorrection = 0.7f;
        [BoxGroup ("iStep Settings")] public float increaseMaxCorrectionDistance = 0.25f;
        [BoxGroup ("iStep Settings")] public float checkGroundRadius = 0.35f;
    }
}
#endif