using UnityEngine;
namespace Map_Generation
{
    public class CurvePoint
    {
        public Vector3 BendPivot;
        public Vector2 BendStrength;
        public CurvePoint(Vector3 bendPivot, Vector2 bendStrength)
        {
            BendStrength = bendStrength;
            BendPivot = bendPivot;
        }
    }
}