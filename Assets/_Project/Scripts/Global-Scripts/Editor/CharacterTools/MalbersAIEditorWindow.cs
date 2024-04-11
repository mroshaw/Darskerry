using System;
using MalbersAnimations.Controller;
using MalbersAnimations.Controller.AI;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;
using UnityEditor.Animations;
using UnityEditor;
using DaftAppleGames.Common.Utils;
using MalbersAnimations.Utilities;
using MalbersAnimations;
using Sirenix.OdinInspector.Editor;

namespace DaftAppleGames.Editor.CharacterTools
{
    public class MalbersAIEditorWindow : OdinEditorWindow
    {
        [BoxGroup("Animal Settings")] public ConfigureMalbersAISettings malbersSettings;

        [SerializeField] [BoxGroup("Animal Object")] private GameObject _animalGameObject;
        [BoxGroup("Animal Object")] public Transform skeleton;

        [MenuItem("Daft Apple Games/Characters/Malbers AI editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MalbersAIEditorWindow));
        }

        private MAnimalAIControl _aiAnimalControl;
        private MAnimalBrain _aiAnimalBrain;
        private MAnimal _animal;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private AnimatorController _animatorController;
        private MalbersAnimations.Utilities.LookAt _lookAt;
        private Aim _aim;
        private Transform _rootBone;
        private Transform _headBone;
        private Transform _neckBone;

        /// <summary>
        /// Update selected Game Object
        /// </summary>
        private void OnSelectionChange()
        {
            if (Selection.gameObjects.Length > 0)
            {
                _animalGameObject = Selection.gameObjects[0];
            }
        }

        /// <summary>
        /// Check the editor settings
        /// </summary>
        /// <returns></returns>
        private bool ValidateSettings()
        {
            if (!_animalGameObject)
            {
                Debug.Log("Please select a scene Game Object to configure!");
                return false;
            }

            if (!malbersSettings)
            {
                Debug.Log("Please select some settings!");
                return false;
            }

            return true;
        }

        [Button("Configure Animal", ButtonSizes.Large)]
        private void ConfigureAnimal()
        {
            if (!ValidateSettings())
            {
                return;
            }
            GetAnimalReferences(_animalGameObject);

            _animator.runtimeAnimatorController = malbersSettings.animatorController;
            _animator.avatar = malbersSettings.animationAvatar;

            ConfigureEyes();
            ConfigureLookAt();
            _animal.RootBone = _rootBone;
        }

        /// <summary>
        /// Configure the LookAt components
        /// </summary>
        [Button("Configure LookAt")]
        private void ConfigureLookAt()
        {
            if (!ValidateSettings())
            {
                return;
            }
            GetAnimalReferences(_animalGameObject);

            _aim.AimOrigin = _neckBone;

            MalbersAnimations.Utilities.LookAt.BoneRotation[] newBones =
                new MalbersAnimations.Utilities.LookAt.BoneRotation[1];

            MalbersAnimations.Utilities.LookAt.BoneRotation newBone1 =
                new MalbersAnimations.Utilities.LookAt.BoneRotation();

            MalbersAnimations.Utilities.LookAt.BoneRotation newBone2 =
                new MalbersAnimations.Utilities.LookAt.BoneRotation();

            newBone1.bone = _neckBone;
            newBone1.offset = malbersSettings.neckLookAtOffset;

            newBone2.bone = _headBone;
            newBone2.offset = malbersSettings.headLookAtOffset;

            newBones[0] = newBone2;
            // newBones[1] = newBone2;

            _lookAt.Bones = newBones;
        }

        /// <summary>
        /// Add and configure the eyes
        /// </summary>
        [Button("Configure Eyes")]
        private void ConfigureEyes()
        {
            if (!ValidateSettings())
            {
                return;
            }
            GetAnimalReferences(_animalGameObject);

            Transform eyes = _headBone.Find("eyes");

            if (!eyes)
            {
                GameObject newEyes = new GameObject("eyes")
                {
                    transform =
                    {
                        parent = _headBone,
                        localPosition = Vector3.zero,
                        localRotation = Quaternion.identity,
                        localScale = Vector3.zero
                    }
                };

                eyes = newEyes.transform;
            }

            // Reposition
            Vector3 newEyesPosition = new Vector3(eyes.position.x, eyes.position.y + malbersSettings.eyesYModifier, eyes.position.z + malbersSettings.eyesZModifier);
            eyes.transform.position = newEyesPosition;

            // Update the eyes
            _aiAnimalBrain.Eyes = eyes;
        }

        /// <summary>
        /// Configure Animations
        /// </summary>
        [Button("Configure Animations")]
        private void ConfigureAnimations()
        {
            if (!ValidateSettings())
            {
                return;
            }
            GetAnimalReferences(_animalGameObject);
            
            ConfigureIdleAnimations();
            ConfigureDeathAnimations();
            ConfigureLocoAnimations();
            SaveAnimatorAssetChanges();
        }

        private void SetAnimatorControllerStateAnim(string subStateName, string stateName, AnimationClip clip)
        {
            if (!clip)
            {
                return;
            }

            // Get the base layer state machine
            AnimatorStateMachine baseSm = _animatorController.layers[0].stateMachine;

            // Find the sub-state
            foreach (ChildAnimatorStateMachine subStateSm in baseSm.stateMachines)
            {
                if (subStateSm.stateMachine.name == subStateName)
                {
                    // Found the State Machine
                    foreach (ChildAnimatorState animState in subStateSm.stateMachine.states)
                    {
                        if (animState.state.name == stateName)
                        {
                            // Found the state - set the clip
                            animState.state.motion = clip;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the state anim based on the substates and state name given
        /// </summary>
        /// <param name="subStateName"></param>
        /// <param name="subState2Name"></param>
        /// <param name="stateName"></param>
        /// <param name="clip"></param>
        private void SetAnimatorControllerStateAnim(string subStateName, string subState2Name, string stateName, AnimationClip clip)
        {
            if (!clip)
            {
                return;
            }

            // Get the base layer state machine
            AnimatorStateMachine baseSm = _animatorController.layers[0].stateMachine;

            // Find the sub-state
            foreach (ChildAnimatorStateMachine subStateSm in baseSm.stateMachines)
            {
                if (subStateSm.stateMachine.name == subStateName)
                {
                    foreach (ChildAnimatorStateMachine subSubStateSm in subStateSm.stateMachine.stateMachines)
                    {
                        if (subSubStateSm.stateMachine.name == subState2Name)
                        {
                            // Found the State Machine
                            foreach (ChildAnimatorState animState in subSubStateSm.stateMachine.states)
                            {
                                if (animState.state.name == stateName)
                                {
                                    // Found the state - set the clip
                                    animState.state.motion = clip;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetAnimatorControllerBlendTreeAnim(string subStateName, string blendTreeName, int clipIndex, AnimationClip clip)
        {
            if (!clip)
            {
                return;
            }

            // Get the base layer state machine
            AnimatorStateMachine baseSm = _animatorController.layers[0].stateMachine;

            // Find the sub-state
            foreach (ChildAnimatorStateMachine subStateSm in baseSm.stateMachines)
            {
                if (subStateSm.stateMachine.name == subStateName)
                {
                    // Found the State Machine
                    foreach (ChildAnimatorState animState in subStateSm.stateMachine.states)
                    {
                        if (animState.state.name == blendTreeName)
                        {
                            // Found the Blend Tree
                            BlendTree blendTree = animState.state.motion as BlendTree;
                            ChildMotion[] newMotions = blendTree.children;

                            newMotions[clipIndex].motion = clip;

                            blendTree.children = newMotions;
                        }
                    }
                }
            }
        }

        private void ConfigureIdleAnimations()
        {
            SetAnimatorControllerStateAnim("Idles", "Default Idles", "Idle 01", malbersSettings.idle1);
            SetAnimatorControllerStateAnim("Idles", "Default Idles", "Idle 02", malbersSettings.idle2);
            SetAnimatorControllerStateAnim("Idles", "Default Idles", "Idle 03", malbersSettings.idle3);
            SetAnimatorControllerStateAnim("Idles", "Default Idles", "Idle 04", malbersSettings.idle1);
        }

        private void ConfigureDeathAnimations()
        {
            SetAnimatorControllerStateAnim("Death", "Death 1", malbersSettings.death1);
            SetAnimatorControllerStateAnim("Death", "Death 2", malbersSettings.death2);
            SetAnimatorControllerStateAnim("Death", "Death 3", malbersSettings.death3);
        }

        private void ConfigureLocoAnimations()
        {
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 0, malbersSettings.locoIdle);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 1, malbersSettings.walkForward);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 2, malbersSettings.walkLeft);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 3, malbersSettings.walkRight);

            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 4, malbersSettings.rotateLeft);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 5, malbersSettings.rotateRight);

            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 6, malbersSettings.trotForward);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 7, malbersSettings.trotLeft);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 8, malbersSettings.trotRight);

            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 9, malbersSettings.runForward);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 10, malbersSettings.runLeft);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 11, malbersSettings.runRight);

            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 12, malbersSettings.walkForward);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 13, malbersSettings.walkLeft);
            SetAnimatorControllerBlendTreeAnim("Locomotion", "Locomotion", 14, malbersSettings.walkRight);
        }

