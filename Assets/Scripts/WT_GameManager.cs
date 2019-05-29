using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WT_GameManager : MonoBehaviour
{
    public static WT_GameManager I { get; private set; }

    #region Fields
    [SerializeField] float timer = 0;
    public GameMode CurrentGameMode { get; private set; }
    #endregion


    #region Events
    public event Action<GameMode> OnGameModeChange = null;
    #endregion


    #region Unity methods
    private void Awake()
    {
        I = this;
    }

    private void Update()
    {
        GameManagerBehaviour();
    }
    #endregion

    #region Custom methods
    public void ChangeGameMode(GameMode _mode)
    {
        CurrentGameMode = _mode;
        OnGameModeChange?.Invoke(_mode);
    }

    void GameManagerBehaviour()
    {
        switch (CurrentGameMode)
        {
            case GameMode.Play:
                Timer();
                break;
            case GameMode.Pause:
                break;
            case GameMode.Menu:
                break;
        }
    }

    void Timer()
    {
        timer += Time.deltaTime;
    }
    #endregion
}
