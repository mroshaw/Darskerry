#if PIXELCRUSHERS
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DaftAppleGames.Common.Quests
{
    public class StartDialogue : MonoBehaviour
    {
        /// <summary>
        /// Starts an "internal" dialog with the player
        /// </summary>
        /// <param name="player"></param>
        public void StartInternalDialog(GameObject player)
        {
            DialogueActor actor = player.GetComponent<DialogueActor>();
            if (!actor)
            {
                return;
            }
        }
    }
}
#endif