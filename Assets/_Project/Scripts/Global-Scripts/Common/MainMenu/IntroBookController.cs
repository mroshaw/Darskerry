#if BOOK
using DaftAppleGames.Common.Books;
using UnityEngine;

namespace DaftAppleGames.Common.MainMenu
{
    public class IntroBookController : MonoBehaviour
    {
        [Header("Intro Book Settings")]
        public BookController bookController;
       
        /// <summary>
        /// Hide the book
        /// </summary>
        void Awake()
        {
            bookController.gameObject.SetActive(false);
        }

        /// <summary>
        /// Show the Intro Book
        /// </summary>
        public void Show()
        {
            bookController.gameObject.SetActive(true);
            bookController.Show();
        }

        /// <summary>
        /// Hide the Intro Book
        /// </summary>
        public void Hide()
        {
            bookController.gameObject.SetActive(false);
        }
    }
}
#endif
