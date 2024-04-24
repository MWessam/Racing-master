using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    LevelStarted,
    LevelFailed,
    LevelCompleted
}


public class GameCanvasControl : MonoBehaviour
{
    public static GameCanvasControl Instance;

    public GameState gameState;

    public GameplayPanelControl gameplayPanelControl;
    public GameOverPanelControl gameOverPanelControl;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        gameOverPanelControl.ResetPanel();
    }


    public void OnTouched()
    {
        gameplayPanelControl.GameStarted();
    }

    public void OnTouchedWithdraw()
    {

    }

    //private void Start()
    //{
    //    //TODO: test
    //    gameOverPanelControl.EnablePanel(GameState.LevelFailed, 1.5f);
    //}

}
