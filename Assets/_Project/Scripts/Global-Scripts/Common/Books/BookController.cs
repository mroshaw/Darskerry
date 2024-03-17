#if BOOK
using System.Linq;
using UnityEngine;
using echo17.EndlessBook;
using UnityEngine.Events;
using DaftAppleGames.Common.GameControllers;
using MalbersAnimations;
using Sirenix.OdinInspector;
using UnityEngine.Rendering.HighDefinition;
using System;

namespace DaftAppleGames.Common.Books
{
    /// <summary>
    /// The type of action to occur from a page view
    /// </summary>
    public enum BookActionTypeEnum
    {
        ChangeState,
        TurnPage
    }

    /// <summary>
    /// Delegate that handles the action taken by a page view
    /// </summary>
    /// <param name="actionType">The type of action to perform</param>
    /// <param name="actionValue">The value of the action (state or page number)</param>
    public delegate void BookActionDelegate(BookActionTypeEnum actionType, int actionValue);


    /// </summary>
    public class BookController : MonoBehaviour
    {
        [BoxGroup("Book Settings")] protected bool audioOn = false;
        [BoxGroup("Book Settings")] protected bool flipping = false;
        [BoxGroup("Book Settings")] public float openCloseTime = 0.3f;
        [BoxGroup("Book Settings")] public EndlessBook.PageTurnTimeTypeEnum groupPageTurnType;
        [BoxGroup("Book Settings")] public float singlePageTurnTime;
        [BoxGroup("Book Settings")] public float groupPageTurnTime;
        [BoxGroup("Book Settings")] public int tableOfContentsPageNumber;
        [BoxGroup("Book Settings")] public bool resetBeforeClose = false;

        [BoxGroup("Startup Settings")] public bool moveToCameraOnStart;
        [BoxGroup("Startup Settings")] public float distanceFromCamera = 0.5f;
        [BoxGroup("Startup Settings")] public bool showOnStart;

        [BoxGroup("Audio Settings")] public AudioClip bookOpenSound;
        [BoxGroup("Audio Settings")]  public AudioClip bookCloseSound;
        [BoxGroup("Audio Settings")] public AudioClip pageTurnSound;
        [BoxGroup("Audio Settings")] public AudioClip pagesFlippingSound;
        [BoxGroup("Audio Settings")] public float pagesFlippingSoundDelay;
        
        [BoxGroup("Complex Page Interactions")] public PageView[] pageViews;

        [FoldoutGroup("Events")] public UnityEvent OnBookShow;
        [FoldoutGroup("Events")] public UnityEvent OnBookReset;
        [FoldoutGroup("Events")] public UnityEvent OnBookHide;
        [FoldoutGroup("Events")] public UnityEvent OnBookFirstOpen;
        [FoldoutGroup("Events")] public UnityEvent OnBookOpen;
        [FoldoutGroup("Events")] public UnityEvent OnPageForward;
        [FoldoutGroup("Events")] public UnityEvent OnPageBackw;
        [FoldoutGroup("Events")] public UnityEvent OnPageTurn;
        [FoldoutGroup("Events")] public UnityEvent OnBookClose;


        private EndlessBook _book;
        private AudioSource _audioSource;

        private TouchPad _touchPad;

        private bool _bookOpened = false;
        private GameObject _mainCameraGameObject;

        /// <summary>
        /// Configure the book
        /// </summary>
        private void Awake()
        {
            // Initialise the book
            _book = GetComponent<EndlessBook>();
            _touchPad = GetComponent<TouchPad>();
            _audioSource = GetComponent<AudioSource>();

            // turn off all the mini-scenes since no pages are visible
            TurnOffAllPageViews();

            // set up touch pad handlers
            _touchPad.touchDownDetected = TouchPadTouchDownDetected;
            _touchPad.touchUpDetected = TouchPadTouchUpDetected;
            _touchPad.dragDetected = TouchPadDragDetected;

            ResetBook();

            // turn on the audio now that the book state is set the first time,
            // otherwise we'd hear a noise and no change would occur
            audioOn = true;

        }

        /// <summary>
        /// Set up the component
        /// </summary>
        private void Start()
        {
            _mainCameraGameObject = PlayerCameraManager.Instance.MainCamera.gameObject;

            if (moveToCameraOnStart)
            {
                MoveBookToCamera();
            }
        }
        
        /// <summary>
        /// Show the book
        /// </summary>
        public void Show()
        {
            OnBookShow.Invoke();
            MoveBookToCamera();
        }

