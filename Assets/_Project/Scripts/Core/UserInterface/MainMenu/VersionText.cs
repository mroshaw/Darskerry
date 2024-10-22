using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace DaftAppleGames.Darskerry.Core.UserInterface.MainMenu
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionText : MonoBehaviour
    {
        private void Start()
        {
            TMP_Text versionText = GetComponent<TMP_Text>();
            versionText.text = Application.version.ToString();
        }
    }
}