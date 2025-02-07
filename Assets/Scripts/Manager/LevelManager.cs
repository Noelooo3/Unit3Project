using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnLevelEnteredListeners;
    [SerializeField] private UnityEvent OnLevelExitedListeners;

    public void EnterLevel()
    {
        OnLevelEnteredListeners?.Invoke();
    }

    public void ExitLevel()
    {
        OnLevelExitedListeners?.Invoke();
        GameManager.GetInstance().ChangeGameState(GameManager.GameState.ExitLevel);
    }
}
