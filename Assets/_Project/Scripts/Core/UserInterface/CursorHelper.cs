using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    public class CursorHelper : MonoBehaviour
    {
        /// <summary>
        /// Show cursor and allow control
        /// </summary>
        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// Lock and hide cursor
        /// </summary>
        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}