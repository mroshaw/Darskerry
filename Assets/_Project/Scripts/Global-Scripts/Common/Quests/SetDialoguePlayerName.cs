using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

namespace DaftAppleGames.Common.Quests
{
    public class SetDialoguePlayerName : MonoBehaviour
    {
        [BoxGroup] public string maleDialogName = "Callum";
        [BoxGroup] public string femaleDialogName = "Emily";

        /// <summary>
        /// Set the Dialog Manager player name on Awake
        /// </summary>
        private void Start()
        {
            if (GameController.Instance.SelectedCharacter == CharSelection.Emily)
            {
                DialogueLua.SetActorField("Player", "Display Name", femaleDialogName);
            }
            else
            {
                DialogueLua.SetActorField("Player", "Display Name", maleDialogName);
            }
        }
    }
}