        /// <summary>
        /// Update component references
        /// </summary>
        private void GetAnimalReferences(GameObject animalGameObject)
        {
            _animator = animalGameObject.GetComponent<Animator>();
            RuntimeAnimatorController runtimeAnimatorController = _animator.runtimeAnimatorController;
            _animatorController  = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(UnityEditor.AssetDatabase.GetAssetPath(runtimeAnimatorController));
            _animal = animalGameObject.GetComponentInChildren<MAnimal>(true);
            _aiAnimalControl = animalGameObject.GetComponentInChildren<MAnimalAIControl>(true);
            _aiAnimalBrain = animalGameObject.GetComponentInChildren<MAnimalBrain>(true);
            _navMeshAgent = animalGameObject.GetComponentInChildren<NavMeshAgent>(true);
            _lookAt = animalGameObject.GetComponentInChildren<MalbersAnimations.Utilities.LookAt>(true);
            _aim = animalGameObject.GetComponentInChildren<Aim>(true);

            _rootBone = skeleton.FindDeepChild("root");

            // Find the head
            _headBone = skeleton.FindDeepChild("head");
            if (!_headBone)
            {
                Debug.Log("Couldn't find the animal head!");
            }

            if (_headBone)
            {
                _neckBone = _headBone.parent;
            }
        }

        private void SaveAnimatorAssetChanges()
        {
            EditorUtility.SetDirty(_animatorController);
        }
    }
}