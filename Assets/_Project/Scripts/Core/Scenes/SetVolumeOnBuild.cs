using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Darskerry.Core.Scenes
{
    public class SetVolumeOnBuild : MonoBehaviour, IBuildApplier
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private float buildVolume;

        private float _currentVolume;

        public void BuildStart()
        {
            audioMixer.GetFloat("MasterVolume", out _currentVolume);
            audioMixer.SetFloat("MasterVolume", buildVolume);
        }

        public void BuildFinished()
        {
            audioMixer.SetFloat("MasterVolume", _currentVolume);
        }
    }
}