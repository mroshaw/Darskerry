using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if PIXELCRUSHERS
using SaveGameDetailsData = DaftAppleGames.Common.LoadSaveGame.SaveDetailsSaver.SaveGameDetailsData;
#endif
namespace DaftAppleGames.Common.LoadSaveGame 
{
    public enum LoadSaveMode { Load, Save }
    
    /// <summary>
    /// A MonoBehaviour class to handle Load and Save UI functionality
    /// </summary>
    public class LoadSaveGameUiManager : MonoBehaviour
    {
        [Header("Main UI Settings")]
        public GameObject mainUiGameObject;
        public GameObject alertUiGameObject;
        public GameObject firstSelectedGameObject;
        public float alertDelay = 2.0f;
        
        [Header("Text UI Settings")]
        public TMP_Text headingText;
        public TMP_Text slot1HeadingText;
        public TMP_Text slot2HeadingText;
        public TMP_Text slot3HeadingText;
        public TMP_Text alertText;
        
        [Header("Save UI Settings")]
        public GameSaveUi save1;
        public GameSaveUi save2;
        public GameSaveUi save3;

        [Header("Load Save Events")]
        [FoldoutGroup("Load Button 1 Events")]
        public UnityEvent onButton1LoadEvent;
        [FoldoutGroup("Load Button 2 Events")]
        public UnityEvent onButton2LoadEvent;
        [FoldoutGroup("Load Button 3 Events")]
        public UnityEvent onButton3LoadEvent;

        [FoldoutGroup("Save Button 1 Events")]
        public UnityEvent onButton1SaveEvent;
        [FoldoutGroup("Save Button 2 Events")]
        public UnityEvent onButton2SaveEvent;
        [FoldoutGroup("Save Button 3 Events")]
        public UnityEvent onButton3SaveEvent;
        
        [FoldoutGroup("Windows UI Events")]
        public UnityEvent onShowUiEvent;
        [FoldoutGroup("Windows UI Events")]
        public UnityEvent onHideUiEvent;
        [FoldoutGroup("Windows UI Events")]
        public UnityEvent onBackButtonEvent;

#if PIXELCRUSHERS
        private LoadSaveGameManager _saveGameManager;
#endif
        private LoadSaveMode _currentMode;

        /// <summary>
        /// Init the UI component
        /// </summary>
        private void Start()
        {
            mainUiGameObject.SetActive(false);
            alertUiGameObject.SetActive(false);
            EventSystem.current.firstSelectedGameObject = firstSelectedGameObject;
#if PIXELCRUSHERS
            _saveGameManager = GetComponent<LoadSaveGameManager>();
#endif
        }

        /// <summary>
        /// Show in Load mode
        /// </summary>
        public void ShowLoad()
        {
            Debug.Log("Show Load!");
            Show(LoadSaveMode.Load);
        }

        /// <summary>
        /// Show in Save mode
        /// </summary>
        public void ShowSave()
        {
            Debug.Log("Show Save!");
            Show(LoadSaveMode.Save);
        }
        
        /// <summary>
        /// Show the Load or Save UI
        /// </summary>
        /// <param name="mode"></param>
        public void Show(LoadSaveMode mode)
        {
            alertUiGameObject.SetActive(false);
            
            Debug.Log($"UI Show: {mode}");
            _currentMode = mode;
            
            switch (mode)
            {
                case LoadSaveMode.Load:
                    headingText.text = "Load Game";
                    slot1HeadingText.text = "Load";
                    slot2HeadingText.text = "Load";
                    slot3HeadingText.text = "Load";
                    break;
                case LoadSaveMode.Save:
                    headingText.text = "Save Game";
                    slot1HeadingText.text = "Save";
                    slot2HeadingText.text = "Save";
                    slot3HeadingText.text = "Save";
                    break;
            }

            PopulateSlots();
            mainUiGameObject.SetActive(true);
            onShowUiEvent.Invoke();
        }

        /// <summary>
        /// Hide the UI window
        /// </summary>
        public void Hide()
        {
            mainUiGameObject.SetActive(false);
            onHideUiEvent.Invoke();
        }

