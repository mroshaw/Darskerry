#if ASMDEF
using System.Collections.Generic;
using EmeraldAI;
using UnityEngine;
using UnityEngine.Serialization;

namespace DaftAppleGames.Editor.AutoEditor.AI
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "EmeraldAIAutoEditorSettings", menuName = "Settings/AI/EmeraldAIAutoEditor", order = 1)]
    public class EmeraldAIAutoEditorSettings : ScriptableObject
    {
        [Header("Basic Settings")]
        public string settingsName;

        [Header("Damage FX")]
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
    }
    }
#endif