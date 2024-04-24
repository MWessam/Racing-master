using System;
using System.Collections;
using Game_Manager;
using Game_Manager.Mediator;
using UI.Loading_Screen;
using UI.UI_Input;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingSceneManager : Singleton<LoadingSceneManager>, IOnEnabled, IOnDisabled
{
    public static event Action OnLoadedScene;
    [SerializeField] float LoadingTime = 5.0f;
    private bool _isLoading;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }


    public void LoadScene(LoadingMenu loadingMenu)
    {
        if (_isLoading) return;
        StartCoroutine(LoadSceneCoroutine(loadingMenu));
    }
    IEnumerator LoadSceneCoroutine(LoadingMenu loadingMenu)
    {
        _isLoading = true;
        UIHandler.Instance.TransitionToLoadingMenu(loadingMenu);
        yield return new WaitForSeconds(LoadingTime);
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(loadingMenu.SceneName, LoadSceneMode.Single);
        asyncScene.allowSceneActivation = false;
        while (asyncScene.progress < 0.9f)
        {
            yield return null;
        }
        asyncScene.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncScene.isDone);
        OnLoadedScene?.Invoke();
        _isLoading = false;
        yield return null;
    }

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
    }
}