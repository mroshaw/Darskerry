// using Coffee.UIExtensions;

namespace DaftAppleGames.Common.UI
{
    /// <summary>
    /// Class to manage Button UI objects
    /// </summary>
    public class UiButton : UiObject
    {
        
        /// <summary>
        /// Lerp the "Shiny" UI effect if the button is selected
        /// </summary>
        public virtual void Update()
        {
            // Don't do anything if not selected
            if (!isSelected)
            {
                return;
            }
        }

        
        /*
        [Header("UI Effect Settings")]
        public float shinyDuration = 1.0f;
        public float shinyFrequency = 2.0f;
        private UIShiny _shinyComponent;
        private float _timer = 0.0f;
        private float _nextTimer = 0.0f;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public override void Start()
        {
            base.Start();
            _shinyComponent = GetComponent<UIShiny>();
        }
        
        /// <summary>
        /// Lerp the "Shiny" UI effect if the button is selected
        /// </summary>
        public virtual void Update()
        {
            // Don't do anything if not selected
            if (!isSelected || !_shinyComponent)
            {
                return;
            }

            // Wait for the frequency timer before starting
            if (_nextTimer < shinyFrequency)
            {
                _nextTimer += Time.unscaledDeltaTime;
                return;
            }
            
            // Reset the timers if we've finished one shine iteration
            if (_shinyComponent.effectFactor > 0.99f)
            {
                _timer = 0.0f;
                _nextTimer = 0.0f;
                _shinyComponent.effectFactor = 0.0f;
                return;
            }

            // Calculate and apply the shiny factor lerp
            _shinyComponent.effectFactor = Mathf.Lerp(0.0f, 1.0f, _timer / shinyDuration);
            _timer += Time.unscaledDeltaTime;
        }

        /// <summary>
        /// Reset shiny effect when deselected
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnDeselect(BaseEventData eventData)
        {
            if (_shinyComponent)
            {
                _shinyComponent.effectFactor = 0.0f;
            }
            base.OnDeselect(eventData);
        }
        */
    }
}