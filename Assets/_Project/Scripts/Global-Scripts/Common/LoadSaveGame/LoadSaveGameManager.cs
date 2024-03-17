#if PIXELCRUSHERS
using DaftAppleGames.Common.GameControllers;
using PixelCrushers;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using SaveSystem = PixelCrushers.Wrappers.SaveSystem;
using SaveGameDetailsData = DaftAppleGames.Common.LoadSaveGame.SaveDetailsSaver.SaveGameDetailsData;
namespace DaftAppleGames.Common.LoadSaveGame
{
    public class LoadSaveGameManager : MonoBehaviour
    {
        [FoldoutGroup("Events")] public UnityEvent onLoadedEvent;
        [FoldoutGroup("Events")] public UnityEvent onSavedEvent;

        /// <summary>
        /// Configure the component
        /// </summary>
        private void Awake()
        {
            SaveSystem.saveEnded -= OnSavedProxy;
            SaveSystem.loadEnded -= OnLoadedProxy;
            SaveSystem.saveEnded += OnSavedProxy;
            SaveSystem.loadEnded += OnLoadedProxy;
        }
        
        /// <summary>
        /// Delegate to invoke events
        /// </summary>
        public void OnLoadedProxy()
        {
            Debug.Log("Loaded!");
            onLoadedEvent.Invoke();
        }

        /// <summary>
        /// Delegate to invoke events
        /// </summary>
        public void OnSavedProxy()
        {
            onSavedEvent.Invoke();
        }
        
        /// <summary>
        /// Load from the specified slot
        /// </summary>
        /// <param name="slot"></param>
        public void LoadFromSlot(int slot)
        {
            Debug.Log($"Loading from slot {slot}...");
            GameController.Instance.IsLoadingFromSave = true;
            GameController.Instance.LoadSlot = slot;
            SaveSystem.LoadFromSlot(slot);
            Debug.Log($"Save Loaded from slot {slot}!");
        }

        /// <summary>
        /// Save to the specific slot
        /// </summary>
        /// <param name="slot"></param>
        public void SaveToSlot(int slot)
        {
            Debug.Log($"Saving to slot {slot}...");
            SaveSystem.SaveToSlot(slot);
            Debug.Log("Save Saved to slot {slot}!");
        }

        /// <summary>
        /// Retrieve the save game details from the given slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public SaveGameDetailsData GetSaveSlotDetails(int slot)
        {
            if (!SaveSystem.HasSavedGameInSlot(slot))
            {
                return null;
            }
            SavedGameData  saveData = SaveSystem.storer.RetrieveSavedGameData(slot);
            string saveDataString = saveData.GetData("GameSaveDetails");
            SaveGameDetailsData saveGameDetails = SaveSystem.Deserialize<SaveGameDetailsData>(saveDataString);
            return saveGameDetails;
        }
        
        /// <summary>
        /// Apply all Scene savers
        /// </summary>
        /// <param name="sceneName"></param>
        public void ApplySceneSaves(string sceneName)
        {
            if (!GameController.Instance.IsLoadingFromSave)
            {
                return;
            }

            Scene scene = SceneManager.GetSceneByName(sceneName);            
            
            Debug.Log($"Applying save data to {scene.name}...");
            var rootGOs = scene.GetRootGameObjects();
            for (int i = 0; i < rootGOs.Length; i++)
            {
                SaveSystem.RecursivelyApplySavers(rootGOs[i].transform);
            }
        }

        public void ApplyJournalSaveData()
        {

        }
    }
}
#endif