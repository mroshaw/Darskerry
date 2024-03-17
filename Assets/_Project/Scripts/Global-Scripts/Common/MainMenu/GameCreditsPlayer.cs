using DaftAppleGames.Common.Ui;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DaftAppleGames.Common.MainMenu
{
    /// <summary>
    /// Credits scroller
    /// </summary>
    public class GameCreditsPlayer : BaseUiWindow
    {
        [BoxGroup("UI Settings")]
        public CanvasRenderer creditsPanel;
        [BoxGroup("Scroller Settings")]
        public float scrollSpeed = 30.0f;
        [BoxGroup("Scroller Settings")]
        public bool scrollOnStart;
        [BoxGroup("Scroller Settings")]
        public float stopBuffer = 600.0f;
        [BoxGroup("Scroller Settings")]
        public float timeToSkip = 2.0f;

        [BoxGroup("Credits")]
        public GameCreditSettings creditSettings;
        
        [BoxGroup("Template Settings")]
        public GameCreditEntryTemplate gameCreditEntryTemplate;
        [BoxGroup("Template Settings")]
        public GameCreditAssetTypeTemplate assetCreditTypeTemplate;
        [BoxGroup("Template Settings")]
        public GameCreditAssetEntryTemplate assetCreditEntryTemplate;

        [FoldoutGroup("Start Events")]
        public UnityEvent onScrollStartedEvent;
        [FoldoutGroup("Stop Events")]
        public UnityEvent onScrollStoppedEvent;
        
        private bool _isScrolling;
        private Vector3 _currentPosition;
        private Vector3 _startingPosition;
        private float _rectHeight;
        
        private RectTransform _rectTransform;
        private ContentSizeFitter _fitter;

        private float _runTime = 0.0f;
        
        /// <summary>
        /// Set the UI state appropriately on awake
        /// </summary>
        private void Awake()
        {
            _fitter = creditsPanel.GetComponent<ContentSizeFitter>();
            _isScrolling = false;
            ConstructUi();
            _startingPosition = new Vector3(0, -600, 0);
        }
        
        /// <summary>
        /// Initialise and start the scroller, if required
        /// </summary>
        public override void Start()
        {
            base.Start();
            gameCreditEntryTemplate.gameObject.SetActive(false);
            assetCreditTypeTemplate.gameObject.SetActive(false);
            assetCreditEntryTemplate.gameObject.SetActive(false);
        }

        /// <summary>
        /// Start scrolling
        /// </summary>
        public void StartScroll()
        {
            _fitter.SetLayoutHorizontal();
            _rectTransform = creditsPanel.GetComponent<RectTransform>();
            _currentPosition = _startingPosition;
            _isScrolling = true;
            onScrollStartedEvent.Invoke();
            _runTime = 0.0f;
        }

        /// <summary>
        /// Scroll the canvas until the user intervenes, or scrolling ends
        /// </summary>
        public void Update()
        {
            if (_isScrolling)
            {
                _runTime += Time.deltaTime;
                if (_runTime > timeToSkip)
                {
                    if (Input.anyKey)
                    {
                        StopScroll();
                    }
                }
                _rectTransform.localPosition = _currentPosition;
                _currentPosition.y += scrollSpeed * Time.deltaTime;
                _rectHeight = _rectTransform.rect.height;
                
                // Check if we've finished scrolling
                if (_rectHeight + stopBuffer - _rectTransform.localPosition.y < 0)
                {
                    StopScroll();
                }
            }
        }
        
        /// <summary>
        /// Stop scrolling and reset
        /// </summary>
        public void StopScroll()
        {
            _isScrolling = false;
            _currentPosition = _startingPosition;
            onScrollStoppedEvent.Invoke();
        }

        /// <summary>
        /// Pauses scrolling, allows resume
        /// </summary>
        public void PauseScroll()
        {
            _isScrolling = false;
        }
        
        /// <summary>
        /// Iterate over credits and construct the UI using the given templates
        /// </summary>
        private void ConstructUi()
        {
            ContructGameCreditsUi();
            ConstructAssetCreditsUi();
            Canvas.ForceUpdateCanvases();
        }

        /// <summary>
        /// Construct the main game credits
        /// </summary>
        private void ContructGameCreditsUi()
        {
            // Build out Game Credits
            foreach (GameCreditSettings.CreditDetails creditDetails in creditSettings.gameCredits)
            {
                GameObject newEntry = Instantiate(gameCreditEntryTemplate.gameObject, gameCreditEntryTemplate.transform.parent, true);
                newEntry.gameObject.transform.localScale = new Vector3(1, 1, 1);
                GameCreditEntryTemplate template = newEntry.GetComponent<GameCreditEntryTemplate>();
                template.nameText.text = creditDetails.name;
                template.roleText.text = creditDetails.role;
                newEntry.SetActive(true);
            }
        }

        /// <summary>
        /// Construct the 3rd party asset credits
        /// </summary>
        private void ConstructAssetCreditsUi()
        {
            // Build out 3rd Party Asset Credits
            int currentType = -1;
            GameObject currentTypeContainer = null;

            foreach (GameCreditSettings.AssetDetails assetDetails in creditSettings.SortedThirdPartyAssets)
            {
                // See if we need to create a new type container
                if ((int)assetDetails.assetType != currentType)
                {
                    GameObject newAssetType = Instantiate(assetCreditTypeTemplate.gameObject, assetCreditTypeTemplate.gameObject.transform.parent, true);
                    GameCreditAssetTypeTemplate typeTemplate = newAssetType.GetComponent<GameCreditAssetTypeTemplate>();
                    typeTemplate.transform.localScale = new Vector3(1, 1, 1);
                    typeTemplate.assetTypeText.text = assetDetails.FriendlyAssetName;
                    newAssetType.SetActive(true);
                    currentTypeContainer = newAssetType;
                    currentType = (int)assetDetails.assetType;
                }

                GameObject newAssetEntry = Instantiate(assetCreditEntryTemplate.gameObject, currentTypeContainer.transform, true);
                newAssetEntry.transform.localScale = new Vector3(1, 1, 1);
                GameCreditAssetEntryTemplate entryTemplate = newAssetEntry.GetComponent<GameCreditAssetEntryTemplate>();
                entryTemplate.assetNameText.text = assetDetails.assetName;
                entryTemplate.assetAuthorText.text = assetDetails.assetAuthor;
                newAssetEntry.SetActive(true);
            }
        }
    }
}
