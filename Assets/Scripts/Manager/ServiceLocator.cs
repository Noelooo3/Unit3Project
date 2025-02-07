using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator GetInstance() => _instance;
    private static ServiceLocator _instance;

    private Dictionary<Type, object> _services = new Dictionary<Type, object>();
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void Register<T>(T service)
    {
        if (_services.ContainsKey(service.GetType()))
        {
            Debug.LogWarning("Service already registered: " + service.GetType().Name);
            return;
        }
        _services.Add(typeof(T), service);
    }

    public bool Get<T>(out T service)
    {
        if (_services.ContainsKey(typeof(T)))
        {
            service = (T)_services[typeof(T)];
            return true;
        }
        else
        {
            service = default(T);
            return false;
        }
    }
}
