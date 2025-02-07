using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator GetInstance() => _instance;
    private static ServiceLocator _instance;

    private GameManager _gameManager;
    private Player _player;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void RegisterGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void RegisterPlayer(Player player)
    {
        _player = player;
    }

    public GameManager GetGameManager()
    {
        return _gameManager;
    }

    public Player GetPlayer()
    {
        return _player;
    }
}
