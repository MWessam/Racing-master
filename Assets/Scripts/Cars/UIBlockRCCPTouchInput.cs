using Nova;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.CurvedWorld;
using Game_Manager.Mediator;
using Map_Generation;
using System;
using UI.UI_Input;
using UnityEngine;

public class UIBlockRCCPTouchInput : UIBlockTouchInputComponent
{
    Transform _currentTransformCached;
    [SerializeField] RCCP_Input _rccpInput;
    private RCCP_CarController _player;
    private Vector3 _startTouchPosition;
    private Player _playerController;
    private float _distanceElapsed = 0;
    private Vector3 _previousPos = Vector3.zero;
    RCCP_Inputs overridedInputs = new RCCP_Inputs();
    public event Action OnFirstMove;
    private bool _hasStarted;
    private float _smoothModifer;
    private Vector3 _playerVelocityCalculation;
    private Quaternion _playerSteeringRotation = Quaternion.identity;
    private bool _isDragging;
    private void FixedUpdate()
    {
        if (_isDragging)
        {
            if (_player.Rigid.velocity.sqrMagnitude >= Mathf.Pow(PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat, 2))
            {
                _player.Rigid.velocity = Vector3.ClampMagnitude(_player.Rigid.velocity, PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat);
                return;
            }
            // _playerVelocityCalculation.y = 0.0f;
            // _player.transform.rotation = _playerSteeringRotation;
            // _player.transform.rotation = _playerSteeringRotation;
        }
    }
    private void Update()
    {
    }
    protected override void HandleOnDrag(Gesture.OnDrag evt)
    {
        _playerVelocityCalculation = _player.transform.forward * PersistantPlayerDataBase.Instance.CurrentCar.Acceleration.CurrentStat * PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat * 3;
        float x = (Input.touches[0].position.x - _startTouchPosition.x);
        float positionX = x / (Screen.width);
        if (Mathf.Abs(x) > 0.05f)
        {
            positionX *=  PersistantPlayerDataBase.Instance.CurrentCar.Handling.CurrentStat * 0.5f;
        }
        else
        {
            positionX = 0;
        }
        _playerSteeringRotation *= Quaternion.Euler(0.0f, positionX * 3.0f, 0.0f);
        Vector3 currentPos = _rccpInput.transform.position;
        _distanceElapsed = currentPos.sqrMagnitude;
        _previousPos = currentPos;
        if (_player.speed >= _player.maximumSpeed)
        {
            overridedInputs.throttleInput = 0.0f;
        }
        else
        {
            overridedInputs.throttleInput = 1.0f;
        }
        overridedInputs.steerInput = positionX;
        _startTouchPosition = Vector3.Lerp(_startTouchPosition, Input.touches[0].position, Time.deltaTime * 10);
        _rccpInput.OverrideInputs(overridedInputs);
    }
    protected override void HandleOnPress(Gesture.OnClick evt)
    {
        _isDragging = true;
        _startTouchPosition = Input.touches[0].position;
        overridedInputs.brakeInput = 0.0f;
        if (_hasStarted)
            return;
        _hasStarted = true;
        OnFirstMove?.Invoke();
    }
    protected override void HandleOnRelease(Gesture.OnRelease evt)
    {
        overridedInputs.throttleInput = 0;
        overridedInputs.brakeInput = 0.1f;
        overridedInputs.steerInput = 0;
        _rccpInput.OverrideInputs(overridedInputs);
        _isDragging = false;
    }
    public override void Start()
    {
        base.Start();
        _smoothModifer = PersistantPlayerDataBase.Instance.CurrentCar.Handling.CurrentStat;
        _rccpInput = RCCP_SceneManager.Instance.activePlayerVehicle.Inputs;
        _player = RCCP_SceneManager.Instance.activePlayerVehicle;
        _playerController = _player.GetComponent<Player>();
        _playerController.OnCarDestroyed += ResetRotation;
    }
    private void ResetRotation()
    {
        _playerSteeringRotation = Quaternion.identity;
    }
    private void OnDrawGizmos()
    {
        // Gizmos.DrawRay(_player.transform.position, _player.transform.position + _player.Rigid.velocity);
    }
}
