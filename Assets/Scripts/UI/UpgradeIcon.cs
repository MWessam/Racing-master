using Game_Manager.Mediator;
using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUpgrade
{
    Acceleration,
    Speed,
    Handling,
    Nitro
}
public class UpgradeIcon : MonoBehaviour, IStartable, IOnDestroy
{
    public UIBlock2D UpgradeBarPrefab;
    public Transform UpgradeBarParent;
    public List<UIBlock2D> UpgradeBars;
    public Color UpgradeColor;
    public EUpgrade StatType;
    private UpgradeableStat Stat;
    public CarViewer _carViewer;
    public TextBlock _priceText;
    public float[] UpgradePrices;
    public void Upgrade()
    {
        if (Stat.CurrentRank >= Stat.BaseStat.Upgrades.Length) return;
        if (!PersistantPlayerDataBase.Instance.TransactionPurchase(UpgradePrices[Stat.CurrentRank - 1]))
            return;
        if (Stat.Upgrade())
        {
            PersistantPlayerDataBase.Instance.CommitTransaction();
            RefreshUpgrade();
            _carViewer.UpdateCarView(PersistantPlayerDataBase.Instance.CurrentCar);
        }
    }
    public void Start()
    {
        switch (StatType)
        {
            case EUpgrade.Acceleration:
                Stat = PersistantPlayerDataBase.Instance.CurrentCar.Acceleration;
                break;
            case EUpgrade.Speed:
                Stat = PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed;
                break;
            case EUpgrade.Handling:
                Stat = PersistantPlayerDataBase.Instance.CurrentCar.Handling;
                break;
            case EUpgrade.Nitro:
                Stat = PersistantPlayerDataBase.Instance.CurrentCar.Nitro;
                break;
        }
        RefreshUpgrade();
        PersistantPlayerDataBase.Instance.OnCarChange += RefreshUpgrade;
    }
    private void RefreshUpgrade()
    {
        switch (StatType)
        {
            case EUpgrade.Acceleration:
                Stat = PersistantPlayerDataBase.Instance.CurrentCar.Acceleration;
                break;
            case EUpgrade.Speed:
                Stat = PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed;
                break;
            case EUpgrade.Handling:
                Stat = PersistantPlayerDataBase.Instance.CurrentCar.Handling;
                break;
            case EUpgrade.Nitro:
                Stat = PersistantPlayerDataBase.Instance.CurrentCar.Nitro;
                break;
        }
        // foreach (var upgradeBar in UpgradeBars)
        // {
        //     
        // }
        // UpgradeBars = new UIBlock2D[]
        
        for (int i = 0; i < Stat.CurrentRank - 1; ++i)
        {
            UpgradeBars[i].Color = UpgradeColor;
        }
        for (int i = Stat.CurrentRank - 1; i < UpgradePrices.Length; ++i)
        {
            UpgradeBars[i].Color = Color.black;
        }
        if (Stat.CurrentRank >= Stat.BaseStat.Upgrades.Length)
        {
            _priceText.Text = "Max";
        }
        else
        {
            _priceText.Text = Mathf.FloorToInt(UpgradePrices[Stat.CurrentRank - 1]).ToString();
        }
    }
    public void OnDestroy()
    {
        PersistantPlayerDataBase.Instance.OnCarChange -= RefreshUpgrade;
    }
}