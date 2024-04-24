using System.Collections;
using System.Collections.Generic;
using Game_Manager.Mediator;
using UnityEngine;

public class InitializeCar : MonoBehaviour, IAwakeable
{
    [SerializeField] Transform _carTransform;
    [SerializeField]
    private Transform _target;
    public void Awake()
    {
        RCCP_SceneManager.Instance.activePlayerVehicle = Instantiate(PersistantPlayerDataBase.Instance.CurrentCar.CarDataStatics.CarPrefab);
        RCCP_SceneManager.Instance.activePlayerVehicle.name = "Player Car";
        RCCP_SceneManager.Instance.activePlayerVehicle.gameObject.SetActive(true);
        Transform transform1 = RCCP_SceneManager.Instance.activePlayerVehicle.transform;
        transform1.position = _carTransform.position;
        transform1.parent = _carTransform;
        _target.parent = transform1;
        transform1.rotation = Quaternion.identity;
        RCCP_SceneManager.Instance.activePlayerVehicle.GetComponent<RCCP_CustomizationApplier>().WheelManager.UpdateWheel(PersistantPlayerDataBase.Instance.CurrentWheelIndex);
    }
}
