using System.Linq;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    public class ColliderTools : MonoBehaviour
    {
        private static PropColliderEditorSettings _settings;

        /// <summary>
        /// Add colliders to search objects
        /// </summary>
        /// <param name="parentGameObject"></param>
        /// <param name="settings"></param>
        /// <param name="isPrefab"></param>
        public static void AddColliders(GameObject parentGameObject, PropColliderEditorSettings settings, bool isPrefab)
        {
            _settings = settings;
            
            MeshFilter[] allMeshes = parentGameObject.GetComponentsInChildren<MeshFilter>(true);
            foreach (MeshFilter mesh in allMeshes)
            {
                // Box colliders
                if (settings.boxSearchStrings.Length != 0)
                {
                    if (settings.boxSearchStrings.Any(mesh.name.Contains))
                    {
                        if (!mesh.gameObject.GetComponent<BoxCollider>())
                        {
                            Debug.Log($"ColliderTools: Adding BoxCollider to {mesh.gameObject.name}");
                            mesh.gameObject.AddComponent<BoxCollider>();
                        }
                    }
                }
                
                // Capsule colliders
                if (settings.capsuleSearchStrings.Length != 0)
                {
                    if (settings.capsuleSearchStrings.Any(mesh.name.Contains))
                    {
                        if (!mesh.gameObject.GetComponent<CapsuleCollider>())
                        {
                            Debug.Log($"ColliderTools: Adding CapsuleCollider to {mesh.gameObject.name}");
                            mesh.gameObject.AddComponent<CapsuleCollider>();
                        }
                    }
                }

                // Mesh collider
                if(settings.meshSearchStrings.Length != 0)
                {
                    if (settings.meshSearchStrings.Any(mesh.name.Contains))
                    {
                        if (!mesh.gameObject.GetComponent<MeshCollider>())
                        {
                            Debug.Log($"ColliderTools: Adding MeshCollider to {mesh.gameObject.name}");
                            mesh.gameObject.AddComponent<MeshCollider>();
                        }
                    }
                }
                
                // Mark prefab as "Dirty" to force save
                if(isPrefab)
                {
                    EditorTools.ForcePrefabSave();
                }
            }
        }
    }
}
