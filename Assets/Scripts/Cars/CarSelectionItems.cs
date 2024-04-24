using Cars;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using Game_Manager.Mediator;
using UI.Car_Selection;
using UnityEngine;
using UnityEngine.Serialization;

enum LastClicked
{
    Wheel,
    Car,
    Paint
}
public class CarSelectionItems : MonoBehaviour, IOnEnabled, IOnDisabled, IStartable
{
    [FormerlySerializedAs("_carDatabase")]
    [SerializeField] PersistantPlayerDataBase playerDatabase;
    [SerializeField] GridView _carGrid;
    [SerializeField] GridView _wheelGrid;
    public Color PurchasedButtonColor;
    public Color CashButtonColor;
    public Color ProgressButtonColor;
    public Color PurchasedButtonBorderColor;
    public Color CashButtonBorderColor;
    public Color ProgressButtonBorderColor;
    private int _currentCarIndex = 0;
    private int _currentWheelIndex;
    private LastClicked _lastClicked;
    [SerializeField]
    private TextBlock _purchaseButtonText;
    [SerializeField] 
    private UIBlock2D _purchaseButton;

    private GridItemVisuals _lastGridItem;
    private int _currentPaintIndex;


    private void HandleClick(Gesture.OnClick evt, CarGridItemVisuals target, int index)
    {
        if (index == _currentCarIndex)
        {
            return;
        }

        if (_currentCarIndex >= 0)
        {
            if (_carGrid.TryGetItemView(_currentCarIndex, out ItemView selectedTab))
            {
                GridItemVisuals selected = selectedTab.Visuals as GridItemVisuals;
                selected.IsSelected = false;
            }
        }

        _currentCarIndex = index;
        playerDatabase.CurrentCarIndex = _currentCarIndex;
        target.IsSelected = true;
        if (playerDatabase.CurrentCar.IsLocked)
        {
            if (playerDatabase.CurrentCar.LevelData > 0)
            {
                _purchaseButtonText.Text = $"Win {playerDatabase.CurrentCar.LevelData - playerDatabase.Data.Level} levels to unlock!";
                _purchaseButton.Color = ProgressButtonColor;
                _purchaseButton.Border.Color = ProgressButtonBorderColor;
            }
            else
            {
                _purchaseButtonText.Text = $"{playerDatabase.CurrentCar.Price}";
                _purchaseButton.Color = CashButtonColor;
                _purchaseButton.Border.Color = CashButtonBorderColor;
            }
        }
        else
        {
            _purchaseButtonText.Text = "Purchased";
            _purchaseButton.Color = PurchasedButtonColor;
            _purchaseButton.Border.Color = PurchasedButtonBorderColor;
        }
        _lastClicked = LastClicked.Car;
        _lastGridItem = target;
    }

    private void ProvideSlice(int sliceIndex, GridView gridView, ref GridSlice gridSlice)
    {
        gridSlice.AutoLayout.AutoSpace = true;
        gridSlice.AutoLayout.Alignment = -1;
        
    }

