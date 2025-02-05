using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour, ISelectable
{
    public UnityEvent OnHoverEnterListners;
    public UnityEvent OnHoverExitListners;
    public UnityEvent OnInteractListeners;

    [SerializeField] private bool _locked = true;

    public void SetLocked(bool locked)
    {
        _locked = locked;
    }
    
    public void OnHoverEnter()
    {
        if (_locked)
            return;
        OnHoverEnterListners?.Invoke();
    }

    public void OnHoverExit()
    {
        if (_locked)
            return;
        OnHoverExitListners?.Invoke();
    }

    public void Interact()
    {
        if (_locked)
            return;
        OnInteractListeners?.Invoke();
    }
}
