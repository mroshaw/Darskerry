using DaftAppleGames.Common.Characters;
#if INVECTOR_SHOOTER
using Invector;
#endif
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DaftAppleGames.Editor.Characters
{
    public enum ColliderType
    {
        Unknown,
        Capsule,
        Box,
        Sphere
    }

    [CustomEditor(typeof(Character))]
    public class CharacterEditor : OdinEditor
    {
        // Public serializable properties
        [Header("Character Editor Settings")]
        public WaterSplashSettings waterSplashSettings;

        public Character character;

        private GameObject _targetGameObject;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            character = target as Character;
            _targetGameObject = character.gameObject;
            if (GUILayout.Button("Add / Update Water Splash"))
            {
                ConfigureWaterSplash();
            }
        }

        /// <summary>
        /// Configures the "WaterSplash" feature
        /// </summary>
        private void ConfigureWaterSplash()
        {
            waterSplashSettings =
                (WaterSplashSettings)EditorGUIUtility.Load(
                    "Assets/_Project/Settings/Characters/WaterSplashSettings.asset");
            if (!waterSplashSettings)
            {
                Debug.Log("Can't find WaterSplashSettings!");
                return;
            }

            ConfigureWaterTrigger();
            ConfigureFootSteps();
            UpdatePrefab();
        }

        /// <summary>
        /// Creates and / or configures the WaterTrigger game object
        /// </summary>
        private void ConfigureWaterTrigger()
        {
            // Configure the WaterTrigger component
            Collider mainCollider = _targetGameObject.GetComponent<Collider>();
            ColliderType colliderType = GetColliderType(mainCollider);
            WaterTrigger waterTrigger = _targetGameObject.GetComponentInChildren<WaterTrigger>();
            GameObject triggerGameObject;

            if (!waterTrigger)
            {
                Debug.Log("Adding WaterTrigger GameObject...");
                triggerGameObject = new GameObject(name = "Water Trigger");
                triggerGameObject.transform.SetParent(_targetGameObject.transform);
                triggerGameObject.tag = "WaterTrigger";
                triggerGameObject.layer = LayerMask.NameToLayer("Player");
                waterTrigger = triggerGameObject.AddComponent<WaterTrigger>();

            }
            else
            {
                triggerGameObject = waterTrigger.gameObject;
            }

            Debug.Log("Configuring Collider...");

            // Configure the collider
            switch (colliderType)
            {
                case ColliderType.Capsule:
                    CapsuleCollider capsuleMainCollider = mainCollider as CapsuleCollider;
                    CapsuleCollider capsuleTriggerCollider = waterTrigger.GetComponent<CapsuleCollider>();
                    if (!capsuleTriggerCollider)
                    {
                        capsuleTriggerCollider = triggerGameObject.AddComponent<CapsuleCollider>();
                    }

                    capsuleTriggerCollider.height = capsuleMainCollider.height;
                    capsuleTriggerCollider.radius = capsuleMainCollider.radius;
                    capsuleTriggerCollider.center = capsuleMainCollider.center;
                    capsuleTriggerCollider.direction = capsuleMainCollider.direction;
                    capsuleTriggerCollider.isTrigger = true;
                    break;

                case ColliderType.Box:
                    BoxCollider boxMainCollider = mainCollider as BoxCollider;
                    BoxCollider boxTriggerCollider = waterTrigger.GetComponent<BoxCollider>();
                    if (!boxTriggerCollider)
                    {
                        boxTriggerCollider = triggerGameObject.AddComponent<BoxCollider>();
                    }

                    boxTriggerCollider.size = boxMainCollider.size;
                    boxTriggerCollider.center = boxMainCollider.center;
                    boxTriggerCollider.isTrigger = true;
                    break;
            }
        }

        /// <summary>
        /// Configures the WaterSplash component on Footstep triggers
        /// </summary>
        private void ConfigureFootSteps()
        {
#if INVECTOR_SHOOTER
            // Find the vFootStep triggers
            vFootStepTrigger[] footstepTriggers = _targetGameObject.GetComponentsInChildren<vFootStepTrigger>(true);
            if (footstepTriggers.Length == 0)
            {
                Debug.Log("No vFootStep triggers found! Please create via the vFootStep editor!");
                return;
            }
            
            WaterSplash waterSplash;
            
            foreach (vFootStepTrigger stepTrigger in footstepTriggers)
            {
                // See if there's an existing WaterSplash on the foot
                waterSplash = stepTrigger.gameObject.transform.parent.GetComponentInChildren<WaterSplash>();
                if (waterSplash)
                {
                    ConfigureWaterSplash(waterSplash, stepTrigger);
                    Debug.Log($"Reconfigured existing WaterSplash on {stepTrigger.gameObject.name}");
                }
                else
                {
                    // Create the game object
                    Debug.Log($"Creating new GameObject...");
                    string vTriggerName = stepTrigger.gameObject.name;
                    GameObject newWaterSplashGameObject = new GameObject(name = $"watersplash_{vTriggerName}");
                    newWaterSplashGameObject.transform.SetParent(stepTrigger.gameObject.transform.parent);
                    newWaterSplashGameObject.transform.rotation = stepTrigger.gameObject.transform.rotation;
                    newWaterSplashGameObject.transform.position = stepTrigger.gameObject.transform.position;
                    newWaterSplashGameObject.transform.localScale = stepTrigger.gameObject.transform.localScale;
                    newWaterSplashGameObject.tag = "Foot";
                    newWaterSplashGameObject.layer = LayerMask.NameToLayer("Player");
                    
                    waterSplash = newWaterSplashGameObject.AddComponent<WaterSplash>();
                    ConfigureWaterSplash(waterSplash, stepTrigger);
                    Debug.Log($"Created new WaterSplash on {stepTrigger.gameObject.name}");
                }
            }
#endif
        }

#if INVECTOR_SHOOTER
        /// <summary>
        /// Configures the given WaterSplash component
        /// </summary>
        /// <param name="waterSplash"></param>
        /// <param name="stepTrigger"></param>
        private void ConfigureWaterSplash(WaterSplash waterSplash, vFootStepTrigger stepTrigger)
        {
            waterSplash.splashClips = waterSplashSettings.splashClips;
            waterSplash.splashFx = waterSplashSettings.splashFx;
            waterSplash.waterLayer = 1 << LayerMask.NameToLayer("Water");
            waterSplash.originTransform = stepTrigger.gameObject.transform.parent.parent.transform;
            
            AudioSource audioSource = waterSplash.GetComponent<AudioSource>();
            if (!audioSource)
            {
                Debug.Log("Adding AudioSource...");
                audioSource = waterSplash.gameObject.AddComponent<AudioSource>();
            }

            Debug.Log("Configuring AudioSource...");
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = waterSplashSettings.mixerGroup;
            audioSource.spatialBlend = 1;
            
            SphereCollider mainSphereCollider =
                waterSplash.gameObject.transform.parent.GetComponentInChildren<SphereCollider>();
            SphereCollider triggerSphereCollider = waterSplash.GetComponent<SphereCollider>();
            if (!triggerSphereCollider)
            {
                Debug.Log("Adding SphereCollider...");
                triggerSphereCollider = waterSplash.gameObject.AddComponent<SphereCollider>();
            }

            Debug.Log("Configuring SphereCollider...");
            triggerSphereCollider.center = mainSphereCollider.center;
            triggerSphereCollider.radius = mainSphereCollider.radius;
            triggerSphereCollider.isTrigger = true;
            
            Debug.Log("Adding Event Listener...");
            // UnityEditor.Events.UnityEventTools.RemovePersistentListener<Collider>(stepTrigger.OnStepCollider, waterSplash.CheckWaterSplash);
            // UnityEditor.Events.UnityEventTools.AddPersistentListener(stepTrigger.OnStepCollider, waterSplash.CheckWaterSplash);
        }
#endif

        /// <summary>
        /// Determine the type of collider
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        private ColliderType GetColliderType(Collider collider)
        {
            if (collider.GetType() == typeof(CapsuleCollider))
            {
                return ColliderType.Capsule;
            }

            if (collider.GetType() == typeof(BoxCollider))
            {
                return ColliderType.Box;
            }

            if (collider.GetType() == typeof(SphereCollider))
            {
                return ColliderType.Sphere;
            }
            return ColliderType.Unknown;
        }
        
        /// <summary>
        /// Marks the prefab as dirty, so it can be saved.
        /// </summary>
        public static void UpdatePrefab()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }
    }
}