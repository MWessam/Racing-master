using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedView : MonoBehaviour
{
    public UIBlock2D SpeedFillRadial;
    public TextBlock SpeedUnits;
    private const float MIN_RADIAL_FILL_ANGLE = 120.0F;
    private const float MAX_RADIAL_FILL_ANGLE = 360.0F;
    public void UpdateSpeed(float speed)
    {
        SpeedUnits.Text = ((int)speed).ToString();
        float speedPercentage = speed / PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat;

        // Interpolate between the min and max fill angles based on the speed percentage
        float fillAngle = Mathf.Lerp(MIN_RADIAL_FILL_ANGLE, MAX_RADIAL_FILL_ANGLE, speedPercentage);
        SpeedFillRadial.RadialFill.FillAngle = fillAngle;
    }
}
