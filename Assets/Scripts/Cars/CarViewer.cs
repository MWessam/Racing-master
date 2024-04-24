using Cars;
using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CarViewer : MonoBehaviour
{
    [SerializeField] Slider _topSpeedFillBar;
    [SerializeField] Slider _handlingFillBar;
    [SerializeField] Slider _accelerationFillBar;
    [SerializeField] Slider _nitroFillBar;
    [SerializeField] TextBlock _carName;
    [SerializeField] TextBlock _topSpeed;
    [SerializeField] TextBlock _handling;
    [SerializeField] TextBlock _acceleration;
    [SerializeField] TextBlock _nitro;
    [SerializeField] Transform _carModel;
    [SerializeField]
    private Material _paintMaterial;
    public void UpdateCarView(CarData data)
    {
        _carName.Text = data.CarDataStatics.CarName;
        ChangeFillBarValue(ref _topSpeedFillBar, data.TopSpeed);
        ChangeFillBarValue(ref _handlingFillBar, data.Handling);
        ChangeFillBarValue(ref _accelerationFillBar, data.Acceleration);
        ChangeFillBarValue(ref _nitroFillBar, data.Nitro);
        _topSpeed.Text = data.TopSpeed.CurrentStat.ToString();
        _handling.Text = data.Handling.CurrentStat.ToString();
        _acceleration.Text = data.Acceleration.CurrentStat.ToString();
        _nitro.Text = data.Nitro.CurrentStat.ToString();
        RCCP_CarController newCar = RCCP.SpawnRCC(data.CarDataStatics.CarPrefab, _carModel.transform.position, Quaternion.identity, false, false, false);
        newCar.Rigid.isKinematic = true;
        newCar.transform.parent = _carModel.parent;
        newCar.KillEngine();
        if (newCar.Audio)
        {
            newCar.Audio.DisableEngineSounds();
            newCar.Audio.gameObject.SetActive(false);
        }
        var rccpCustomizationApplier = newCar.GetComponent<RCCP_CustomizationApplier>();
        RCCP_CustomizationManager.Instance.vehicle = rccpCustomizationApplier;
        rccpCustomizationApplier.WheelManager.UpdateWheel(PersistantPlayerDataBase.Instance.CurrentWheelIndex);
        _paintMaterial.color = PersistantPlayerDataBase.Instance.CurrentPaint.Color;
        Destroy(_carModel.gameObject);
        _carModel = newCar.transform;
    }
    private void ChangeFillBarValue(ref Slider slider, UpgradeableStat stat)
    {
        slider.Max = stat.BaseStat.MaxStat;
        slider.Value = stat.CurrentStat;
    }
}
