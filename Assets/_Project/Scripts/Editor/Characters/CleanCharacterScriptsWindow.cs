using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.Characters
{
    public class CleanCharacterScriptsWindow : OdinEditorWindow
    {
        [MenuItem("Daft Apple Games/Characters/Clean Character Scripts")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CleanCharacterScriptsWindow));
        }

        [SerializeField] private bool disableInsteadOfDestroyCore = false;
        [SerializeField] private bool disableInsteadOfDestroyCollider = true;
        [SerializeField] private bool disableInsteadOfDestroyAnimator = false;

        [SerializeField] private GameObject[] selectedGameObject;

        private void OnSelectionChange()
        {
            selectedGameObject = Selection.gameObjects;
        }

        [Button("Clean Scripts", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void CleanScripts()
        {
            foreach(GameObject currGameObject in selectedGameObject)
            {
                RemoveScripts(currGameObject);
            }
        }

        private void RemoveScripts(GameObject currGameObject)
        {
            // Get prefab asset
            string prefabPath = "";
            GameObject instance;
            if (PrefabUtility.IsPartOfAnyPrefab(currGameObject))
            {
                prefabPath = AssetDatabase.GetAssetPath(currGameObject);
                instance = PrefabUtility.LoadPrefabContents(prefabPath);
            }
            else
            {
                instance = currGameObject;
            }

            // Physics Bone
            PhysicBonesCore[] physicsBones = instance.GetComponentsInChildren<PhysicBonesCore>(true);
            foreach (PhysicBonesCore physicBonesCore in physicsBones)
            {
                if (disableInsteadOfDestroyCore)
                {
                    physicBonesCore.enabled = false;
                }
                else
                {
                    DestroyImmediate(physicBonesCore);
                }
            }

            // Physics Bone Collider
            PhysicBonesCollider[] physicsBoneColliders = instance.GetComponentsInChildren<PhysicBonesCollider>(true);
            foreach (PhysicBonesCollider physicBoneCollider in physicsBoneColliders)
            {
                if (disableInsteadOfDestroyCollider)
                {
                    physicBoneCollider.enabled = false;
                }
                else
                {
                    DestroyImmediate(physicBoneCollider);
                }
            }


            // Animator
            Animator[] animators = instance.GetComponentsInChildren<Animator>(true);
            foreach (Animator animator in animators)
            {
                if (disableInsteadOfDestroyAnimator)
                {
                    animator.enabled = false;
                }
                else
                {
                    DestroyImmediate(animator);
                }
            }


            if (PrefabUtility.IsPartOfAnyPrefab(currGameObject))
            {
                PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
            }
        }
    }
}