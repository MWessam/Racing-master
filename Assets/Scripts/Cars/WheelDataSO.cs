using UI.Car_Selection;
using UnityEngine;


[CreateAssetMenu(fileName = "Wheel Data", menuName = "Scriptable Objects/ Wheels / Wheel Data")]
public class WheelDataSO : DatabaseObjectSO
{
    public Sprite WheelSprite;
    public GameObject WheelObject;
    public bool IsLocked = true;
    public int Price;
    public bool IsPurchaseable = true;
}