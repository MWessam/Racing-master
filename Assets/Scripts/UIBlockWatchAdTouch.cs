using Nova;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Loading_Screen;
using UI.UI_Input;
using UnityEngine;

public class UIBlockWatchAdTouch : UIBlockTouchInputComponent
{
    public LoadingMenu _menu;
    [SerializeField]
    private int _sceneName;
    public override void Awake()
    {
        base.Awake();
        if (_menu == null)
        {
            _menu = FindObjectOfType<LoadingMenu>(true);
        }
    }
    protected override void HandleOnDrag(Gesture.OnDrag evt)
    {
        
    }
    protected override void HandleOnPress(Gesture.OnClick evt)
    {
        Task<bool> ad = WatchAd();
        if (ad.Result)
        {
            GiveReward();
        }
    }
    private void GiveReward()
    {
        PersistantPlayerDataBase.Instance.ScaleWinMoney(3.0f);
        PersistantPlayerDataBase.Instance.ApplyBonus();
        GoToMainMenu();
    }
    private void GoToMainMenu()
    {
        _menu.SceneName = _sceneName;
        UIHandler.Instance.TransitionToLoadingMenu(_menu);
    }
    protected override void HandleOnRelease(Gesture.OnRelease evt)
    {
    }

    private async Task<bool> WatchAd()
    {
        return true;
    }
}

