using Cars;
using Game_Manager.Mediator;
using Map_Generation;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;
[Serializable]
public class PlayerData
{
    [SerializeReference]
    public CarData[] CarData;
    [SerializeReference]
    public WheelData[] WheelData;
    [SerializeReference]
    public PaintData[] PaintData;
    public float Money;
    public int CurrentCarIndex;
    public int CurrentWheelIndex;
    public int CurrentPaintIndex;
    public int Level = 1;
}
public class PersistantPlayerDataBase : Singleton<PersistantPlayerDataBase>, IStartable
{
    [SerializeField]
    private PlayerData _data;
    private PaintData[] _paintData;
    public TerrainPropsDatabaseSO[] Themes;
    private float _winMoney;
    private float _bonusMoney;
    [SerializeField] 
    CarDatabaseSO _carDatabaseSO;
    [SerializeField] 
    WheelDatabaseSO _wheelDatabaseSO;
    [SerializeField]
    private PaintDatabaseSO _paintDatabaseSO;
    public event Action OnMoneyChange;
    public CarData this[int index]
    {
        get { return _data.CarData[index]; }
    }
    public CarData[] Cars => _data.CarData;
    public WheelData[] Wheels => _data.WheelData;
    public PaintData[] PaintData => _data.PaintData;
    
    public float Money => _data.Money;
    private int _currentPaintIndex = 0;
    private int _validCurrentCarIndex = 0;
    private int _validCurrentWheelIndex = 0;
    private int _validCurrentPaintIndex = 0;
    private float _currentTransactionMoney;
    public int CurrentCarIndex
    {
        get
        {
            return _data.CurrentCarIndex;
        }
        set
        {
            {
                _data.CurrentCarIndex = value;
                if (!CurrentCar.IsLocked)
                {
                    _validCurrentCarIndex = value;
                }
                OnCarChange?.Invoke();
            }
        }
    }
    public int CurrentWheelIndex
    {
        get
        {
            return _data.CurrentWheelIndex;
        }
        set
        {
            _data.CurrentWheelIndex = value;
            if (!CurentWheel.IsLocked)
            {
                _validCurrentWheelIndex = value;
            }
            OnWheelChange?.Invoke();
        }
    }
    public int CurrentPaintIndex
    {
        get
        {
            return _data.CurrentPaintIndex;
        }
        set
        {
            _data.CurrentPaintIndex = value;
            if (!CurrentPaint.IsLocked)
            {
                _validCurrentPaintIndex = value;
            }
            OnPaintChange?.Invoke();
        }
    }

