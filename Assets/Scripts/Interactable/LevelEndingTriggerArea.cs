using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEndingTriggerArea : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTriggerListeners;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerListeners?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
