using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class Player : MonoBehaviour
{
    private static Player _instance;

    private float _health = 100f;

    public Action<float> OnHealthChangedListener;
    public Action OnDeathListener;
    
    public static Player GetInstance()
    {
        return _instance;
    }
    
    private void Awake()
    {
        // Set up a singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        OnHealthChangedListener?.Invoke(_health);
    }

    public void TakeDamage(float damage)
    {
        if (_health <= 0)
        {
            return;
        }
        _health -= damage;
        
        if (_health <= 0)
        {
            _health = 0f;
            OnDeathListener?.Invoke();
        }
        OnHealthChangedListener?.Invoke(_health);
    }
}