        /// <summary>
        /// Reset the book back to front cover
        /// </summary>
        public void ResetBook(StateChangedDelegate onCompleted = null)
        {
            // set the book closed
            _book.SetState(EndlessBook.StateEnum.ClosedFront, 1.0f, ResetBookDelegate);
        }

        private void ResetBookDelegate(EndlessBook.StateEnum fromState, EndlessBook.StateEnum toState, int pageNumber)
        {
            OnBookReset.Invoke();
        }
        
        /// <summary>
        /// Called when the book's state changes
        /// </summary>
        /// <param name="fromState">Previous state</param>
        /// <param name="toState">Current state</param>
        /// <param name="pageNumber">Current page number</param>
        protected virtual void OnBookStateChanged(EndlessBook.StateEnum fromState, EndlessBook.StateEnum toState, int pageNumber)
        {
            switch (toState)
            {
                case EndlessBook.StateEnum.ClosedFront:

                    // play the closed sound
                    if (audioOn)
                    {
                        _audioSource.PlayOneShot(bookCloseSound);
                    }

                    // turn off page mini-scenes
                    TurnOffAllPageViews();

                    break;

                case EndlessBook.StateEnum.ClosedBack:

                    // play the closed sound
                    if (audioOn)
                    {
                        _audioSource.PlayOneShot(bookCloseSound);
                    }

                    // turn off page mini-scenes
                    TurnOffAllPageViews();
                    if (resetBeforeClose)
                    {
                        ResetBook();
                    }
                    OnBookClose.Invoke();

                    break;

                case EndlessBook.StateEnum.OpenMiddle:

                    if (fromState != EndlessBook.StateEnum.OpenMiddle)
                    {
                        // play open sound
                        //_audioSource.PlayOneShot(bookOpenSound);
                        _audioSource.PlayOneShot(pageTurnSound);
                    }
                    else
                    {
                        // stop the flipping sound
                        flipping = false;
                        _audioSource.Stop();
                    }

                    // turn off the front and back page mini-scenes
                    TogglePageView(0, false);
                    TogglePageView(999, false);

                    break;

                case EndlessBook.StateEnum.OpenFront:
                    if(fromState == EndlessBook.StateEnum.ClosedFront)
                    {
                        _audioSource.PlayOneShot(bookOpenSound);
                    }
                    else
                    {
                        _audioSource.PlayOneShot(pageTurnSound);
                    }
                    break;
                case EndlessBook.StateEnum.OpenBack:

                    // play the open sound
                    // _audioSource.PlayOneShot(bookOpenSound);
                    _audioSource.PlayOneShot(pageTurnSound);

                    if (!_bookOpened)
                    {
                        _bookOpened = true;
                        OnBookFirstOpen.Invoke();
                    }

                    OnBookOpen.Invoke();

                    break;
            }

            // turn on the touchpad
            ToggleTouchPad(true);
        }

        /// <summary>
        /// Toggle the touchpad on and off
        /// </summary>
        /// <param name="on">Whether the touchpad is on</param>
        protected virtual void ToggleTouchPad(bool on)
        {
            // left page should only be available if the book is not in the ClosedFront state
            _touchPad.Toggle(TouchPad.PageEnum.Left, on && _book.CurrentState != EndlessBook.StateEnum.ClosedFront);

            // right page should only be available if the book is not in the ClosedBack state
            _touchPad.Toggle(TouchPad.PageEnum.Right, on && _book.CurrentState != EndlessBook.StateEnum.ClosedBack);
        }

        /// <summary>
        /// Deactivates all the page mini-scenes
        /// </summary>
        protected virtual void TurnOffAllPageViews()
        {
            for (var i = 0; i < pageViews.Length; i++)
            {
                if (pageViews[i] != null)
                {
                    pageViews[i].Deactivate();
                }
            }
        }

        /// <summary>
        /// Turns a page mini-scene on or off
        /// </summary>
        /// <param name="pageNumber">The page number</param>
        /// <param name="on">Whether the mini-scene is on or off</param>
        protected virtual void TogglePageView(int pageNumber, bool on)
        {
            var pageView = GetPageView(pageNumber);

            if (pageView != null)
            {
                if (pageView != null)
                {
                    if (on)
                    {
                        pageView.Activate();
                    }
                    else
                    {
                        pageView.Deactivate();
                    }
                }
            }
        }

