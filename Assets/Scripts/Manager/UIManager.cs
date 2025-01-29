using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject aimingPoint;

    private void Awake()
    {
        Player.GetInstance().OnHealthChangedListener += OnHealthChanged;
        Player.GetInstance().OnDeathListener += OnGameOver;
    }
    
    private void OnHealthChanged(float health)
    {
        healthText.text = health.ToString("0");
    }

    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        aimingPoint.SetActive(false);
    }

    private void OnDestroy()
    {
        Player.GetInstance().OnHealthChangedListener -= OnHealthChanged;
        Player.GetInstance().OnDeathListener -= OnGameOver;
    }
}
