using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using Rewards;
using UI.UI_Input;

public class GameManager : SingletonNonPersistent<GameManager>
{
    public List<GameObject> Paths;
    [HideInInspector]
    public List<Vector3> mainControlPoints;
    public Finish finish;
    public PathCreator path;
    public CameraManagement cameraManagement;
    public AmplifyMotionEffect amplifyMotion;
    public WinMenu WinMenu;
    public UiManager UiManager;

    protected override void ExtraSingletonLogic()
    {
        base.ExtraSingletonLogic();
        Application.targetFrameRate = 60;
        ConstructLine();
        WinMenu = FindObjectOfType<WinMenu>(true);
        UiManager = FindObjectOfType<UiManager>(true);
        
    }

    private void OnEnable()
    {
        RenderSettings.fog = true;
    }

    private void Start()
    {
    }

    public void ConstructLine()
    {
        mainControlPoints.Clear();
        for (int pathIndex = 0; pathIndex < Paths.Count; pathIndex++)
        {
            if (Paths[pathIndex].GetComponent<WayPoints>())
            {
                WayPoints wayPoints = Paths[pathIndex].GetComponent<WayPoints>();
                for (int i = 0; i < wayPoints.waypoints.Count; i++)
                {
                    mainControlPoints.Add(wayPoints.waypoints[i].position);
                }
            }

        }

        for (int i = 0; i < finish.finishLineWayPoints.Length; i++)
        {
            mainControlPoints.Add(finish.finishLineWayPoints[i].position);
        }

        if (mainControlPoints.Count > 0)
        {
            BezierPath bezierPath = new PathCreation.BezierPath(mainControlPoints.ToArray(), false, PathSpace.xyz);
            path.bezierPath = bezierPath;
        }
    }


    public void Win()
    {
        PersistantPlayerDataBase.Instance.AddWinMoney((6 - UiManager.playerDistanceCounter.rank) * 5 + 10, (6 - UiManager.playerDistanceCounter.rank) * 5);
        UIHandler.Instance.TransitionToMenu(WinMenu, MenuTransitionDirection.Bottom, MenuTransitionDirection.Top);
        PersistantPlayerDataBase.Instance.IncreaseLevel();
        UiManager._nitroButton.SetActive(false);
    }

    public void Lose()
    {
        PersistantPlayerDataBase.Instance.AddWinMoney(5, 5);
        UIHandler.Instance.TransitionToMenu(WinMenu, MenuTransitionDirection.Left, MenuTransitionDirection.Right);

    }
}
