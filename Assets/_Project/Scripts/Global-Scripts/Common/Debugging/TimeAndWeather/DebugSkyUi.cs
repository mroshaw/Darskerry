using System;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DaftAppleGames.Common.Debugging
{
    public class DebugSkyUi : DebugBaseUi
    {
        [BoxGroup("UI Settings")] public TMP_Dropdown skyOptionsDropdown;
        [BoxGroup("Settings")] public string debugSkyObjectName;

        private DebugSky _debugSky;

        /// <summary>
        /// Set up the component
        /// </summary>
        public override void Start()
        {
            base.Start();

            if (string.IsNullOrEmpty(debugSkyObjectName))
            {
                Debug.LogError($"DebugSkyUi: Please set the debugSkyObjectName property on {gameObject.name}!");
            }
            else
            {
                _debugSky = (DebugSky)base.FindDebugObject<DebugSky>(debugSkyObjectName);
            }

            PopulateSkyOptions();
        }

        /// <summary>
        /// Populate the list of Sky Options
        /// </summary>
        private void PopulateSkyOptions()
        {

            skyOptionsDropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (DebugSkyOptions option in Enum.GetValues(typeof(DebugSkyOptions)))
            {
                TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
                newOption.text = option.ToString();
                options.Add(newOption);
            }

            skyOptionsDropdown.options = options;
        }


        /// <summary>
        /// Handler for set detail button
        /// </summary>
        public void ApplyOptionsProxy()
        {
            _debugSky.ApplyOption((DebugSkyOptions)skyOptionsDropdown.value);
        }
    }
}