using UnityEngine;
using System.Collections;

public static class GameEventManager
{
    #region Delegates & Events
    public delegate void StartGameAction();
    public static event StartGameAction OnStartGame;

    public delegate void PauseGameAction();
    public static event PauseGameAction OnPauseGame;

    public delegate void ResumeGameAction();
    public static event ResumeGameAction OnResumeGame;

    public delegate void StopGameAction();
    public static event StopGameAction OnStopGame;
    #endregion

    #region Public Functions
    public static void StartGame()
    {
        if(OnStartGame != null)
            OnStartGame();
    }

    public static void PauseGame()
    {
        if(OnPauseGame != null)
            OnPauseGame();
    }

    public static void ResumeGame()
    {
        if(OnResumeGame != null)
            OnResumeGame();
    }

    public static void StopGame()
    {
        if(OnStopGame != null)
            OnStopGame();
    }
    #endregion

}
