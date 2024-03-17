#if ASMDEF
#if EMERALD_AI_PRESENT
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using EmeraldAI;
using System.Collections.Generic;

namespace DaftAppleGames.Editor.AutoEditor.AI
{
    public class EmeraldAIAutoEditor : OdinEditorWindow
    {
        [MenuItem("Window/Characters/Emerald AI Auto Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(EmeraldAIAutoEditor));
        }

        [Header("Configuration")]
        [Tooltip("Choose a root GameObject from the hierarchy. If left blank, the whole scene will be used.")]
        public GameObject rootGameObject;
        public List<GameObject> hitEffects;
        
        [Header("Detection")]
        public LayerMask obstructionIgnoreLayers;
        public EmeraldAISystem.YesOrNo objectAvoidance = EmeraldAISystem.YesOrNo.Yes;
        public int detectionDistance = 18;
        public int chaseDistance = 30;
        public int extendedChaseDistance = 80;
        public EmeraldAISystem.YesOrNo aiAvoidance = EmeraldAISystem.YesOrNo.Yes;
        public LayerMask aiAvoidanceLayer;
        
        [Header("Tags")]
        public string emeraldTag = "EmeraldAI";
        public LayerMask detectionLayers;
        public string followerTag = "Untagged";
        public string faction = "Creature";
        public bool isEnemy = true;
        
        [Header("Stats")]
        public string aiName;
        public string aiTitle;
        public int aiHealth = 50;
        public int regenRate = 1;
        public int regenAmount = 1;

        [Header("Movement")]
        public int stoppingDistance = 2;
        public int followingStoppingDistance = 5;
        public int turningAngle = 20;
        public int combatTurningAngle = 25;
        public int stationaryTurnSpeed = 30;
        public float movementTurnSpeed = 2.5f;
        public int stationaryCombatTurnSpeed = 30;
        public float movingCombatTurnSpeed = 2;
        public EmeraldAISystem.YesOrNo randomRotateOnStart = EmeraldAISystem.YesOrNo.Yes;
        public EmeraldAISystem.YesOrNo alignAi = EmeraldAISystem.YesOrNo.Yes;

        [Header("Combat")]
        public float meleeAttackDistance = 2.5f;
        
        /// <summary>
        /// Configure button
        /// </summary>
        [Button("Configure")]
        [Tooltip("Run the editor configuration process.")]
        private void ConfigureClick()
        {
            if (string.IsNullOrEmpty(editorSettingsName))
            {
                Debug.LogError("Please load a config file!");
                return;
            }
            Debug.Log($"Processing: {rootGameObject.name}");
            ApplySettings(rootGameObject);
        }

        [Header("Settings")]
        [PropertyOrder(1)]
        public EmeraldAIAutoEditorSettings autoEditorSettings;
        [PropertyOrder(1)]
        public string editorSettingsName;

        /// <summary>
        /// Load Config button
        /// </summary>
        [Button("Load Settings")]
        [PropertyOrder(1)]
        private void LoadSettingsClick()
        {
            LoadSettings();
        }

        /// <summary>
        /// Load the default settings. Should be overridden in the parent class.
        /// </summary>
        public virtual void LoadSettings()
        {
            // Update config settings
            editorSettingsName = autoEditorSettings.aiName;

            // Update object identifiers
            hitEffects = autoEditorSettings.hitEffects;
            obstructionIgnoreLayers = autoEditorSettings.obstructionIgnoreLayers;
            objectAvoidance = autoEditorSettings.objectAvoidance;
            detectionDistance = autoEditorSettings.detectionDistance;
            chaseDistance = autoEditorSettings.chaseDistance;
            extendedChaseDistance = autoEditorSettings.extendedChaseDistance;
            emeraldTag = autoEditorSettings.emeraldTag;
            detectionLayers = autoEditorSettings.detectionLayers;
            followerTag = autoEditorSettings.followerTag;
            faction = autoEditorSettings.faction;
            aiName = autoEditorSettings.aiName;
            aiTitle = autoEditorSettings.aiTitle;
            aiHealth = autoEditorSettings.aiHealth;
            regenRate = autoEditorSettings.regenRate;
            regenAmount = autoEditorSettings.regenAmount;
            stoppingDistance = autoEditorSettings.stoppingDistance;
            followingStoppingDistance = autoEditorSettings.followingStoppingDistance;
            turningAngle = autoEditorSettings.turningAngle;
            combatTurningAngle = autoEditorSettings.combatTurningAngle;
            stationaryTurnSpeed = autoEditorSettings.stationaryTurnSpeed;
            movementTurnSpeed = autoEditorSettings.movementTurnSpeed;
            stationaryCombatTurnSpeed = autoEditorSettings.stationaryCombatTurnSpeed;
            movingCombatTurnSpeed = autoEditorSettings.movingCombatTurnSpeed;
            randomRotateOnStart = autoEditorSettings.randomRotateOnStart;
            alignAi = autoEditorSettings.alignAi;
            meleeAttackDistance = autoEditorSettings.meleeAttackDistance;
            aiAvoidance = autoEditorSettings.aiAvoidance;
            aiAvoidanceLayer = autoEditorSettings.aiAvoidanceLayer;
            isEnemy = autoEditorSettings.isEnemy;
        }

        /// <summary>
        /// Add missing colliders
        /// </summary>
        private void ApplySettings(GameObject gameObject)
        {
            EmeraldAISystem emeraldAISystem = gameObject.GetComponent<EmeraldAISystem>();

            Debug.Log("Setting Up Hit Effects...");
            emeraldAISystem.UseHitEffect = EmeraldAISystem.YesOrNo.Yes;
            emeraldAISystem.BloodEffectsList = hitEffects;
            Debug.Log("Done!");

            Debug.Log("Setting Up Head...");
            emeraldAISystem.HeadTransform = FindHead(gameObject);
            Debug.Log("Done!");
            
            Debug.Log("Setting Up Detection...");
            emeraldAISystem.ObstructionDetectionLayerMask = obstructionIgnoreLayers;
            emeraldAISystem.DetectionRadius = detectionDistance;
            emeraldAISystem.MaxChaseDistance = chaseDistance;
            emeraldAISystem.ExpandedChaseDistance = extendedChaseDistance;
            emeraldAISystem.UseAIAvoidance = aiAvoidance;
            emeraldAISystem.AIAvoidanceLayerMask = aiAvoidanceLayer;
            
            Debug.Log("Done!");

            Debug.Log("Setting Up Tags ...");
            emeraldAISystem.EmeraldTag = emeraldTag;
            emeraldAISystem.DetectionLayerMask = detectionLayers;
            emeraldAISystem.FollowTag = followerTag;
            Debug.Log("Done!");
            
            Debug.Log("Setting Up Movement...");
            emeraldAISystem.StoppingDistance = stoppingDistance;
            emeraldAISystem.FollowingStoppingDistance = followingStoppingDistance;
            emeraldAISystem.AngleToTurn = turningAngle;
            emeraldAISystem.CombatAngleToTurn = combatTurningAngle;
            emeraldAISystem.StationaryTurningSpeedNonCombat = stationaryTurnSpeed;
            emeraldAISystem.MovingTurnSpeedNonCombat = movementTurnSpeed;
            emeraldAISystem.StationaryTurningSpeedCombat = stationaryCombatTurnSpeed;
            emeraldAISystem.MovingTurnSpeedCombat = movingCombatTurnSpeed;
            emeraldAISystem.UseRandomRotationOnStartRef = randomRotateOnStart;
            emeraldAISystem.AlignAIWithGroundRef = alignAi;            
            Debug.Log("Done!");
            
            Debug.Log("Setting Up Stats...");
            emeraldAISystem.AIName = aiName;
            emeraldAISystem.AITitle = aiTitle;
            emeraldAISystem.StartingHealth = aiHealth;
            emeraldAISystem.RegenerateTimer = regenRate;
            emeraldAISystem.RegenerateAmount = regenAmount;
            Debug.Log("Done!");
            
            Debug.Log("Setting Up Combat...");
            emeraldAISystem.MeleeAttackDistance = meleeAttackDistance;
            Debug.Log("Done!");
            
            Debug.Log("Setting Up Factions...");
            if (isEnemy)
            {
                emeraldAISystem.CurrentFaction = 1;
                SetFactionStateEnemy(emeraldAISystem);
            }
            else
            {
                emeraldAISystem.CurrentFaction = 3;
                SetFactionStateFriendly(emeraldAISystem);
            }
            Debug.Log("Done!");
            
            Debug.Log($"AI processed: {gameObject.name}");
        }

        /// <summary>
        /// Find the "Head" transform
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private Transform FindHead(GameObject gameObject)
        {
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                if (child.name.ToLower().Contains("head"))
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        /// Set up Animal and Creature factions
        /// </summary>
        /// <param name="emeraldAISystem"></param>
        private void SetFactionStateEnemy(EmeraldAISystem emeraldAISystem)
        {
            // Clear Faction List
            emeraldAISystem.FactionRelationsList.Clear();
            
            // Add "Animal as Enemy"
            EmeraldAISystem.FactionsList animalFaction = new EmeraldAISystem.FactionsList(3, 0);
            emeraldAISystem.FactionRelationsList.Add(animalFaction);
            
            // Add "Creature as Neutral"
            EmeraldAISystem.FactionsList creatureFaction = new EmeraldAISystem.FactionsList(1, 1);
            emeraldAISystem.FactionRelationsList.Add(creatureFaction);
        }
        
        /// <summary>
        /// Set up Animal and Creature factions
        /// </summary>
        /// <param name="emeraldAISystem"></param>
        private void SetFactionStateFriendly(EmeraldAISystem emeraldAISystem)
        {
            // Clear Faction List
            emeraldAISystem.FactionRelationsList.Clear();
            
            // Add "Animal as Friend"
            EmeraldAISystem.FactionsList animalFaction = new EmeraldAISystem.FactionsList(3, 1);
            emeraldAISystem.FactionRelationsList.Add(animalFaction);
            
            // Add "Creature as Enemy"
            EmeraldAISystem.FactionsList creatureFaction = new EmeraldAISystem.FactionsList(1, 0);
            emeraldAISystem.FactionRelationsList.Add(creatureFaction);
        }
        
    }
}
#endif
#endif