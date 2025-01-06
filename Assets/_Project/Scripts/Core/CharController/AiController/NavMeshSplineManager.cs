using UnityEngine;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using Unity.Behavior;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class NavMeshSplineManager : MonoBehaviour
    {
         #region Editor methods
        #if UNITY_EDITOR
        [Button("Refresh All Modifiers")]
        private void RefreshAllModifiers()
        {
            foreach (NavMeshSplineModifier modifier in GetComponentsInChildren<NavMeshSplineModifier>(false))
            {
                modifier.UpdateNavMeshModifiers();
            }

            RefreshNavMeshSurfaces();
        }

        internal static void RefreshNavMeshSurfaces()
        {
            foreach(NavMeshSurface surface in FindObjectsByType<NavMeshSurface>(FindObjectsSortMode.None))
            {
                surface.BuildNavMesh();
            }
        }
#endif
        #endregion
    }
}