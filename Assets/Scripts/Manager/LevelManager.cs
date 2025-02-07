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
        if (ServiceLocator.GetInstance().Get(out GameManager gameManager))
        {
            gameManager.ChangeGameState(GameManager.GameState.ExitLevel);
        }
    }
}
