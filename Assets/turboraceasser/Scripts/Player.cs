using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace PathCreation.Examples
{

    public class Player : MonoBehaviour
    {
        [SerializeField] private float releaseHardness = 1;
        [SerializeField] private float dragControl = 1;
        [SerializeField] private Transform cameraPlaceHolder;
        [SerializeField] private float leftRightRotation = 9;

        public PathFollower _movements;

        public Transform WheelFrontLeft, WheelFrontRight, WheelBackLeft, WheelBackRight;
        public bool permitControl = true;

        public float finishDeviation = 0f;

        public void RotateWheel()
        {
            Vector3 wheelMovementDirection = Vector3.right * _movements.speed * 20f * Time.deltaTime;
            WheelFrontLeft.Rotate(wheelMovementDirection, Space.Self);
            WheelFrontRight.Rotate(wheelMovementDirection, Space.Self);
            WheelBackLeft.Rotate(wheelMovementDirection, Space.Self);
            WheelBackRight.Rotate(wheelMovementDirection, Space.Self);

        }


        float currentMouseX, LastMouseX, difference;
        [HideInInspector]
        public float targetDeviation = 0;
        public bool touched = false;
        float catchUpSpeed = 0f;

        private void Start()
        {
            _movements.MaxSpeed = PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat;
            _movements.acclearionRate = PersistantPlayerDataBase.Instance.CurrentCar.Acceleration.CurrentStat;
            dragControl = PersistantPlayerDataBase.Instance.CurrentCar.Handling.CurrentStat * 0.3f;
        }

        public void DisallowControl()
        {
            permitControl = false;
            DOVirtual.DelayedCall(.35f, () => permitControl = true);
        }

        void Update()
        {

            catchUpSpeed = Mathf.Clamp(_movements.speed/3f, 10f, 15f);

            if (Input.GetMouseButtonDown(0))
            {
                _movements.isStart = true;
                LastMouseX = Input.mousePosition.x;
                touched = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                touched = false;
            }

            if (!touched)
            {
                _movements.speed = Mathf.Clamp(_movements.speed - (_movements.acclearionRate / 2) * Time.deltaTime, 0,
                    _movements.MaxSpeed);
                difference = 0f;
            }
            if (_movements.isStart && !_movements.isfinish)
            {
                if (touched)
                {
                    currentMouseX = Input.mousePosition.x;
                    difference = currentMouseX - LastMouseX;
                    LastMouseX = currentMouseX;
                    _movements.speed += _movements.acclearionRate * Time.deltaTime;
                    if (_movements.speed > _movements.MaxSpeed) _movements.speed = _movements.MaxSpeed;
                }


                if (permitControl)
                {
                    if (_movements.onPath && touched)
                    {
                        targetRotation = 0f;
                        targetDeviation += difference * dragControl * 0.34f * Time.deltaTime;
                        targetDeviation = Mathf.Clamp(targetDeviation, -.98f, .98f);
                        _movements.deviation = Mathf.Lerp(_movements.deviation, targetDeviation, catchUpSpeed * Time.deltaTime);
                    }
                    else
                    {
                        targetDeviation = Mathf.Lerp(targetDeviation, 0f, Time.deltaTime * releaseHardness);
                        _movements.deviation = targetDeviation;
                    }

                    // if (!_movements.onPath)
                    // {
                    //     FlyRotation(difference);
                    // }
                }

            }

            if(_movements.isfinish)
            {
                targetDeviation = Mathf.Lerp(targetDeviation, finishDeviation, Time.deltaTime * releaseHardness / 2f);
                _movements.deviation = targetDeviation;
            }

        }

        float targetRotation = 0f;

        
        //
        // void FlyRotation(float _mouseX)
        // {
        //     transform.Rotate(Vector3.up, _mouseX * leftRightRotation * Time.deltaTime, Space.Self);
        //
        //     if (!_movements.istransation)
        //     {
        //         if (_mouseX != 0)
        //         {
        //             targetRotation += _mouseX * releaseHardness * Time.deltaTime;
        //
        //         }
        //         else
        //         {
        //             targetRotation = Mathf.Lerp(targetRotation, 0f, releaseHardness * Time.deltaTime);
        //         }
        //
        //         targetRotation = Mathf.Clamp(targetRotation, -30f, 30f);
        //         _movements.Car.localEulerAngles = new Vector3(_movements.Car.localEulerAngles.x, _movements.Car.localEulerAngles.y, -targetRotation);
        //     }
        //
        // }

    }
}