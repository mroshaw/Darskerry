#if BOOK
using echo17.EndlessBook;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Books
{
    /// <summary>
    /// Class to manage readable game objects like books and diaries
    /// </summary>
    public class Readable : MonoBehaviour
    {
        [BoxGroup("Book Settings")] public BookController bookController;
        [BoxGroup("Book Settings")] public GameObject renderGameObject;

        [BoxGroup("Events")] public UnityEvent OnShowBookEvent;
        [BoxGroup("Events")] public UnityEvent OnHideBookEvent;

        private GameObject _bookControllerGameObject;

        private StateChangedDelegate onStateCompleted;

        /// <summary>
        /// Configure component on wake, before other components start
        /// </summary>
        private void Awake()
        {
            _bookControllerGameObject = bookController.gameObject;
            _bookControllerGameObject.SetActive(false);
            onStateCompleted = HideBookGameObjectDelegate;
        }
        
        /// <summary>
        /// Brings the interactive book up to the camera
        /// </summary>
        public void ShowBook()
        {
            // Enable the renderer components
            if (renderGameObject)
            {
                renderGameObject.SetActive(true);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _bookControllerGameObject.SetActive(true);
            bookController.MoveBookToCamera();
            OnShowBookEvent.Invoke();
        }

        /// <summary>
        /// Hides the interactive book
        /// </summary>
        public void HideBook()
        {
            bookController.ResetBook(onStateCompleted);
            OnHideBookEvent.Invoke();
        }

        /// <summary>
        /// Hides the book
        /// </summary>
        private void HideBookGameObjectDelegate(EndlessBook.StateEnum fromState, EndlessBook.StateEnum toState, int pageNumber)
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Hide the renderer components
            if (renderGameObject)
            {
                renderGameObject.SetActive(false);
            }
            
            _bookControllerGameObject.SetActive(false);
        }
    }
}
#endif