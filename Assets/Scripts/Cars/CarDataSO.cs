using System.Collections;
using System.Collections.Generic;
using UI.Car_Selection;
using UnityEngine;

[CreateAssetMenu(fileName = "Car Data", menuName = "Scriptable Objects / Car / Car Data")]
public class CarDataSO : DatabaseObjectSO
{
    public BaseStat TopSpeed;
    public BaseStat Handling;
    public BaseStat Acceleration;
    public BaseStat Nitro;
    public RCCP_CarController CarPrefab;
    public Texture2D CarIcon;
    public string CarName;
    public int Price;
    public bool IsPurchaseable;
    public int LevelRequirement;
    public bool isLocked = true;
    
}