using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;

    private bool _doorLocked = true;

    public void UnlockDoor()
    {
        _doorLocked = false;
    }

    public void LockDoor()
    {
        _doorLocked = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the door is locked, then we end the function here.
        if (_doorLocked)
        {
            return;
        }
        
        //if (other.gameObject.tag == "Player")
        if (other.CompareTag("Player"))
        {
            Debug.Log("Open door");
            doorAnimator.SetBool("Open", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Close door");
            doorAnimator.SetBool("Open", false);
        }
    }
}
