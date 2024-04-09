using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace DaftAppleGames.Common.Debugger
{
    public abstract class DebugBaseUi : MonoBehaviour
    {
        [BoxGroup("UI Settings")] public string panelHeadingText;
        [BoxGroup("UI Settings")] public TMP_Text panelHeading;

        /// <summary>
        /// Set the panel heading
        /// </summary>
        public virtual void Start()
        {
            panelHeading.text = panelHeadingText;
        }

        /// <summary>
        /// Find Debug objects given type and name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public DebugBase FindDebugObject<T>(string objectName)
        {
            DebugBase[] allDebugObjects = FindObjectsOfType<DebugBase>();

            foreach (DebugBase debugObject in allDebugObjects)
            {
                if (debugObject.gameObject.name == objectName && debugObject is T)
                {
                    return debugObject;
                }
            }
            Debug.LogError($"DebugBaseUi: could not find object named {objectName} in loaded scenes! Please check: {gameObject.name}!");
            return null;
        }
    }
}