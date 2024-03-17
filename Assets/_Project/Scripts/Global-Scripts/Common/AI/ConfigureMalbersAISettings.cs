using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

namespace DaftAppleGames.Scripts.Common.AI
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "ConfigureMalbersAISettings", menuName = "AI/Configure Malbers AI Settings", order = 1)]
    public class ConfigureMalbersAISettings : ScriptableObject
    {
#if UNITY_EDITOR
        [BoxGroup("Animation")] public Avatar animationAvatar;
        [BoxGroup("Animation")] public AnimatorController animatorController;
        [BoxGroup("General Animations")] public string animationsRootFolder = "Assets/_Project/Animations/Animals/";
        [BoxGroup("General Animations")] public AnimationClip idle1;
        [BoxGroup("General Animations")] public AnimationClip idle2;
        [BoxGroup("General Animations")] public AnimationClip idle3;
        [BoxGroup("General Animations")] public AnimationClip death1;
        [BoxGroup("General Animations")] public AnimationClip death2;
        [BoxGroup("General Animations")] public AnimationClip death3;
        [BoxGroup("Loco Animations")] public AnimationClip rotateLeft;
        [BoxGroup("Loco Animations")] public AnimationClip locoIdle;
        [BoxGroup("Loco Animations")] public AnimationClip rotateRight;
        [BoxGroup("Loco Animations")] public AnimationClip walkLeft;
        [BoxGroup("Loco Animations")] public AnimationClip walkForward;
        [BoxGroup("Loco Animations")] public AnimationClip walkRight;
        [BoxGroup("Loco Animations")] public AnimationClip trotLeft;
        [BoxGroup("Loco Animations")] public AnimationClip trotForward;
        [BoxGroup("Loco Animations")] public AnimationClip trotRight;
        [BoxGroup("Loco Animations")] public AnimationClip runLeft;
        [BoxGroup("Loco Animations")] public AnimationClip runForward;
        [BoxGroup("Loco Animations")] public AnimationClip runRight;

        [BoxGroup("Eyes")] public float eyesYModifier = 0.001f;
        [BoxGroup("Eyes")] public float eyesZModifier = 0.0008f;

        [BoxGroup("Look At")] public Vector3 neckLookAtOffset = new Vector3(0, -90, -90);
        [BoxGroup("Look At")] public Vector3 headLookAtOffset = new Vector3(0, -90, -90);

        [Button("Copy walk to rotate")]
        private void CopyWalkToRotate()
        {
            rotateLeft = walkLeft;
            rotateRight = walkRight;
        }

        [Button("Copy walk to trot")]
        private void CopyWalkToTrot()
        {
            trotForward = walkForward;
            trotLeft = walkLeft;
            trotRight = walkRight;
        }

        [Button("Copy run to trot")]
        private void CopyRunToTrot()
        {
            trotForward = runForward;
            trotLeft = runLeft;
            trotRight = runRight;
        }

        [Button("Copy trot to run")]
        private void CopyTrotToRun()
        {
            runForward = trotForward;
            runLeft = trotLeft;
            runRight = trotRight;
        }


        [Button("Find Animations")]
        private void FindAnimations()
        {
            idle1 = FindAnimation("idle1", "idle_1");
            idle2 = FindAnimation("idle2", "idle_2");
            idle3 = FindAnimation("idle3", "idle_3");

            death1 = FindAnimation("death1", "death_1");
            death2 = FindAnimation("death2", "death_2");
            death3 = FindAnimation("death3", "death_3");

            rotateLeft = FindAnimation("Rotate_left_", "Rotate_left_RM");
            locoIdle = FindAnimation("idle1", "idle_1");
            rotateRight = FindAnimation("Rotate_right_", "Rotate_right_RM");

            walkLeft = FindAnimation("Walk_left_", "Walk_left_RM");
            walkForward = FindAnimation("Walk_forward_", "Walk_forward_RM");
            walkRight = FindAnimation("Walk_right_", "Walk_right_RM");

            trotLeft = FindAnimation("Trot_left_", "Trot_left_RM");
            trotForward = FindAnimation("Trot_forward_", "Trot_forward_RM");
            trotRight = FindAnimation("Trot_right_", "Trot_right_RM");

            runLeft = FindAnimation("Run_left_", "Run_left_RM");
            runForward = FindAnimation("Run_forward_", "Run_forward_RM");
            runRight = FindAnimation("Run_right_", "Run_right_RM");


        }

        /// <summary>
        /// Finds an animation clip using one of the two string names
        /// </summary>
        /// <param name="animName"></param>
        /// <param name="animName2"></param>
        /// <returns></returns>
        private AnimationClip FindAnimation(string animName, string animName2)
        {
            AnimationClip clip = null;
            clip = FindAnimationClip(animName);

            if (clip)
            {
                return clip;
            }
            clip = FindAnimationClip(animName2);

            if (clip)
            {
                return clip;
            }

            return null;
        }

        /// <summary>
        /// Finds an animation clip using the name
        /// </summary>
        /// <param name="animName"></param>
        /// <returns></returns>
        private AnimationClip FindAnimationClip(string animName)
        {
            string[] assetGuids = AssetDatabase.FindAssets($"{animName} t:AnimationClip", new[] { animationsRootFolder });

            if (assetGuids.Length != 1)
            {
                return null;
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
            AnimationClip clip = (AnimationClip)AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimationClip));

            return clip;
        }
#endif
    }
}
