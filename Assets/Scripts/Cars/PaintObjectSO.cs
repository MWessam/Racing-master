using UI.Car_Selection;
using UnityEngine;
[CreateAssetMenu(menuName = "Create PaintObjectSO", fileName = "PaintObjectSO", order = 0)]
public class PaintObjectSO : DatabaseObjectSO
{
    public Color PaintColor;
    public int Price;
    public bool IsLocked;
    public bool IsPurchaseable = true;
}
