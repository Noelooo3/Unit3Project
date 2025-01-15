using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IPickable
{
    [SerializeField] private Rigidbody keyRigidbody;
    
    public void PickUp(Transform attachPoint)
    {
        Debug.Log("Key picked up");
        
        transform.position = attachPoint.position;
        transform.rotation = attachPoint.rotation;
        transform.SetParent(attachPoint);

        // Physics will be off
        keyRigidbody.isKinematic = true;
    }

    public void Drop()
    {
        transform.SetParent(null);
        keyRigidbody.isKinematic = false;
    }
}
