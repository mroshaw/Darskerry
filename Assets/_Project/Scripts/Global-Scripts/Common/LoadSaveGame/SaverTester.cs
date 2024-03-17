#if PIXELCRUSHERS
using System.Collections;
using PixelCrushers;
using UnityEngine;
using SaveSystem = PixelCrushers.Wrappers.SaveSystem;

namespace DaftAppleGames.Common.LoadSaveGame 
{
    public class SaverTester : MonoBehaviour
    {
        private Saver _saver;
        
        private void OnEnable()
        {
            SaveSystem.sceneLoaded += SceneLoaded;
            SaveSystem.loadEnded += LoadEnded;
        }
        
        private void OnDisable()
        {
            SaveSystem.sceneLoaded -= SceneLoaded;
            SaveSystem.loadEnded -= LoadEnded;
        }
        
        private void Awake()
        {
            _saver = GetComponent<Saver>();
        }
        
        private void Start()
        {

        }

        private void LoadEnded()
        {
            Debug.Log($"{gameObject.name} Save Loaded: Position: {gameObject.transform.position.ToString()}");
            Debug.Log($"{gameObject.name} Save Loaded: State: {gameObject.activeSelf}");
        }
        
        private void SceneLoaded(string scene, int mode)
        {
            Debug.Log($"{gameObject.name} Scene Loaded: Position: {gameObject.transform.position.ToString()}");
            Debug.Log($"{gameObject.name} Scene Loaded: State: {gameObject.activeSelf}");

            StartCoroutine(LogAfterDelay());
        }

        private IEnumerator LogAfterDelay()
        {
            yield return new WaitForSeconds(3);
            Debug.Log($"Async {gameObject.name} Scene Loaded: Position: {gameObject.transform.position.ToString()}");
        }
    }
}
#endif