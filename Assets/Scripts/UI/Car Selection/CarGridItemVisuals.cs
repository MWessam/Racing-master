using Cars;
using UnityEditor;
using UnityEngine;
namespace UI.Car_Selection
{
    public class CarGridItemVisuals : GridItemVisuals
    {
        public override void Bind(GridData data)
        {
            if (data is not CarData data2)
                return;
            ContentRoot.gameObject.SetActive(data2.ID >= 0);
            IconBlock.SetImage(data2.CarDataStatics.CarIcon);
            IsLocked = data2.IsLocked;
        }
    }
}