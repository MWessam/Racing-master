using System;
using UnityEngine;
[System.Serializable]
public class WheelData : GridData
{
    [SerializeReference]
    public WheelDataSO WheelStatics;
    public int Price;
    public WheelData(int id, WheelDataSO wheelstatics, int price, bool isLocked = true)
    {
        ID = id;
        IsLocked = isLocked;
        WheelStatics = wheelstatics;
        IsPurchaseable = wheelstatics.IsPurchaseable;
        Price = price;
    }
}
[Serializable]
public class PaintData : GridData
{
    public Color Color;
    public int Price;
    public PaintData(int id, int price, Color color, bool isPurchaseable, bool isLocked = true)
    {
        ID = id;
        Price = price;
        Color = color;
        IsPurchaseable = isPurchaseable;
        IsLocked = isLocked;
    }
}