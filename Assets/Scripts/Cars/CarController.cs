using Game_Manager.Mediator;
using Map_Generation;
using Nova;
using System;
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
public class CarController : MonoBehaviour, IAwakeable, IStartable
{
    protected RCCP_CarController _carController;
    public RankingView RankView;
    [SerializeField]
    protected ParticleSystem _destroyParticles;
    private Vector3 _previousValidSpot = Vector3.up;
    protected bool _isDestroyed = false;
    protected int _ranking;
    protected Collider _carCollider;
    private Collider _otherCollider;
    private bool _isRespawnFlag;
    protected bool _immunityFlag;
    protected bool _isInCountdown;
    private WheelCollider[] _wheelColliders;
    public event Action OnCarDestroyed;
    public int Ranking
    {
        get => _ranking;
        set => _ranking = value;
    }
    private void OnCollisionEnter(Collision other)
    {
        _otherCollider = other.collider;
        if (_isDestroyed) return;
        // if (_carController.speed < 20.0f) return;
        if (other.gameObject.TryGetComponent<CarController>(out var car))
        {
            if (Physics.Raycast(transform.position, transform.forward * 2.0f, out var hit))
            {
                if (hit.collider.TryGetComponent<CarController>(out var hitCar))
                {
                    if (car == hitCar) car.DestroyCar();
                }
            }
        }
        if (!other.gameObject.GetComponent<Obstacle>()) return;
        DestroyCar();
    }
    protected virtual void DestroyCar()
    {
        if (_isDestroyed || _immunityFlag) return;
        StartCoroutine(DisableCar());
        OnCarDestroyed?.Invoke();
    }
    private IEnumerator DisableCar()
    {
        _isDestroyed = true;
        _immunityFlag = true;
        _carController.KillEngine();
        yield return new WaitForSeconds(2.0f);
        Respawn();
        yield return new WaitForSeconds(3.0f);
        _immunityFlag = false;
        ChangeLayerOfChildrenRecursive(transform, 8);
    }
    private void Respawn()
    {
        _isRespawnFlag = true;
    }

    private void ChangeLayerOfChildrenRecursive(Transform objectTransform, int layer)
    {
        objectTransform.gameObject.layer = layer;
        if (objectTransform.childCount == 0)
        {
            return;
        }
        foreach (Transform child in objectTransform)
        {
            ChangeLayerOfChildrenRecursive(child, layer);
        }

    }
    private void FixedUpdate()
    {
        if (_isRespawnFlag)
        {
            FixedUpdateRespawn();
            _isRespawnFlag = false;
        }
    }
    private void FixedUpdateRespawn()
    {

        _carController.Rigid.velocity = Vector3.zero;
        _carController.Rigid.angularVelocity = Vector3.zero;
        RCCP.Transport(_carController, _previousValidSpot, Quaternion.identity);
        _carController.StartEngine();
        _isDestroyed = false;
        ChangeLayerOfChildrenRecursive(transform, 11);
        foreach (var wheel in _wheelColliders)
        {
            wheel.gameObject.layer = 9;
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_isDestroyed) return;
        if (other.TryGetComponent<CheckPoint>(out CheckPoint seg))
        {
            _previousValidSpot = seg.transform.position + Vector3.up * 2;
        }
        else if (other.TryGetComponent<Obstacle>(out Obstacle obst))
        {
            DestroyCar();
        }
        else if (other.TryGetComponent<FinishLine>(out FinishLine finishLine))
        {
            FinishedRace();
        }
    }
    protected virtual void FinishedRace()
    {
        _carController.KillEngine();
        _carController.Rigid.velocity = Vector3.zero;
    }

    public virtual void Awake()
    {
        _previousValidSpot = transform.position + Vector3.up;
        _carController = GetComponent<RCCP_CarController>();
    }
    public virtual void Start()
    {
        _wheelColliders = GetComponentsInChildren<WheelCollider>();
        float minYLevel = _wheelColliders.Min(x => x.transform.position.y);
        foreach (var wheel in _wheelColliders)
        {
            wheel.forceAppPointDistance = 0.0f;
            wheel.transform.position = new Vector3(wheel.transform.position.x, minYLevel, wheel.transform.position.z);
        }
        
        _carController.Rigid.centerOfMass = Vector3.zero;
        _carController.AeroDynamics.COM.localPosition = Vector3.zero;
        _carController.AeroDynamics.downForce = 5.0f;
        // _carController.Engine.maximumTorqueAsNM = 1200;
        // _carController.Engine.maxEngineRPM = 10000;
        // _carController.Engine.engineInertia = 0;
        // _carController.Engine.engineFriction = 0;
        //
        // Respawn();
    }
    public virtual void Update()
    {
        if (Vector3.Dot(transform.forward, Map.Instance.LastCheckpoint.position - transform.position) <= 0 && !_immunityFlag)
        {
            DestroyCar();
        }
        // if (_carController.speed < 5.0f && !_isInCountdown && GameManager.Instance.IsGameRunning && !_immunityFlag)
        // {
        //     StartCoroutine(InvalidPositionCoroutine());
        // }
    }
    private IEnumerator InvalidPositionCoroutine()
    {
        _isInCountdown = true;
        yield return new WaitForSeconds(3.0f);
        _isInCountdown = false;
        if (_immunityFlag)
        {
            yield break;
        }
        if (_carController.speed > 2.0f)
        {
            yield break;
        }
        DestroyCar();
    }
}


public class RewardsBlockVisuals : ItemVisuals
{
    [SerializeField]
    private TextBlock _positionReward;
    [SerializeField]
    private TextBlock _bonusReward;
    [SerializeField]
    private TextBlock _totalReward;
    
    [SerializeField]
    private TextBlock _tripleReward;

    public void Bind(float positionReward, float bonusReward)
    {
        _positionReward.Text = positionReward.ToString();
        _bonusReward.Text = bonusReward.ToString();
        _totalReward.Text = (positionReward + bonusReward).ToString();
        _tripleReward.Text = ((positionReward + bonusReward) * 3).ToString();
    }
}

public class LevelVisuals : ItemVisuals
{
    [SerializeField]
    private TextBlock _levelText;
    public void Bind(int level)
    {
        _levelText.Text = $"LEVEL {level}";
    }
}