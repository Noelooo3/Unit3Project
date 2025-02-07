using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<LevelManager> _levelManagers;

    private int _currentLevelIndex;
    private GameState _currentGameState;
    
    private void Awake()
    {
        ServiceLocator.GetInstance().Register(this);
    }

    private void Start()
    {
        _currentLevelIndex = 0;
        ChangeGameState(GameState.EnterLevel);
    }

    public void ChangeGameState(GameState newGameState)
    {
        _currentGameState = newGameState;

        switch (_currentGameState)
        {
            case GameState.EnterLevel:
                OnEnterLevel();
                break;
            case GameState.InLevel:
                InLevel();
                break;
            case GameState.ExitLevel:
                OnExitLevel();
                break;
            case GameState.GameEnded:
                OnGameEnded();
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
        }
    }

    private void OnEnterLevel()
    {
        LevelManager currentLevel = _levelManagers[_currentLevelIndex];
        currentLevel.EnterLevel();
        ChangeGameState(GameState.InLevel);
    }

    private void InLevel()
    {
        Debug.Log("Level started: " + _currentLevelIndex);
    }

    private void OnExitLevel()
    {
        _currentLevelIndex++;
        if (_currentLevelIndex >= _levelManagers.Count)
        {
            ChangeGameState(GameState.GameEnded);
        }
        else
        {
            ChangeGameState(GameState.EnterLevel);
        }
    }

    private void OnGameEnded()
    {
        Debug.Log("Game Ended!");
    }

    private void OnGameOver()
    {
        Debug.Log("Game Over!");
    }

    public enum GameState
    {
        EnterLevel,
        InLevel,
        ExitLevel,
        GameEnded,
        GameOver,
    }
}
