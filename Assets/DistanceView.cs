using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceView : MonoBehaviour
{
    public Slider DistanceSlider;

    public void UpdateDistance(float distance, float maxDistance)
    {
        DistanceSlider.Max = maxDistance;
        DistanceSlider.Value = distance;
    }
}
