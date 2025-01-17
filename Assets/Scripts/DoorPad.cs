using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPad : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private Transform _padCheckingPoint;
    [SerializeField] private float _padCheckingRadius;

    private const int KEY_LAYER = 11; 

    private void OnCollisionEnter(Collision other)
    {
        // More optimized way to write this
        // Works but without a limited area
        // if (other.gameObject.layer != KEY_LAYER)
        // {
        //     Debug.Log("GameObject is not key layer");
        //     return;
        // }
        // Key key = other.gameObject.GetComponent<Key>();
        // if (key == null)
        // {
        //     Debug.Log("GameObject is not key");
        //     return;
        // }
        // _door.UnlockDoor();
        
        if (other.gameObject.layer != KEY_LAYER)
        {
            Debug.Log("GameObject is not key layer");
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(_padCheckingPoint.position, _padCheckingRadius);
        //Same as for (int i = 0; i < colliders.Length; i++)
        //But cleaner, and easier to access
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer != KEY_LAYER)
            {
                continue;
            }
            Key key = collider.gameObject.GetComponent<Key>();
            if (key == null)
            {
                //continue: Jump to the next run for this loop
                continue;
            }
            _door.UnlockDoor();
            //break: Kill the loop
            break;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer != KEY_LAYER)
        {
            Debug.Log("GameObject is not key layer");
            return;
        }
        Key key = other.gameObject.GetComponent<Key>();
        if (key == null)
        {
            Debug.Log("GameObject is not key");
            return;
        }
        _door.LockDoor();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_padCheckingPoint.position, _padCheckingRadius);
    }
}
