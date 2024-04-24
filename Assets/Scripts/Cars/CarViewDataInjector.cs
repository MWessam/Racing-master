using Game_Manager.Mediator;
using UnityEngine;
using UnityEngine.Serialization;

public class CarViewDataInjector : MonoBehaviour, IStartable, IOnDisabled, IAwakeable
{
    [SerializeField] CarViewer _carViewer;
    [FormerlySerializedAs("_persistantCarDataBase")]
    [SerializeField] PersistantPlayerDataBase persistantPlayerDataBase;

    public void Start()
    {
        persistantPlayerDataBase = PersistantPlayerDataBase.Instance;
        persistantPlayerDataBase.OnCarChange += ChangePlayerPreview;
        persistantPlayerDataBase.OnWheelChange += ChangePlayerPreview;
        persistantPlayerDataBase.OnPaintChange += ChangePlayerPreview;
        ChangePlayerPreview();
    }
    public void ChangePlayerPreview()
    {
        _carViewer.UpdateCarView(persistantPlayerDataBase.CurrentCar);
    }

    public void OnDisable()
    {
        persistantPlayerDataBase.OnCarChange -= ChangePlayerPreview;
        persistantPlayerDataBase.OnWheelChange -= ChangePlayerPreview;
        persistantPlayerDataBase.OnPaintChange -= ChangePlayerPreview;
    }
    public void Awake()
    {
    }
}