using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{

    public event EventHandler OnStateChanged;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public static KitchenGameManager Instance { get; private set; }


    private enum State
    {
        WaitingToStart,
        CoutdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;

    private float coutdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax;

    private bool isGamePaused = false;


    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        state = State.WaitingToStart;
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.CoutdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);

            GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.CoutdownToStart:
                coutdownToStartTimer -= Time.deltaTime;
                if (coutdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = LevelsManager.GetCurrentLevelSO().levelSettings.levelGameTime;
                    gamePlayingTimerMax = LevelsManager.GetCurrentLevelSO().levelSettings.levelGameTime;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public bool IsCoutdownToStartActive()
    {
        return state == State.CoutdownToStart;
    }

    public float GetCoutdownToStartTimer()
    {
        return coutdownToStartTimer;
    }

    public float GetPlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;

            OnGameUnpaused?.Invoke(this, EventArgs.Empty);

        }
    }

    public void EndGame()
    {
        state = State.GameOver;
        OnStateChanged?.Invoke(this,EventArgs.Empty);
    }
}
