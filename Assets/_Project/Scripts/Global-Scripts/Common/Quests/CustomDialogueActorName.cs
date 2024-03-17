#if PIXELCRUSHERS
using DaftAppleGames.Common.GameControllers;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DaftAppleGames.Common.Quests 
{
    /// <summary>
    /// Very simple Monobehaviour class to support custom Dialogue System player actor names
    /// </summary>
    public class CustomDialogueActorName : MonoBehaviour
    {
        public string maleName = "Callum";
        public string femaleName = "Emily";

        /// <summary>
        /// Set the Dialogue Actor Name depending on which character is selected.
        /// </summary>
        private void Start()
        {
            switch (GameController.Instance.SelectedCharacter)
            {
                case CharSelection.Callum:
                    DialogueLua.SetActorField("Player", "Display Name", maleName);
                    break;
                case CharSelection.Emily:
                    DialogueLua.SetActorField("Player", "Display Name", femaleName);
                    break;
            }
        }
    }
}
#endif