    private void BindItems(Data.OnBind<CarData> evt, CarGridItemVisuals target, int index)
    {
        target.Bind(evt.UserData);
        UpdateGridItems(_carGrid.GetDataSource<CarData>()[index], _carGrid, index);
    }
    private void BindItems(Data.OnBind<WheelData> evt, WheelGridItemVisuals target, int index)
    {
        target.Bind(evt.UserData);
        UpdateGridItems(_carGrid.GetDataSource<WheelData>()[index], _carGrid, index);
    }
    private void BindItems(Data.OnBind<PaintData> evt, PaintGridItemVisuals target, int index)
    {
        target.Bind(evt.UserData);
        UpdateGridItems(_carGrid.GetDataSource<PaintData>()[index], _wheelGrid, index);
    }
    public void SetDataSource(CarData[] dataSource)
    {
        _wheelGrid.gameObject.SetActive(false);
        _carGrid.gameObject.SetActive(true);
        _carGrid.SetDataSource(dataSource);
    }
    public void SetDataSource(WheelData[] dataSource)
    {
        _carGrid.gameObject.SetActive(false);
        _wheelGrid.gameObject.SetActive(true);
        _wheelGrid.SetDataSource(dataSource);
    }
    private void UpdateGridItems(WheelData dataSource, GridView grid, int currentIndex)
    {

        if (!grid.TryGetItemView(currentIndex, out var item) || item.Visuals is not WheelGridItemVisuals gridVisuals)
            return;
        if (currentIndex == PersistantPlayerDataBase.Instance.CurrentWheelIndex)
        {
            gridVisuals.IsSelected = true;
        }
        else
        {
            gridVisuals.IsSelected = false;
            gridVisuals.IsLocked = dataSource.IsLocked;
        }
    }
    private void UpdateGridItems(CarData dataSource, GridView grid, int currentIndex) 
    {
        if (!grid.TryGetItemView(currentIndex, out var item) || item.Visuals is not CarGridItemVisuals gridVisuals)
            return;
        if (currentIndex == PersistantPlayerDataBase.Instance.CurrentWheelIndex)
        {
            gridVisuals.IsSelected = true;
        }
        else
        {
            gridVisuals.IsSelected = false;
            gridVisuals.IsLocked = dataSource.IsLocked;
        }
    }
    private void UpdateGridItems(PaintData dataSource, GridView grid, int currentIndex) 
    {

        if (!grid.TryGetItemView(currentIndex, out var item) || item.Visuals is not PaintGridItemVisuals gridVisuals)
            return;
        if (currentIndex == PersistantPlayerDataBase.Instance.CurrentWheelIndex)
        {
            gridVisuals.IsSelected = true;
        }
        else
        {
            gridVisuals.IsSelected = false;
            gridVisuals.IsLocked = dataSource.IsLocked;
        }
    }
    public void OnEnable()
    {
        _purchaseButtonText.Text = "Purchased";
        _purchaseButton.Color = Color.gray;
        _currentCarIndex = PersistantPlayerDataBase.Instance.CurrentCarIndex;
        _currentWheelIndex = PersistantPlayerDataBase.Instance.CurrentWheelIndex;
        _carGrid.AddDataBinder<CarData, CarGridItemVisuals>(BindItems);
        _wheelGrid.AddDataBinder<WheelData, WheelGridItemVisuals>(BindItems);
        _carGrid.AddDataBinder<PaintData, PaintGridItemVisuals>(BindItems);
        _carGrid.SetSliceProvider(ProvideSlice);
        _wheelGrid.SetSliceProvider(ProvideSlice);
        _carGrid.AddGestureHandler<Gesture.OnClick, CarGridItemVisuals>(HandleClick);
        _wheelGrid.AddGestureHandler<Gesture.OnClick, WheelGridItemVisuals>(HandleClickWheel);
        _carGrid.AddGestureHandler<Gesture.OnClick, PaintGridItemVisuals>(HandleClickPaint);
        _carGrid.Refresh();
    }
    private void HandleClickWheel(Gesture.OnClick evt, WheelGridItemVisuals target, int index)
    {
        if (index == _currentWheelIndex)
        {
            return;
        }

        if (_currentWheelIndex >= 0)
        {
            if (_wheelGrid.TryGetItemView(_currentWheelIndex, out ItemView selectedWheel))
            {
                GridItemVisuals selected = selectedWheel.Visuals as GridItemVisuals;
                selected.IsSelected = false;
            }
        }
        _currentWheelIndex = index;
        playerDatabase.CurrentWheelIndex = _currentWheelIndex;
        target.IsSelected = true;
        if (playerDatabase.CurentWheel.IsLocked)
        {
            _purchaseButtonText.Text = $"{playerDatabase.CurentWheel.Price}";
            _purchaseButton.Color = Color.green;
        }
        else
        {
            _purchaseButtonText.Text = "Purchased";
            _purchaseButton.Color = Color.gray;
        }
        _lastClicked = LastClicked.Wheel;
        _lastGridItem = target;
    }
    private void HandleClickPaint(Gesture.OnClick evt, PaintGridItemVisuals target, int index)
    {
        if (index == _currentPaintIndex)
        {
            return;
        }

        if (_currentPaintIndex >= 0)
        {
            if (_carGrid.TryGetItemView(_currentWheelIndex, out ItemView selectedWheel))
            {
                GridItemVisuals selected = selectedWheel.Visuals as GridItemVisuals;
                selected.IsSelected = false;
            }
        }
        _currentPaintIndex = index;
        playerDatabase.CurrentPaintIndex = _currentPaintIndex;
        target.IsSelected = true;
        if (playerDatabase.CurrentPaint.IsLocked)
        {
            _purchaseButtonText.Text = $"{playerDatabase.CurrentPaint.Price}";
            _purchaseButton.Color = Color.green;
        }
        else
        {
            _purchaseButtonText.Text = "Purchased";
            _purchaseButton.Color = Color.gray;
        }
        _lastClicked = LastClicked.Paint;
        _lastGridItem = target;
    }

