using System;
using UnityEngine;

namespace DaftAppleGames.Common.Settings
{
    /// <summary>
    /// Static class of methods to help with player settings
    /// </summary>
    public static class SettingsUtils
    {
        /// <summary>
        /// Save a "float" type to Player Settings
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="value"></param>
        public static void SaveFloatSetting(string settingKey, float value)
        {
            PlayerPrefs.SetFloat(settingKey, value);
        }

        /// <summary>
        /// Save an "int" type to Player Settings
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="value"></param>
        public static void SaveIntSetting(string settingKey, int value)
        {
            PlayerPrefs.SetInt(settingKey, value);
        }

        /// <summary>
        /// Save a "string" type to Player Settings
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="value"></param>
        public static void SaveStringSetting(string settingKey, string value)
        {
            PlayerPrefs.SetString(settingKey, value);
        }
        
        /// <summary>
        /// Save a "bool" type to Player Settings
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="value"></param>
        public static void SaveBoolSetting(string settingKey, bool value)
        {
            PlayerPrefs.SetInt(settingKey, Convert.ToInt32(value));
        }

        /// <summary>
        /// Load a "float" type Player Setting
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float LoadFloatSetting(string settingKey, float defaultValue)
        {
            float settingValue;

            // Check if key already exists and return it, otherwise return the default
            if (PlayerPrefs.HasKey(settingKey))
            {
                settingValue = PlayerPrefs.GetFloat(settingKey);
            }
            else
            {
                settingValue = defaultValue;
            }
            return settingValue;
        }

        /// <summary>
        /// Load a "bool" type Player setting
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool LoadBoolSetting(string settingKey, bool defaultValue)
        {
            bool settingValue;

            // Check if key already exists and return it, otherwise return the default
            if (PlayerPrefs.HasKey(settingKey))
            {
                settingValue = Convert.ToBoolean(PlayerPrefs.GetInt(settingKey));
            }
            else
            {
                settingValue = defaultValue;
            }
            return settingValue;
        }

        /// <summary>
        /// Load an "integer" type Player setting
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int LoadIntSetting(string settingKey, int defaultValue)
        {
            int settingValue;

            // Check if key already exists and return it, otherwise return the default
            if (PlayerPrefs.HasKey(settingKey))
            {
                settingValue = PlayerPrefs.GetInt(settingKey);
            }
            else
            {
                settingValue = defaultValue;
            }
            return settingValue;
        }

        /// <summary>
        /// Load a "string" type Player setting
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string LoadStringSetting(string settingKey, string defaultValue)
        {
            string settingValue;
            
            // Check if key already exists and return it, otherwise return the default
            if (PlayerPrefs.HasKey(settingKey))
            {
                settingValue = PlayerPrefs.GetString(settingKey);
            }
            else
            {
                settingValue = defaultValue;
            }
            return settingValue;
            
        }
    }
}