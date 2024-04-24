using Nova;
using UI.UI_Input;
using UnityEngine;

namespace UI.ButtonInput
{
    public class UITransitionToMenuButton : UIBlockTouchInputComponent
    {
        [SerializeField] protected MenuTransitionDirection _from;
        [SerializeField] protected MenuTransitionDirection _to;
        [SerializeField] protected Menu _targetMenu;
        protected override void HandleOnDrag(Gesture.OnDrag evt)
        {
        }

        protected override void HandleOnPress(Gesture.OnClick evt)
        {
            UIHandler.Instance.TransitionToMenu(_targetMenu, _from, _to);
        }

        protected override void HandleOnRelease(Gesture.OnRelease evt)
        {
        }
    }

}
