using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Collections.Generic;

namespace DaftAppleGames.Common.Debugging
{
    public class DebugTeleporterUi : DebugBaseUi
    {
        [BoxGroup("UI Settings")] public TMP_Dropdown teleportTargetDropdown;
        [BoxGroup("Settings")] public string debugTeleporterObjectName;

        private DebugTeleporter _debugTeleporter;

        /// <summary>
        /// Set up the component
        /// </summary>
        public override void Start()
        {
            base.Start();

            if (string.IsNullOrEmpty(debugTeleporterObjectName))
            {
                Debug.LogError($"DebugTeleporterUi: Please set the debugTeleporterObjectName property on {gameObject.name}!");
            }
            else
            {
                _debugTeleporter = (DebugTeleporter)base.FindDebugObject<DebugTeleporter>(debugTeleporterObjectName);
            }
            PopulateTeleportTargets();
        }

        /// <summary>
        /// Populate the Teleport Target dropdown
        /// </summary>
        private void PopulateTeleportTargets()
        {

            teleportTargetDropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (DebugTeleporter.TeleportTarget teleportTarget in _debugTeleporter.teleportTargets)
            {
                TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
                newOption.text = teleportTarget.TargetName;
                options.Add(newOption);
            }

            teleportTargetDropdown.options = options;
        }

        /// <summary>
        /// Handler for the teleport button
        /// </summary>
        public void TeleportProxy()
        {
            string currentTargetName = teleportTargetDropdown.options[teleportTargetDropdown.value].text;
            _debugTeleporter.Teleport(currentTargetName);
        }
    }
}