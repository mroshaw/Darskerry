using UnityEngine;
using UnityEngine.EventSystems;

namespace DaftAppleGames.Common.UI
{
    /// <summary>
    /// Class to underpin all UI components
    /// </summary>
    public class UiObject : MonoBehaviour, ISelectHandler, IDeselectHandler, ICancelHandler
    {
        [Header("UI Settings")]
        public GameObject selectFrame;
        public bool isSelected;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public virtual void Start()
        {
            isSelected = false;
        }
        
        /// <summary>
        /// Enable UI object frame when selected
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnSelect(BaseEventData eventData)
        {
            isSelected = true;
            
            if(selectFrame != null)
            {
                selectFrame.SetActive(true);
            }
        }

        /// <summary>
        /// Disable UI object frame when deselected
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDeselect(BaseEventData eventData)
        {
            isSelected = false;
            
            if (selectFrame != null)
            {
                selectFrame.SetActive(false);
            }
        }

        /// <summary>
        /// Disable UI object frame when canelled
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void OnCancel(BaseEventData eventData)
        {
            isSelected = false;
            
            if (selectFrame != null)
            {
                selectFrame.SetActive(false);
            }
        }

        /// <summary>
        /// Disable UI object frame if disabled
        /// </summary>
        public void OnDisable()
        {
            isSelected = false;
            
            if (selectFrame != null)
            {
                selectFrame.SetActive(false);
            }
        }
    }
}