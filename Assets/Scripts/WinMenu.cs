using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class WinMenu : Menu
{
    [SerializeField] ItemView _rewardView;
    [SerializeField] ItemView _levelView;
    [SerializeField]
    private UIBlock _actualWinMenu;
    [SerializeField]
    private UIBlock _chestMenu;
    public override UIMenuAnimation Transition(Vector2 startPositionPercentage, Vector2 endPositionPercentage)
    {
        if (_rewardView.Visuals is RewardsBlockVisuals rewardVisuals)
        {
            rewardVisuals.Bind(PersistantPlayerDataBase.Instance.WinMoney, PersistantPlayerDataBase.Instance.BonusMoney);
        }
        if (_levelView.Visuals is LevelVisuals levelVisuals)
        {
            levelVisuals.Bind(PersistantPlayerDataBase.Instance.Data.Level);
        }
        _actualWinMenu.gameObject.SetActive(false);
        _chestMenu.gameObject.SetActive(true);
        return base.Transition(startPositionPercentage, endPositionPercentage);
    }
    public void RenderWinMenu()
    {
        _actualWinMenu.gameObject.SetActive(true);
        _chestMenu.gameObject.SetActive(false);
    }
    
    public override void Awake()
    {
        base.Awake();
    }
    protected override void MenuInitialize()
    {
        base.MenuInitialize();
        
    }
}
