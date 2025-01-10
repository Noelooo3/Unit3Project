using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject);
        if (other.gameObject.layer != 3)
        {
            Destroy(other.gameObject);
        }
    }
}
