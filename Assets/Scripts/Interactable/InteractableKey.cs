using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableKey : MonoBehaviour
{
    [SerializeField] private UnityEvent OnPickedUpListeners;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickedUpListeners?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
