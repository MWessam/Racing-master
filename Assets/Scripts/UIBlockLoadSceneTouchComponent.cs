using Nova;
using System.Collections;
using System.Collections.Generic;
using UI.Loading_Screen;
using UI.UI_Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBlockLoadSceneTouchComponent : UIBlockTouchInputComponent
{
    [SerializeField] protected LoadingMenu _loadingMenu;
    [SerializeField]
    protected string _sceneName;
    public override void Awake()
    {
        base.Awake();

    }
    protected override void HandleOnDrag(Gesture.OnDrag evt)
    {
    }

    protected override void HandleOnPress(Gesture.OnClick evt)
    {
    }

    protected override void HandleOnRelease(Gesture.OnRelease evt)
    {
        if (_loadingMenu == null)
        {
            _loadingMenu = FindObjectOfType<LoadingMenu>(true);
        }

        var actualLevelIndex = LevelLoader.Instance.Actual_Level_Index();
        _loadingMenu.SceneName = actualLevelIndex;
        _loadingMenu.InitializeLoadMenu();
        LoadingSceneManager.Instance.LoadScene(_loadingMenu);
    }
}