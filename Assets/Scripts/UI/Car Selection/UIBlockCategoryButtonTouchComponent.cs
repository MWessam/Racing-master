using Nova;
using UI.UI_Input;

namespace UI.Car_Selection
{
    public class UIBlockCategoryButtonTouchComponent : UIBlockTouchInputComponent
    {
        protected override void HandleOnDrag(Gesture.OnDrag evt)
        {
        }

        protected override void HandleOnPress(Gesture.OnClick evt)
        {
            _block.BodyEnabled = false;
        }

        protected override void HandleOnRelease(Gesture.OnRelease evt)
        {
        }
    }
}
