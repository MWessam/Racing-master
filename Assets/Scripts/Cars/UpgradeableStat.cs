using UnityEngine;

[System.Serializable]
public class UpgradeableStat
{
    public BaseStat BaseStat;
    public int CurrentRank = 1;
    public float CurrentStat => Mathf.Clamp(BaseStat.Base * BaseStat.Upgrades[CurrentRank - 1].Multiplier + BaseStat.Upgrades[CurrentRank-1].Additive, 0, BaseStat.MaxStat);
    public bool Upgrade()
    {
        CurrentRank = CurrentRank < BaseStat.Upgrades.Length ? CurrentRank + 1 : BaseStat.Upgrades.Length - 1;
        return CurrentRank <= BaseStat.Upgrades.Length;
    }
    public UpgradeableStat(BaseStat baseStat, int rank = 1)
    {
        BaseStat = baseStat;
        CurrentRank = rank;
    }
}