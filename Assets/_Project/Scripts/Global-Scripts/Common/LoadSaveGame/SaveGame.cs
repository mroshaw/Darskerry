/*
using System;
using System.Collections;
using PixelCrushers;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SaveGame
{
    public class SaveGame : LoadSaveGame
    {

        [Header("Save Game UI")]
        public GameObject gameSaveSuccessGameObject;
        public float timeToShowSuccess = 5.0f;
        public void Start()
        {
            gameSaveSuccessGameObject.SetActive(false);
        }

        private void ShowGameSaveSuccess()
        {
            StartCoroutine(ShowGameSaveSuccessAsync());
        }

        /// <summary>
        /// Show the success panel
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowGameSaveSuccessAsync()
        {
            gameSaveSuccessGameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(timeToShowSuccess);
            gameSaveSuccessGameObject.SetActive(false);
        }
        
        /// <summary>
        /// Save to slot
        /// </summary>
        /// <param name="slotNum"></param>
        private void SaveSlot(int slotNum)
        {
            SaveSystem.SaveToSlot(slotNum);
            ShowGameSaveSuccess();
            InitSlots();
        }
        
        /// <summary>
        /// Handle click of the slot 1 save button
        /// </summary>
        public void SaveSlot1(bool overwrite)
        {
            SaveSlot(1);
        }
        /// <summary>
        /// Handle click of the slot 1 save button
        /// </summary>
        public void SaveSlot2(bool overwrite)
        {
            SaveSlot(2);
        }

        /// <summary>
        /// Handle click of the slot 1 save button
        /// </summary>
        public void SaveSlot3(bool overwrite)
        {
            SaveSlot(3);
        }
    }
}
*/