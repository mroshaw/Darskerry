#if HAP
using DaftAppleGames.Common.Characters;
using DaftAppleGames.Common.GameControllers;
using MalbersAnimations.HAP;
using PixelCrushers.Wrappers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Spawning
{
    public class HorseSpawner : MonoBehaviour
    {
        [BoxGroup("Horse Settings")]
        public GameObject horsePrefab;
        [BoxGroup("Horse Settings")]
        public Transform horseSpawnTarget;
        [BoxGroup("Debug")]
        public GameObject horseGameObject;

        private MRider _horseRider;

        /// <summary>
        /// Spawn the selected player game object in the the given transform location
        /// </summary>
        public void SpawnHorse(GameObject horseRiderGameObject)
        {
            // Check that we've not already spawned the horse. If so, we'll re-position.
            if (horseGameObject != null)
            {
                horseGameObject.transform.position = horseSpawnTarget.position;
                return;
            }
            
            horseGameObject = Instantiate(horsePrefab, horseSpawnTarget.position, horseSpawnTarget.rotation);
            horseGameObject.SetActive(true);
            
            // Set the Store Mount, to allow the player to call the horse straight away
            _horseRider = horseRiderGameObject.GetComponent<MRider>();

            // horseRider.UpdateCanMountDismount();
            // Tell the horse what spawned it
            Horse horse = horseGameObject.GetComponent<Horse>();
            horse.spawner = this;

            // Apply saved data, if appropriate
            if (GameController.Instance.IsLoadingFromSave)
            {
                ApplySaveData();
            }
            
        }

        /// <summary>
        /// Sets the horse as the players mount
        /// </summary>
        public void SetHorseAsMount()
        {
            _horseRider.Set_StoredMount(horseGameObject);
        }

        /// <summary>
        /// Apply Savers on the spawned horse
        /// </summary>
        private void ApplySaveData()
        {
            SaveSystem.RecursivelyApplySavers(horseGameObject.transform);
        }
    }
}
#endif