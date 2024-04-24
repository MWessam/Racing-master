using Cinemachine;
using Game_Manager.Mediator;
using Map_Generation;
using Nova;
using NovaSamples.UIControls;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.ButtonInput;
using UI.Loading_Screen;
using UI.UI_Input;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : CarController, IStartable, IUpdateable
{
    [SerializeField]
    private UIBlock2D _fillBar;
    [SerializeField]
    private TextBlock _text;
    [SerializeField]
    private Slider _speedSlider;

    private LoadingMenu _loadingMenu;
    [SerializeField]
    private Map _map;
    private Transform _car;
    private float _distanceRatio;
    private float _distance;

    [SerializeField]
    private UITransition _transitionToWin;
    public DistanceView Distance;
    public SpeedView Speed;

    public event Action<int> OnRaceFinish;
    private bool _isCreated;
    private int _nitroCharges;
    private Transform _mapLastCheckpoint;
    private RCCP_Camera _camera;
    private float _normalCameraDistance;

    [SerializeField]
    private UIBlockRCCPTouchInput _touchInput;

    public override void Start()
    {
        if (_isCreated)
        {
            base.Start();
            _camera = RCCP_SceneManager.Instance.activePlayerCamera;
            _nitroCharges = 2;
            StartCoroutine(GrantImmunityAtStart());
            _mapLastCheckpoint = _map.LastCheckpoint;
            _touchInput = FindObjectOfType<UIBlockRCCPTouchInput>();
            _carController.Rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _speedSlider.Max = PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat;
            _carController.Engine.maxEngineRPM = PersistantPlayerDataBase.Instance.CurrentCar.Acceleration.CurrentStat * 250;
            _carController.Engine.maximumTorqueAsNM = PersistantPlayerDataBase.Instance.CurrentCar.Acceleration.CurrentStat * 25;
            return;
        }
        _car = _carController.transform;
        Player newPlayer = _carController.AddComponent<Player>();
        newPlayer._isCreated = true;
        newPlayer.AddComponent<Mediator>();
        newPlayer._fillBar = _fillBar;
        newPlayer._text = _text;
        newPlayer._map = _map;
        newPlayer._carController = _carController;
        newPlayer.RankView = RankView;
        newPlayer.Speed = Speed;
        newPlayer.Distance = Distance;
        newPlayer._car = _car;
        newPlayer._speedSlider = _speedSlider;
        _distance = _map.DistanceBetweenCurrent2Segments;
        newPlayer._distance = _distance;
        newPlayer._destroyParticles = _destroyParticles;
        newPlayer._transitionToWin = _transitionToWin;
        _map.CurveController.bendPivotPoint = _car;
        _loadingMenu = FindObjectOfType<LoadingMenu>();
        newPlayer._loadingMenu = _loadingMenu;
        _carCollider = _carController.GetComponentInChildren<Collider>();
        newPlayer._carCollider = _carCollider;
        _carController.overrideMaxSpeed = true;
        GetComponent<Mediator>().IsActive = false;
        Destroy(this);

    }
    private IEnumerator GrantImmunityAtStart()
    {
        _immunityFlag = true;
        yield return new WaitForSeconds(5.0f);
        _immunityFlag = false;
    }
    public override void Update()
    {
        base.Update();
        _carController.OverridenMaxSpeed = PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat;
        _distanceRatio = (_distance - (_car.position - _map.NextObjectPosition).magnitude) / _distance;
        _map.CurveController.bendHorizontalSize = Mathf.Lerp(_map.CurrentCurve.BendStrength.x, _map.GetNextCurve.BendStrength.x, _distanceRatio * _distanceRatio);
        _map.CurveController.bendVerticalSize = Mathf.Lerp(_map.CurrentCurve.BendStrength.y, _map.GetNextCurve.BendStrength.y, _distanceRatio * _distanceRatio);
        if (_carController.speed >= PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat * 0.8f)
        {
            PlaySpeedEffects();
        }
        Distance.UpdateDistance(Vector3.Distance(Vector3.zero, transform.position), Vector3.Distance(Vector3.zero, _mapLastCheckpoint.position));
        Speed.UpdateSpeed(_carController.speed);
    }
    private void PlaySpeedEffects()
    {
        
    }

    public void UseNitros()
    {
        if (_carController.NitroUsed || _nitroCharges <= 0) return;
        StartCoroutine(StartNitro());
        --_nitroCharges;
    }
    private IEnumerator StartNitro()
    {
        _carController.NitroAmount = PersistantPlayerDataBase.Instance.CurrentCar.Nitro.CurrentStat * 3;
        _speedSlider.Max = _carController.NitroAmount + PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat;
        _carController.NitroUsed = true;
        yield return new WaitForSeconds(5.0f);
        _speedSlider.Max = PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat;
        _carController.NitroUsed = false;
    }
    protected override void DestroyCar()
    {
        if (_isDestroyed || _immunityFlag) return;
        Destroy(Instantiate(_destroyParticles, transform.position, Quaternion.identity, transform),2.0f);
        base.DestroyCar();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.TryGetComponent<SegmentConnector>(out SegmentConnector seg))
        {
            ChangeBendPoint(seg);
        }
    }
    public void ChangeBendPoint(SegmentConnector seg)
    {
        _map.OnChunkTriggerHandler(seg.transform.parent.gameObject);
        _distance = _map.DistanceBetweenCurrent2Segments;
        _map.CurveController.bendHorizontalSize = _map.CurrentCurve.BendStrength.x;
        _map.CurveController.bendVerticalSize = _map.CurrentCurve.BendStrength.y;
    }
    protected override void FinishedRace()
    {
        base.FinishedRace();
        _touchInput.gameObject.SetActive(false);
        // GameManager.Instance.FinishRace();
        _transitionToWin.Transition();
        OnRaceFinish?.Invoke(_ranking);
    }
    public override void Awake()
    {
        base.Awake();
        _carController = RCCP_SceneManager.Instance.activePlayerVehicle;
        _carController.maximumSpeed = PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat;
    }


}