        /// <summary>
        /// Handler for when a page starts to turn.
        /// We play a sound, turn of the touchpad, and toggle
        /// page view mini-scenes.
        /// </summary>
        /// <param name="page">The page the starting turning</param>
        /// <param name="pageNumberFront">The page number of the front of the page</param>
        /// <param name="pageNumberBack">The page number of the back of hte page</param>
        /// <param name="pageNumberFirstVisible">The page number of the first visible page in the book</param>
        /// <param name="pageNumberLastVisible">The page number of the last visible page in the book</param>
        /// <param name="turnDirection">The direction the page is turning</param>
        protected virtual void OnPageTurnStart(Page page, int pageNumberFront, int pageNumberBack, int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
        {
            // play page turn sound if not flipping through multiple pages
            if (!flipping)
            {
                _audioSource.PlayOneShot(pageTurnSound);
            }

            // turn off the touch pad
            ToggleTouchPad(false);

            // turn on the front and back page views of the page if necessary
            TogglePageView(pageNumberFront, true);
            TogglePageView(pageNumberBack, true);

            switch (turnDirection)
            {
                case Page.TurnDirectionEnum.TurnForward:

                    // turn on the last visible page view if necessary
                    TogglePageView(pageNumberLastVisible, true);

                    break;

                case Page.TurnDirectionEnum.TurnBackward:

                    // turn on the first visible page view if necessary
                    TogglePageView(pageNumberFirstVisible, true);

                    break;
            }
        }

        /// <summary>
        /// Handler for when a page stops turning.
        /// We toggle the page views for the mini-scenes off for the relevent pages
        /// </summary>
        /// <param name="page">The page the starting turning</param>
        /// <param name="pageNumberFront">The page number of the front of the page</param>
        /// <param name="pageNumberBack">The page number of the back of hte page</param>
        /// <param name="pageNumberFirstVisible">The page number of the first visible page in the book</param>
        /// <param name="pageNumberLastVisible">The page number of the last visible page in the book</param>
        /// <param name="turnDirection">The direction the page is turning</param>
        protected virtual void OnPageTurnEnd(Page page, int pageNumberFront, int pageNumberBack, int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
        {
            switch (turnDirection)
            {
                case Page.TurnDirectionEnum.TurnForward:

                    // turn off the two pages that are now hidden by this page
                    TogglePageView(pageNumberFirstVisible - 1, false);
                    TogglePageView(pageNumberFirstVisible - 2, false);

                    break;

                case Page.TurnDirectionEnum.TurnBackward:

                    // turn off the two pages that are now hidden by this page
                    TogglePageView(pageNumberLastVisible + 1, false);
                    TogglePageView(pageNumberLastVisible + 2, false);

                    break;
            }
        }

        /// <summary>
        /// Turns to the table of contents
        /// </summary>
        protected virtual void TableOfContentsDetected()
        {
            TurnToPage(tableOfContentsPageNumber);
        }

        /// <summary>
        /// Handles whether a mouse down was detected on the touchpad
        /// </summary>
        /// <param name="page">The page that was hit</param>
        /// <param name="hitPointNormalized">The normalized hit point on the page</param>
        protected virtual void TouchPadTouchDownDetected(TouchPad.PageEnum page, Vector2 hitPointNormalized)
        {
            if (_book.CurrentState == EndlessBook.StateEnum.OpenMiddle)
            {
                PageView pageView;

                switch (page)
                {
                    case TouchPad.PageEnum.Left:

                        // get the left page view if available
                        pageView = GetPageView(_book.CurrentLeftPageNumber);

                        if (pageView != null)
                        {
                            // call touchdown on the page view
                            pageView.TouchDown();
                        }

                        break;

                    case TouchPad.PageEnum.Right:

                        // get the right page view if available
                        pageView = GetPageView(_book.CurrentRightPageNumber);

                        if (pageView != null)
                        {
                            // call the touchdown on the page view
                            pageView.TouchDown();
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Handles the touch up event from the touchpad
        /// </summary>
        /// <param name="page">The page that was hit</param>
        /// <param name="hitPointNormalized">The normalized hit point on the page</param>
        /// <param name="dragging">Whether we were dragging</param>
        protected virtual void TouchPadTouchUpDetected(TouchPad.PageEnum page, Vector2 hitPointNormalized, bool dragging)
        {
            switch (_book.CurrentState)
            {
                case EndlessBook.StateEnum.ClosedFront:

                    switch (page)
                    {
                        case TouchPad.PageEnum.Right:

                            // transition from the ClosedFront to the OpenFront states
                            OpenFront();

                            break;
                    }

                    break;

                case EndlessBook.StateEnum.OpenFront:

                    switch (page)
                    {
                        case TouchPad.PageEnum.Left:

                            // transition from the OpenFront to the ClosedFront states
                            ClosedFront();

                            break;

                        case TouchPad.PageEnum.Right:

                            // transition from the OpenFront to the OpenMiddle states
                            OpenMiddle();

                            break;
                    }

                    break;

                case EndlessBook.StateEnum.OpenMiddle:

                    PageView pageView;

                    if (dragging)
                    {
                        // get the left page view if available.
                        // in this demo we only have one group of pages that handle the drag: the map.
                        // instead of having logic for dragging on both pages, we'll just handle it on the left
                        pageView = GetPageView(_book.CurrentLeftPageNumber);

                        if (pageView != null)
                        {
                            // call the drag method on the page view
                            pageView.Drag(Vector2.zero, true);
                        }

                        return;
                    }

                    switch (page)
                    {
                        case TouchPad.PageEnum.Left:

                            // get the left page view if available
                            pageView = GetPageView(_book.CurrentLeftPageNumber);

                            if (pageView != null)
                            {
                                // cast a ray into the page and exit if we hit something (don't turn the page)
                                if (pageView.RayCast(hitPointNormalized, BookAction))
                                {
                                    return;
                                }
                            }

                            break;

                        case TouchPad.PageEnum.Right:

                            // get the right page view if available
                            pageView = GetPageView(_book.CurrentRightPageNumber);

                            if (pageView != null)
                            {
                                // cast a ray into the page and exit if we hit something (don't turn the page)
                                if (pageView.RayCast(hitPointNormalized, BookAction))
                                {
                                    return;
                                }
                            }

                            break;
                    }

                    break;

                case EndlessBook.StateEnum.OpenBack:

                    switch (page)
                    {
                        case TouchPad.PageEnum.Left:

                            // transition from the OpenBack to the OpenMiddle states
                            OpenMiddle();

                            break;

                        case TouchPad.PageEnum.Right:

                            // transition from the OpenBack to the ClosedBack states
                            ClosedBack();

                            break;
                    }

                    break;

                case EndlessBook.StateEnum.ClosedBack:

                    switch (page)
                    {
                        case TouchPad.PageEnum.Left:

                            // transition from the ClosedBack to the OpenBack states
                            OpenBack();

                            break;
                    }

                    break;

            }

            switch (page)
            {
                case TouchPad.PageEnum.Left:

                    if (_book.CurrentLeftPageNumber == 1)
                    {
                        // if on the first page, transition from the OpenMiddle to the OpenFront states
                        OpenFront();
                    }
                    else
                    {
                        // not on the first page, so just turn back one page
                        _book.TurnBackward(singlePageTurnTime, onCompleted: OnBookStateChanged, onPageTurnStart: OnPageTurnStart, onPageTurnEnd: OnPageTurnEnd);
                    }

                    break;

                case TouchPad.PageEnum.Right:

                    if (_book.CurrentRightPageNumber == _book.LastPageNumber)
                    {
                        // if on the last page, transition from the OpenMiddle to the OpenBack states
                        OpenBack();
                    }
                    else
                    {
                        // not on the last page, so just turn forward a page
                        _book.TurnForward(singlePageTurnTime, onCompleted: OnBookStateChanged, onPageTurnStart: OnPageTurnStart, onPageTurnEnd: OnPageTurnEnd);
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the drag event from the touchpad
        /// </summary>
        /// <param name="page">The page that was dragged on</param>
        /// <param name="touchDownPosition">The position of the touch</param>
        /// <param name="currentPosition">The current position</param>
        /// <param name="incrementalChange">The change of the touch positions between frames</param>
        protected virtual void TouchPadDragDetected(TouchPad.PageEnum page, Vector2 touchDownPosition, Vector2 currentPosition, Vector2 incrementalChange)
        {
            // only handle drag in the OpenMiddle state
            if (_book.CurrentState == EndlessBook.StateEnum.OpenMiddle)
            {
                // get the page view if available
                var pageView = GetPageView(_book.CurrentLeftPageNumber);

                if (pageView != null)
                {
                    // drag
                    pageView.Drag(incrementalChange, false);
                }
            }
        }

        /// <summary>
        /// Handler for a raycast hit on a page view
        /// </summary>
        /// <param name="actionType">The type of action to perform</param>
        /// <param name="actionValue">The value of the action (state or page number)</param>
        protected virtual void BookAction(BookActionTypeEnum actionType, int actionValue)
        {
            switch (actionType)
            {
                case BookActionTypeEnum.ChangeState:

                    // set the book state
                    SetState((EndlessBook.StateEnum)System.Convert.ToInt16(actionValue));

                    break;

                case BookActionTypeEnum.TurnPage:

                    // table of contents actions

                    if (actionValue == 999)
                    {
                        // go to the back page (OpenBack state)
                        OpenBack();
                    }
                    else
                    {
                        // turn to a page
                        TurnToPage(System.Convert.ToInt16(actionValue));
                    }

                    break;
            }
        }

        /// <summary>
        /// Gets the page view mini-scene of a page number
        /// </summary>
        /// <param name="pageNumber">The page number</param>
        /// <returns></returns>
        protected virtual PageView GetPageView(int pageNumber)
        {
            // search for a page view.
            // 0 = front page,
            // 999 = back page
            return pageViews.Where(x => x.name == string.Format("PageView_{0}", (pageNumber == 0 ? "Front" : (pageNumber == 999 ? "Back" : pageNumber.ToString("00"))))).FirstOrDefault();
        }

        /// <summary>
        /// Set the ClosedFront state
        /// </summary>
        protected virtual void ClosedFront()
        {
            SetState(EndlessBook.StateEnum.ClosedFront);
        }

        /// <summary>
        /// Set the OpenFront state
        /// </summary>
        protected virtual void OpenFront()
        {
            // toggle the front page view
            TogglePageView(0, true);

            SetState(EndlessBook.StateEnum.OpenFront);
        }

        /// <summary>
        /// Set the OpenMiddle state
        /// </summary>
        protected virtual void OpenMiddle()
        {
            // toggle the left and right page views
            TogglePageView(_book.CurrentLeftPageNumber, true);
            TogglePageView(_book.CurrentRightPageNumber, true);

            SetState(EndlessBook.StateEnum.OpenMiddle);
        }

        /// <summary>
        /// Set the OpenBack state
        /// </summary>
        protected virtual void OpenBack()
        {
            // toggle the back page view
            TogglePageView(999, true);

            SetState(EndlessBook.StateEnum.OpenBack);
        }

        /// <summary>
        /// Set the ClosedBack state
        /// </summary>
        protected virtual void ClosedBack()
        {
            SetState(EndlessBook.StateEnum.ClosedBack);
        }

        /// <summary>
        /// Set the state
        /// </summary>
        /// <param name="state">The state to set to</param>
        protected virtual void SetState(EndlessBook.StateEnum state)
        {
            // turn of the touch pad
            ToggleTouchPad(false);

            // set the state
            _book.SetState(state, openCloseTime, OnBookStateChanged);
        }

        /// <summary>
        /// Turns to a page
        /// </summary>
        /// <param name="pageNumber"></param>
        protected virtual void TurnToPage(int pageNumber)
        {
            var newLeftPageNumber = pageNumber % 2 == 0 ? pageNumber - 1 : pageNumber;

            // play the flipping sound if more than a single page is turning
            if (Mathf.Abs(newLeftPageNumber - _book.CurrentLeftPageNumber) > 2)
            {
                flipping = true;
                _audioSource.PlayDelayed(pagesFlippingSoundDelay);
            }

            // turn to page
            _book.TurnToPage(pageNumber, groupPageTurnType, groupPageTurnTime,
                            openTime: openCloseTime,
                            onCompleted: OnBookStateChanged,
                            onPageTurnStart: OnPageTurnStart,
                            onPageTurnEnd: OnPageTurnEnd);
        }


        /// <summary>
        /// Brings the book up to the camera
        /// </summary>
        public void MoveBookToCamera()
        {
            if(!_mainCameraGameObject)
            {
                _mainCameraGameObject = PlayerCameraManager.Instance.MainCamera.gameObject;
            }

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            
            Vector3 newPosition = _mainCameraGameObject.transform.position + (_mainCameraGameObject.transform.forward * distanceFromCamera);
            Quaternion newRotation = _mainCameraGameObject.transform.rotation * Quaternion.Euler(270.0f, 0, 0.0f);
            gameObject.transform.position = newPosition;
            gameObject.transform.rotation = newRotation;
        }
    }
}
#endif
