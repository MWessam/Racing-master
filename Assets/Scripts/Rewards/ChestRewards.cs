using Nova;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Rewards
{
    public class ChestVisuals : ItemVisuals
    {
        public UIBlock2D Chest;
        public UIBlock2D Reward;
        public TextBlock MoneyText;
    }
    public class ChestData
    {
        public int Money;
        public int WheelIndex;
        public int PaintIndex;
        public bool IsClicked = false;
    }
    public class ChestRewards : MonoBehaviour
    {
        public ListView ChestList;
        private ChestData[] _dataSource;
        [SerializeField]
        private Sprite _moneySprite;
        [SerializeField]
        private WinMenu _winMenu;
        private int _freeChestCount = 1;
        private int _openedChestCount = 0;
        private void Awake()
        {
            _dataSource = new ChestData[3];
        }
        private void OnEnable()
        {
            ChestList.AddDataBinder<ChestData, ChestVisuals>(BindItem);
            ChestList.AddGestureHandler<Gesture.OnClick, ChestVisuals>(ClickChest);
            ChestList.SetDataSource(_dataSource);
        }
        private void ClickChest(Gesture.OnClick evt, ChestVisuals target, int index)
        {
            var data = _dataSource[index];
            if (data.IsClicked) return;
            if (_openedChestCount >= _freeChestCount)
            {
                if (!WatchAd())
                {
                    return;
                }
                return;
            }
            if (data.Money != -1)
            {
                target.Chest.SetImage(_moneySprite);
                target.MoneyText.Visible = true;
                target.MoneyText.Text = data.Money.ToString();
                PersistantPlayerDataBase.Instance.TransactionPurchase(-data.Money);
                PersistantPlayerDataBase.Instance.CommitTransaction();
            }
            else
            {
                target.MoneyText.Visible = false;
                if (data.WheelIndex != -1)
                {
                    target.Chest.SetImage(PersistantPlayerDataBase.Instance.Wheels[data.WheelIndex].WheelStatics.WheelSprite);
                    PersistantPlayerDataBase.Instance.Wheels[index].IsLocked = false;
                }
                if (data.PaintIndex != -1)
                {
                    target.Chest.Visible = false;
                    target.Reward.Visible = true;
                    target.Reward.Color = PersistantPlayerDataBase.Instance.PaintData[data.PaintIndex].Color;
                    PersistantPlayerDataBase.Instance.PaintData[index].IsLocked = false;
                }
            }
            data.IsClicked = true;
            ++_openedChestCount;

        }
        private bool WatchAd()
        {
            return true;
        }

        private void BindItem(Data.OnBind<ChestData> evt, ChestVisuals target, int index)
        {
            target.Chest.Visible = true;
            target.Reward.Visible = false;
            float moneyChance = 0.7f;
            float wheelChance = 0.2f;
            float paintChance = 0.1f;
            float totalWeight = moneyChance + wheelChance + paintChance;
            float randomValue = Random.Range(0f, totalWeight);
            if (randomValue < moneyChance)
            {
                ChestData chestData = new ChestData();
                GiveRandomMoney(chestData);
                chestData.PaintIndex = -1;
                chestData.WheelIndex = -1;
                _dataSource[index] = chestData;
            }
            else if (randomValue < moneyChance + wheelChance)
            {
                ChestData chestData = new ChestData();
                chestData.Money = -1;

                if (PersistantPlayerDataBase.Instance.Wheels.Any(x => x.IsLocked))
                {
                    var wheel = PersistantPlayerDataBase.Instance.Wheels.First(x => x.IsLocked);
                    int chestDataWheelIndex = Array.IndexOf(PersistantPlayerDataBase.Instance.Wheels, wheel);
                    if (_dataSource.All(x =>
                        {
                            if (x is null) return false;
                            return x.WheelIndex != chestDataWheelIndex;
                        }))
                    {
                        chestData.WheelIndex = chestDataWheelIndex;
                    }
                    else
                    {
                        GiveRandomMoney(chestData);
                    }
                }
                else
                {
                    GiveRandomMoney(chestData);
                }

                chestData.PaintIndex = -1;
                _dataSource[index] = chestData;
            }
            else
            {
                ChestData chestData = new ChestData();
                chestData.Money = -1;
                if (PersistantPlayerDataBase.Instance.PaintData.Any(x =>
                    {
                        if (x is null) return false;
                        return x.IsLocked;
                    }))
                {
                    var paint = PersistantPlayerDataBase.Instance.PaintData.First(x => x.IsLocked);
                    int chestDataPaintIndex = Array.IndexOf(PersistantPlayerDataBase.Instance.PaintData, paint);
                    if (_dataSource.All(x =>
                        {
                            if (x is null) return false;
                            return x.PaintIndex != chestDataPaintIndex;
                        }))
                    {
                        chestData.PaintIndex = chestDataPaintIndex;
                    }
                    else
                    {
                        GiveRandomMoney(chestData);
                    }
                }
                else
                {
                    GiveRandomMoney(chestData);
                }

                chestData.WheelIndex = -1;
                _dataSource[index] = chestData;
            }
        }

        public void Continue()
        {
            _winMenu.RenderWinMenu();
        }
        private void GiveRandomMoney(ChestData chestData)
        {
            chestData.Money = Random.Range(20, 50);
        }
    }
}
