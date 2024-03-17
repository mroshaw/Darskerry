#if ASMDEF
#if INVECTOR_SHOOTER
using DaftAppleGames.Common.Characters;
using Invector.vCharacterController.AI;
using Sirenix.OdinInspector;
using UnityEditor;

namespace DaftAppleGames.Editor.AutoEditor.Characters
{
    /// <summary>
    /// Editor tool to configure a GameObject as an NPC Character
    /// </summary>
    public class NpcCharacterEditor : BaseCharacterEditor
    {
        private NpcCharacterSettings _npcCharacterSettings;

        protected override void OnEnable()
        {
            base.OnEnable();
        }
        
        [MenuItem("Window/Characters/NPC Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(NpcCharacterEditor));
        }

        /// <summary>
        /// Configure the NPC character
        /// </summary>
        [Button("Configure NPC")]
        public override void ConfigureCharacter()
        {
            _npcCharacterSettings = characterSettings as NpcCharacterSettings;
            
            SetUpTagAndLayer();
            SetUpMovement();
            SetUpAudio();
            SetUpDamage();
            SetUpFootSteps();
            SetUpDetection();
        }

        public void SetUpNpcCharacter()
        {
            // Add "Player Character" component
            LogStartComponent("NpcCharacter");
            NpcCharacter npcCharacter = playerGameObject.GetComponent<NpcCharacter>();
            if (!npcCharacter)
            {
                npcCharacter = playerGameObject.AddComponent<NpcCharacter>();
                LogAddComponent("NpcCharacter");
            }

            npcCharacter.deathAudioClips = _npcCharacterSettings.deathAudioClips;
            npcCharacter.hitAudioClips = _npcCharacterSettings.hitAudioClips;
            
            LogDoneComponent("NpcCharacter");

            // Set up the character as required.
            npcCharacter.characterName = characterSettings.characterName;
        }
        
        /// <summary>
        /// Set up NPC AI detection settings
        /// </summary>
        public void SetUpDetection()
        {
            vControlAI controller = playerGameObject.GetComponent<vControlAI>();
            if (!controller)
            {
                controller = playerGameObject.AddComponent<vControlAI>();
                LogAddComponent("vControlAI");
            }

            controller.SetDetectionLayer(_npcCharacterSettings.detectLayer);
            controller.SetDetectionTags(_npcCharacterSettings.detectTags);
            controller.SetObstaclesLayer(_npcCharacterSettings.obstaclesLayer);
            controller.detectionPointReference = FindEyes(playerGameObject);
        }
        
        /// <summary>
        /// Set up NPC AI movement settings
        /// </summary>
        public void SetUpMovement()
        {
            // Set up movement properties
            LogStartComponent("vControlAI");
            vControlAI controller = playerGameObject.GetComponent<vControlAI>();
            if (!controller)
            {
                controller = playerGameObject.AddComponent<vControlAI>();
                LogAddComponent("vControlAI");
            }

            controller.freeSpeed.walkSpeed = _npcCharacterSettings.walkSpeed;
            controller.freeSpeed.runningSpeed = _npcCharacterSettings.runSpeed;
            controller.freeSpeed.sprintSpeed = _npcCharacterSettings.sprintSpeed;
            controller.freeSpeed.crouchSpeed = _npcCharacterSettings.crouchSpeed;

            controller.freeSpeed.rotationSpeed = _npcCharacterSettings.rotationSpeed;

            controller.strafeSpeed.walkSpeed = _npcCharacterSettings.strafeWalkSpeed;
            controller.strafeSpeed.runningSpeed = _npcCharacterSettings.strafeRunSpeed;
            controller.strafeSpeed.sprintSpeed = _npcCharacterSettings.strafeSprintSpeed;
            controller.strafeSpeed.crouchSpeed = _npcCharacterSettings.strafeCrouchSpeed;

            controller.useRootMotion = _npcCharacterSettings.useRootMotion;
            
            // Set up stamina and health
            controller.maxHealth = (int)_npcCharacterSettings.maxHealth;
            controller.currentHealth = _npcCharacterSettings.startHealth;
            controller.healthRecovery = _npcCharacterSettings.healthRecovery;
            controller.healthRecoveryDelay = _npcCharacterSettings.healthRecoveryDelay;

            // Set up grounded
            controller.groundLayer = _npcCharacterSettings.groundedLayer;

            LogDoneComponent("vControlAI");
        }
    }
}
#endif
#endif