using Expanse;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.TimeAndWeather
{
    public class ExpanseCameraHelper : MonoBehaviour
    {
        void Awake()
        {
            // Find Expanse Global Settings and set the camera
            if(TryGetComponent<GlobalSettings>(out GlobalSettings expanseSettings))
            {
                Debug.Log("Setting Expanse camera...");
                expanseSettings.m_ambientProbeCamera = Camera.main;
            }
        }
    }
}