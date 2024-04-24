using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    public List<ObstacleData> Obstacles;
    public Transform CarParent;
    public PathCreator PathCreator;
    public List<GameObject> ObstacleObjects = new();
    [SerializeField] public Transform ObstacleHolder;
    [SerializeField] public GameManager GameManager;

    private void Start()
    {
        var car = Instantiate(PersistantPlayerDataBase.Instance.CurrentCar.CarDataStatics.CarPrefab, Vector3.zero, Quaternion.identity, CarParent);
        car.GetComponent<RCCP_CustomizationApplier>().WheelManager.UpdateWheel(PersistantPlayerDataBase.Instance.CurrentWheelIndex);
        var componentInChildren = car.GetComponentInChildren<MeshCollider>();
        if (componentInChildren)
        {
            Destroy(componentInChildren);
        }
        var remover = car.gameObject.AddComponent<RemoveRCCP>();
        remover.NewTransform = CarParent;
        remover.Remove();
        var halfRampContainer = GameObject.Find("All Half Ramp");
        if (halfRampContainer)
        {
            Destroy(halfRampContainer);
        }
    }
}

[Serializable]
public class ObstacleData
{
    public GameObject Obstacle;
    public Vector2 XRange;
    public Vector2 YRange;
}