        /// <summary>
        /// Show the alert dialog
        /// </summary>
        private void ShowLoadSaveAlert()
        {
            if (_currentMode == LoadSaveMode.Load)
            {
                alertText.text = "Loading...";
            }

            if (_currentMode == LoadSaveMode.Save)
            {
                alertText.text = "Game Saved!";
            }
            alertUiGameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the Load / Save alert
        /// </summary>
        private void HideLoadSaveAlert()
        {
            alertUiGameObject.SetActive(false);
        }

        /// <summary>
        /// Public method to show Load alert
        /// </summary>
        public void ShowLoadAlert()
        {
            ShowLoadSaveAlert();
        }
        
        /// <summary>
        /// Public method to show the Save alert
        /// </summary>
        public void ShowSaveAlert()
        {
            StartCoroutine(ShowLoadSaveAlertAsync(alertDelay));
        }
        
        /// <summary>
        /// Show then hide the dialog
        /// </summary>
        /// <param name="numSeconds"></param>
        /// <returns></returns>
        private IEnumerator ShowLoadSaveAlertAsync(float numSeconds)
        {
            ShowLoadSaveAlert();
            yield return new WaitForSecondsRealtime(numSeconds);
            HideLoadSaveAlert();
            EventSystem.current.firstSelectedGameObject = firstSelectedGameObject;
        }
        
        /// <summary>
        /// Handles clicking the back button
        /// </summary>
        public void BackButtonHandler()
        {
            Hide();
            onBackButtonEvent.Invoke();
        }
        
        /// <summary>
        /// Used to map the load / save button to a method in the save manager component
        /// </summary>
        public void Button1Handler()
        {
            switch (_currentMode)
            {
                case LoadSaveMode.Load:
                    save1.saveButton.enabled = false;
                    onButton1LoadEvent.Invoke();
                    break;
                case LoadSaveMode.Save:
                    onButton1SaveEvent.Invoke();
                    break;
            }
        }

        /// <summary>
        /// Used to map the load / save button to a method in the save manager component
        /// </summary>
        public void Button2Handler()
        {
            switch (_currentMode)
            {
                case LoadSaveMode.Load:
                    save2.saveButton.enabled = false;
                    onButton2LoadEvent.Invoke();
                    break;
                case LoadSaveMode.Save:
                    onButton2SaveEvent.Invoke();
                    break;
            }
        }

        /// <summary>
        /// Used to map the load / save button to a method in the save manager component
        /// </summary>
        public void Button3Handler()
        {
            switch (_currentMode)
            {
                case LoadSaveMode.Load:
                    save3.saveButton.enabled = false;
                    onButton3LoadEvent.Invoke();
                    break;
                case LoadSaveMode.Save:
                    onButton3SaveEvent.Invoke();
                    break;
            }
        }

        /// <summary>
        /// Populate slot details
        /// </summary>
        public void PopulateSlots()
        {
#if PIXELCRUSHERS
            SaveGameDetailsData saveDetails;

            for (int slot = 1; slot <= 3; slot++)
            {
                saveDetails = _saveGameManager.GetSaveSlotDetails(slot);
                if (saveDetails != null)
                {
                    PopulateButtonSlot(slot, saveDetails.description, saveDetails.date, saveDetails.time, true);
                }
                else
                {
                    PopulateButtonSlot(slot, "EMPTY", "EMPTY", "EMPTY", false);
                }
            }
#endif
        }
        
        /// <summary>
        /// Populate a slot
        /// </summary>
        /// <param name="index"></param>
        /// <param name="saveDetails"></param>
        /// <param name="saveDate"></param>
        /// <param name="saveTime"></param>
        public void PopulateButtonSlot(int index, string saveDetails, string saveDate, string saveTime, bool enable)
        {
            switch (index)
            {
                case 1:
                    UpdateSlot(save1, saveDetails, saveDate, saveTime, enable);
                    break;
                case 2:
                    UpdateSlot(save2, saveDetails, saveDate, saveTime, enable);
                    break;
                case 3:
                    UpdateSlot(save3, saveDetails, saveDate, saveTime, enable);
                    break;
            }
        }

        /// <summary>
        /// Helper function to populate a specific slot with details
        /// </summary>
        /// <param name="saveSlot"></param>
        /// <param name="saveDetails"></param>
        /// <param name="saveDate"></param>
        /// <param name="saveTime"></param>
        private void UpdateSlot(GameSaveUi saveSlot, string saveDetails, string saveDate, string saveTime, bool enable)
        {
            saveSlot.saveNameText.text = saveDetails;
            saveSlot.saveDateText.text = saveDate;
            saveSlot.saveTimeText.text = saveTime;
            if (_currentMode == LoadSaveMode.Load)
            {
                saveSlot.saveButton.interactable = enable;
            }
            else
            {
                saveSlot.saveButton.interactable = true;
            }
        }
        
        /// <summary>
        /// Internal class to simplify definition of a slot
        /// </summary>
        [Serializable]
        public class GameSaveUi
        {
            public TMP_Text saveNameText;
            public TMP_Text saveDateText;
            public TMP_Text saveTimeText;
            public Button saveButton;
        }
    }
}
