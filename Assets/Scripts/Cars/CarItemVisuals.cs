using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarItemVisuals : ItemVisuals
{
    [SerializeField] TextBlock _topSpeedText;
    [SerializeField] TextBlock _accelerationText;
    [SerializeField] TextBlock _handlingText;
    [SerializeField] TextBlock _nitroText;
    [SerializeField] TextBlock _carText;
    [SerializeField] GameObject _car;
}