    public bool TransactionPurchase(float amount)
    {
        _currentTransactionMoney = amount;
        return amount <= _data.Money;
    }
    public void CommitTransaction()
    {
        _data.Money -= _currentTransactionMoney;
        OnMoneyChange?.Invoke();
    }
    public void RollbackCosmetics()
    {
        CurrentWheelIndex = _validCurrentWheelIndex;
        CurrentCarIndex = _validCurrentCarIndex;
        CurrentPaintIndex = _validCurrentPaintIndex;
    }
    public event Action OnCarChange;
    public event Action OnWheelChange;
    public event Action OnPaintChange;
    public CarData CurrentCar
    {
        get => _data.CarData[CurrentCarIndex];
    }
    public WheelData CurentWheel
    {
        get => _data.WheelData[CurrentWheelIndex];
    }
    public PaintData CurrentPaint
    {
        get => _data.PaintData[CurrentPaintIndex];
    }
    public float WinMoney => _winMoney;
    public float BonusMoney => _bonusMoney;
    public PlayerData Data => _data;
    protected override void ExtraSingletonLogic()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60; 
        if (LoadData()) 
        {
            return;
        }
        _data = new();
        _carDatabaseSO.Initialize();
        _wheelDatabaseSO.Initialize();
        _paintDatabaseSO.Initialize();
        PopulateInitialData(_carDatabaseSO, _wheelDatabaseSO, _paintDatabaseSO);
        Application.targetFrameRate = 60;
    }
    public void PopulateInitialData(CarDatabaseSO carDatabaseSO, WheelDatabaseSO wheelDatabase, PaintDatabaseSO paintDatabase)
    {
        _data.CarData = new CarData[carDatabaseSO.Objects.Length];
        for (int i = 0; i < carDatabaseSO.Objects.Length; ++i)
        {
            UpgradeableStat topSpeed = new UpgradeableStat(carDatabaseSO.Objects[i].TopSpeed);
            UpgradeableStat handling = new UpgradeableStat(carDatabaseSO.Objects[i].Handling);
            UpgradeableStat acceleration = new UpgradeableStat(carDatabaseSO.Objects[i].Acceleration);
            UpgradeableStat nitro = new UpgradeableStat(carDatabaseSO.Objects[i].Nitro);
            _data.CarData[i] = new CarData(i, carDatabaseSO.Objects[i].Price, carDatabaseSO.Objects[i].LevelRequirement, carDatabaseSO.Objects[i], 0, 0, carDatabaseSO.Objects[i].isLocked, topSpeed, handling, acceleration, nitro);
        }
        _data.WheelData = new WheelData[wheelDatabase.Objects.Length];
        for (int i = 0; i < wheelDatabase.Objects.Length; ++i)
        {
            _data.WheelData[i] = new WheelData(wheelDatabase.Objects[i].ID, wheelDatabase.Objects[i], wheelDatabase.Objects[i].Price, wheelDatabase.Objects[i].IsLocked);
        }
        _data.PaintData = new PaintData[paintDatabase.Objects.Length];
        for (int i = 0; i < paintDatabase.Objects.Length; ++i)
        {
            _data.PaintData[i] = new PaintData(paintDatabase.Objects[i].ID, paintDatabase.Objects[i].Price, paintDatabase.Objects[i].PaintColor, paintDatabase.Objects[i].IsPurchaseable, paintDatabase.Objects[i].IsLocked);
        }
    }
    public void Start()
    {
        ReloadData();
    }
    private void ReloadData()
    {
        OnMoneyChange?.Invoke();
        OnCarChange?.Invoke();
        OnWheelChange?.Invoke();
        OnPaintChange?.Invoke();
        SaveData();
    }
    public void ApplyBonus()
    {
        TransactionPurchase(-(_winMoney + _bonusMoney));
        CommitTransaction();
    }
    public void AddWinMoney(float winMoney, float bonus)
    {
        _winMoney = winMoney;
        _bonusMoney = bonus;
    }
    public void ScaleWinMoney(float scale)
    {
        _winMoney *= scale;
        _bonusMoney *= scale;
    }
    public void SaveData()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        string json = JsonUtility.ToJson(_data);
        File.WriteAllText(path, json);
    }
    public bool LoadData()
    {
        // Path to load from
        string path = Application.persistentDataPath + "/playerData.json";
        // Check if the save file exists
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _data = JsonUtility.FromJson<PlayerData>(json);
            if (CheckDataIsValid(_data))
                return true;
            _data = new();
            return false;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return false;
        }
    }

    private bool CheckDataIsValid(PlayerData data)
    {
        try
        {
            bool check = data.WheelData.Length > 0;
            if (!check) return false;
            check = data.CarData.Length > 0;
            if (!check) return false;
            check = data.PaintData.Length > 0;
            if (!check) return false;
            check = data.CarData[0].CarDataStatics;
            if (!check) return false;
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SaveData();
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
    public TerrainPropsDatabaseSO GetRandomTheme() => Themes[Random.Range(0, Themes.Length)];
    public void IncreaseLevel()
    {
        _data.Level++;
        foreach (var car in _data.CarData)
        {
            if (!car.IsPurchaseable && car.LevelData <= _data.Level)
            {
                car.IsLocked = false;
            }
        }
    }
}