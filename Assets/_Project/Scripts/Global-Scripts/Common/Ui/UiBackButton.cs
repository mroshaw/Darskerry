using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.UI
{
    public class UiBackButton : UiButton
    {
        [Header("UI Settings")]
        private Button _button;
        public string gamePadBack = "Cancel";

        /// <summary>
        /// Get the component button
        /// </summary>
        public override void Start()
        {
            base.Start();
            _button = GetComponent<Button>();
        }

        /// <summary>
        /// If defined, the controller back button "clicks"
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (string.IsNullOrEmpty(gamePadBack))
            {
                return;

            }
            if (Input.GetButtonDown(gamePadBack))
            {
                _button.onClick.Invoke();
            }
        }
    }
}