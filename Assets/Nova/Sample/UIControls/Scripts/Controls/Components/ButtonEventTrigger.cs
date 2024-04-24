using Nova;
using UnityEngine;
using UnityEngine.Events;
namespace NovaSamples.UIControls
{
    public class ButtonEventTrigger : UIControl<ButtonVisuals>
    {
        [Tooltip("Event fired when the button is clicked.")]
        public UnityEvent OnClicked = null;

        private void OnEnable()
        {
            if (View.TryGetVisuals(out ButtonVisuals visuals))
            {
                // Set default state
                visuals.UpdateVisualState(VisualState.Default);
            }

            // Subscribe to desired events
            View.UIBlock.AddGestureHandler<Gesture.OnClick, ButtonVisuals>(HandleClicked);
        }

        private void OnDisable()
        {
            // Unsubscribe from events
            View.UIBlock.RemoveGestureHandler<Gesture.OnClick, ButtonVisuals>(HandleClicked);
        }

        /// <summary>
        /// Fire the Unity event on Click.
        /// </summary>
        /// <param name="evt">The click event data.</param>
        /// <param name="visuals">The buttons visuals which received the click.</param>
        private void HandleClicked(Gesture.OnClick evt, ButtonVisuals visuals) => OnClicked?.Invoke();
    }
}
