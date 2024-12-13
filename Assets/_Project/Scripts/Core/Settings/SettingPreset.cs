using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public abstract class SettingPreset : ScriptableObject
    {
        [BoxGroup("Defaults")] public bool settingEnabled = true;
    }
}