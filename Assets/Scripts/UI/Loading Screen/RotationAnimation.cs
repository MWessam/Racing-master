using Nova;
using UnityEngine;

namespace UI.Loading_Screen
{
    public struct RotationAnimation : Nova.IAnimation
    {
        public UIBlock Block;
        [Tooltip("IN DEGREESSSS! THE ANGLEEEE TOOOO ROTATEEEE TOOOOOO INNNNNN DEGREEEEEEEEEEEEEEEEEEEEESSSSSSSSSSSSSSSSSSSSS!!!!!")]
        public float Angle;
        [Tooltip("WHICH AXIS TO ROTATE UPON? 1 = ROTATE, 0 = DONT, ANY NUMBER IN BETWEEN = FRACTIONAL ROTATION, ANY NUMBER GREATER THAN OR LESS = N FRACTIONAL ROTATIONS")]
        public Vector3 Axis;
        private Transform _transform;
        private Vector3 _baseAngles;
        public void Update(float percentDone)
        {
            if (percentDone <= 0)
            {
                _transform = Block.transform;
                _baseAngles = _transform.eulerAngles;
                if (_baseAngles.z > 180)
                {
                    _baseAngles = new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, (_transform.eulerAngles.z - 360) * Axis.z);
                }
            }
            _transform.eulerAngles = Vector3.Lerp(_baseAngles, Axis * Angle, percentDone);
        }
    }
}