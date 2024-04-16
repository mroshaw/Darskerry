#if INVECTOR_SHOOTER
using DaftAppleGames.Common.Characters;
using Invector.vCharacterController;
using Sirenix.OdinInspector;
using UnityEditor;

namespace DaftAppleGames.Editor.CharacterTools
{
    /// <summary>
    /// Editor tool to configure a GameObject as a Player Character
    /// </summary>
    public class PlayerCharacterEditorWindow : BaseCharacterEditorWindow
    {
        private PlayerCharacterSettings _playerCharacterSettings;

        protected override void OnEnable()
        {
            base.OnEnable();
            _playerCharacterSettings = characterSettings as PlayerCharacterSettings;
        }
        
        [MenuItem("Daft Apple Games/Tools/Characters/Player character editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(PlayerCharacterEditorWindow));
        }

        
        /// <summary>
        /// Configure the NPC character
        /// </summary>
        [Button("Configure Player")]
        public override void ConfigureCharacter()
        {
            SetUpTagAndLayer();
            SetUpPlayerCharacter();
            SetUpMovement();
            SetUpSwimming();
            SetUpShooterAndMelee();
            SetUpHeadTrack();
            SetUpInventory();
            SetUpAudio();
            SetUpDamage();
            SetUpFootSteps();
            SetUpFootIk();
        }

        /// <summary>
        /// Setup the Player Character settings
        /// </summary>
        public void SetUpPlayerCharacter()
        {
            // Add "Player Character" component
            LogStartComponent("PlayerCharacter");
            PlayerCharacter playerCharacter = playerGameObject.GetComponent<PlayerCharacter>();
            if (!playerCharacter)
            {
                playerCharacter = playerGameObject.AddComponent<PlayerCharacter>();
                LogAddComponent("PlayerCharacter");
            }

            playerCharacter.deathAudioClips = _playerCharacterSettings.deathAudioClips;
            playerCharacter.hitAudioClips = _playerCharacterSettings.hitAudioClips;
            
            LogDoneComponent("Player Character");

            // Set up the character as required.
            playerCharacter.characterName = characterSettings.characterName;
        }
        
                public void SetUpMovement()
        {
            // Set up movement properties
            LogStartComponent("vThirdPersonController");
            vThirdPersonController controller = playerGameObject.GetComponent<vThirdPersonController>();
            if (!controller)
            {
                controller = playerGameObject.GetComponent<vThirdPersonController>();
                LogAddComponent("vThirdPersonController");
            }

            controller.freeSpeed.walkSpeed = _playerCharacterSettings.walkSpeed;
            controller.freeSpeed.runningSpeed = _playerCharacterSettings.runSpeed;
            controller.freeSpeed.sprintSpeed = _playerCharacterSettings.sprintSpeed;
            controller.freeSpeed.crouchSpeed = _playerCharacterSettings.crouchSpeed;

            controller.freeSpeed.rotationSpeed = _playerCharacterSettings.rotationSpeed;
            controller.freeSpeed.movementSmooth = _playerCharacterSettings.movementSmooth;

            controller.strafeSpeed.walkSpeed = _playerCharacterSettings.strafeWalkSpeed;
            controller.strafeSpeed.runningSpeed = _playerCharacterSettings.strafeRunSpeed;
            controller.strafeSpeed.sprintSpeed = _playerCharacterSettings.strafeSprintSpeed;
            controller.strafeSpeed.crouchSpeed = _playerCharacterSettings.strafeCrouchSpeed;

            controller.useLeanMovementAnim = _playerCharacterSettings.useLeanAnim;
            controller.leanSmooth = _playerCharacterSettings.leanSmooth;

            controller.useRootMotion = _playerCharacterSettings.useRootMotion;
            
            // Set up stamina and health
            controller.maxHealth = (int)_playerCharacterSettings.maxHealth;
            // controller.currentHealth = _playerCharacterSettings.startHealth;
            controller.maxStamina = (int)_playerCharacterSettings.maxStamina;
            controller.healthRecovery = _playerCharacterSettings.healthRecovery;
            controller.healthRecoveryDelay = _playerCharacterSettings.healthRecoveryDelay;
            controller.staminaRecovery = _playerCharacterSettings.staminaRecovery;
            controller.jumpStamina = _playerCharacterSettings.jumpStamina;
            controller.rollStamina = _playerCharacterSettings.rollStamina;

            /*

            // Set up grounded
            controller.groundLayer = _playerCharacterSettings.groundedLayer;
            controller.useSnapGround = _playerCharacterSettings.useSnapGround;

            // Stop layer
            controller.stopMoveLayer = _playerCharacterSettings.stopMoveLayer;

            // Step
            controller.stepOffsetLayer = _playerCharacterSettings.stepOffsetLayer;
            controller.stepOffsetMaxHeight = _playerCharacterSettings.stepOffsetMaxHeight;
            controller.stepOffsetMinHeight = _playerCharacterSettings.stepOffsetMinHeight;
            controller.stepOffsetDistance = _playerCharacterSettings.stepOffsetDistance;
            
            // Slope
            controller.useSlopeLimit = _playerCharacterSettings.useSlopeLimit;
            controller.slopeLimit = 45.0f;
            */
            LogDoneComponent("vThirdPersonController");
        }
    }
}
#endif