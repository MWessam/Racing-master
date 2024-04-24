using Nova;
using UI.Loading_Screen;

public class UIBlockWinBackToSceneTouchComponent : UIBlockLoadSceneTouchComponent
{
    protected override void HandleOnRelease(Gesture.OnRelease evt)
    {
        PersistantPlayerDataBase.Instance.ApplyBonus();
        if (_loadingMenu == null)
        {
            _loadingMenu = FindObjectOfType<LoadingMenu>(true);
        }

        var actualLevelIndex = LevelLoader.Instance.Actual_Level_Index();
        _loadingMenu.SceneName = 0;
        _loadingMenu.InitializeLoadMenu();
        LoadingSceneManager.Instance.LoadScene(_loadingMenu);
    }
}
