using Nova;
using UI.UI_Animations;
using UI.UI_Input;
using UnityEngine;

namespace UI.ButtonInput
{
    public class PopupMenuButton : UIBlockTouchInputComponent
    {
        public enum PopupState
        {
            Close, Open
        }
        [SerializeField] UIPopupAnimation _popupOpenAnimation;
        [SerializeField] UIPopupAnimation _popupCloseAnimation;
        [SerializeField] PopupState _popupState;
        [Tooltip("Duration of popup IN SECONDS")]
        [SerializeField] float _popupTime;
        public override void Start()
        {
            base.Start();
        }
        protected override void HandleOnDrag(Gesture.OnDrag evt)
        {
        }

        protected override void HandleOnPress(Gesture.OnClick evt)
        {
            AnimationHandle handle;
            switch (_popupState)
            {
                case PopupState.Open:
                    handle = _popupOpenAnimation.Run(_popupTime);
                    _popupState = PopupState.Close;
                    break;
                case PopupState.Close:
                default:
                    handle = _popupCloseAnimation.Run(_popupTime);
                    _popupState = PopupState.Open;
                    break;


            }
        }

        protected override void HandleOnRelease(Gesture.OnRelease evt)
        {
        }
    }
}