    public void OnDisable()
    {
        PersistantPlayerDataBase.Instance.RollbackCosmetics();
        _carGrid.RemoveDataBinder<CarData, CarGridItemVisuals>(BindItems);
        _wheelGrid.RemoveDataBinder<WheelData, WheelGridItemVisuals>(BindItems);
        _carGrid.RemoveDataBinder<PaintData, PaintGridItemVisuals>(BindItems);
        _carGrid.ClearSliceProvider();
        _wheelGrid.ClearSliceProvider();
        _carGrid.RemoveGestureHandler<Gesture.OnClick, CarGridItemVisuals>(HandleClick);
        _wheelGrid.RemoveGestureHandler<Gesture.OnClick, WheelGridItemVisuals>(HandleClickWheel);
        _carGrid.RemoveGestureHandler<Gesture.OnClick, PaintGridItemVisuals>(HandleClickPaint);

        _currentCarIndex = PersistantPlayerDataBase.Instance.CurrentCarIndex;
        _currentWheelIndex = PersistantPlayerDataBase.Instance.CurrentWheelIndex;
    }
    public void PurchaseLastItemClicked()
    {
        switch (_lastClicked)
        {
            case LastClicked.Car:
                var currCar = PersistantPlayerDataBase.Instance.CurrentCar;
                if (!currCar.IsLocked || !currCar.IsPurchaseable) return;
                if (PersistantPlayerDataBase.Instance.TransactionPurchase(currCar.Price))
                {
                    currCar.IsLocked = false;
                    _lastGridItem.IsLocked = false;
                    PersistantPlayerDataBase.Instance.CurrentCarIndex = _currentCarIndex;
                    PersistantPlayerDataBase.Instance.CommitTransaction();
                    _purchaseButtonText.Text = "Purchased";
                    _purchaseButton.Color = Color.gray;
                }
                break;
            case LastClicked.Wheel:
                var currWheel = PersistantPlayerDataBase.Instance.CurentWheel;
                if (!currWheel.IsLocked || !currWheel.IsPurchaseable) return;
                if (PersistantPlayerDataBase.Instance.TransactionPurchase(currWheel.Price))
                {
                    currWheel.IsLocked = false;
                    _lastGridItem.IsLocked = false;
                    PersistantPlayerDataBase.Instance.CurrentWheelIndex = _currentWheelIndex;
                    PersistantPlayerDataBase.Instance.CommitTransaction();
                    _purchaseButtonText.Text = "Purchased";
                    _purchaseButton.Color = Color.gray;
                }
                break;
            case LastClicked.Paint:
                var currPaint = PersistantPlayerDataBase.Instance.CurrentPaint;
                if (!currPaint.IsLocked || !currPaint.IsPurchaseable) return;
                if (PersistantPlayerDataBase.Instance.TransactionPurchase(currPaint.Price))
                {
                    currPaint.IsLocked = false;
                    _lastGridItem.IsLocked = false;
                    PersistantPlayerDataBase.Instance.CurrentPaintIndex = _currentPaintIndex;
                    PersistantPlayerDataBase.Instance.CommitTransaction();
                    _purchaseButtonText.Text = "Purchased";
                    _purchaseButton.Color = Color.gray;
                }
                break;
            default:
                break;
        }
    }
    public void Start()
    {
        playerDatabase = PersistantPlayerDataBase.Instance;
    }
    public void SetDataSource(PaintData[] dataSource)
    {
        _carGrid.SetDataSource(dataSource);
    }
}
