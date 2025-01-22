using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletRigidbody;

    public Action<Rigidbody> OnHitListener;
    
    private void OnCollisionEnter(Collision other)
    {
        OnHitListener?.Invoke(bulletRigidbody);
        
        if (other.gameObject.layer != 3)
        {
            Destroy(other.gameObject);
        }
    }
}
