using UnityEditor;
using UnityEngine;


namespace DaftAppleGames.Editor.ProjectTools
{
    public class HiddenObjectEditor : UnityEditor.Editor
    {
        [MenuItem("Daft Apple Games/Tools/Project/Show hidden objects")]
        private static void RevealHiddenGameObject(GameObject gameObject)
        {
            if (gameObject.hideFlags.HasFlag(HideFlags.HideInHierarchy))
            {
                Debug.Log("Revealing hidden GameObject " + gameObject.name, gameObject);
                gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
            }

            foreach (Transform child in gameObject.transform)
            {
                RevealHiddenGameObject(child.gameObject);
            }
        }
    }
}