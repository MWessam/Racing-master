using Nova;
using System.Collections;
using System.Collections.Generic;
using UI.UI_Input;
using UnityEngine;

public class PurchaseCarButton : UIBlockTouchInputComponent
{
    [SerializeField]
    private CarSelectionItems _selectionItems;
    protected override void HandleOnDrag(Gesture.OnDrag evt)
    {
        
    }
    protected override void HandleOnPress(Gesture.OnClick evt)
    {
        _selectionItems.PurchaseLastItemClicked();
    }
    protected override void HandleOnRelease(Gesture.OnRelease evt)
    {
    }
}
