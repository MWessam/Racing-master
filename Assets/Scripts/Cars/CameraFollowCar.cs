using System.Collections;
using System.Collections.Generic;
using Game_Manager.Mediator;
using UnityEngine;

public class CameraFollowCar : MonoBehaviour, IStartable, IUpdateable
{
    [SerializeField]
    private RCCP_Camera _camera;
    public void Start()
    {
        _camera.cameraTarget.playerVehicle = RCCP_SceneManager.Instance.activePlayerVehicle;
    }

    public void Update()
    {
    }
}
