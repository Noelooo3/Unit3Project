using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private Rigidbody rocketPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletInitialVelocity;
    [SerializeField] private float rocketInitialVelocity;

    // Use a list for demo, just because list is visible in the inspector. But it's not ideal.
    public List<Rigidbody> bulletPool;
    // Use the one from Unity
    public ObjectPool<Rigidbody> rocketPool;

    private void Awake()
    {
        bulletPool = new List<Rigidbody>();
        rocketPool = new ObjectPool<Rigidbody>(CreateRocket, GetRocket, ReturnRocket, DestroyRocket, defaultCapacity: 0, maxSize: 5);
    }

    private void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireBullet();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            FireRocket();
        }
    }
    
    private void FireBullet()
    {
        if (bulletPool.Count > 0)
        {
            // Get the first bullet in the pool
            Rigidbody bullet = bulletPool[0];
            bulletPool.RemoveAt(0);
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.gameObject.SetActive(true);
            bullet.isKinematic = false;
            bullet.velocity = fpsCamera.transform.forward * bulletInitialVelocity;
        }
        else
        {
            Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().OnHitListener = OnBulletHit;
            bullet.velocity = fpsCamera.transform.forward * bulletInitialVelocity;
        }
    }

    private void OnBulletHit(Rigidbody bullet)
    {
        bullet.isKinematic = true;
        bullet.gameObject.SetActive(false);
        bulletPool.Add(bullet);
    }

    private void FireRocket()
    {
        rocketPool.Get();
    }

    private Rigidbody CreateRocket()
    {
        Debug.Log("Create a new rocket");
        Rigidbody rocket = Instantiate(rocketPrefab, shootPoint.position, Quaternion.identity);
        rocket.GetComponent<Bullet>().OnHitListener = OnRocketHit;
        return rocket;
    }

    private void GetRocket(Rigidbody rocket)
    {
        Debug.Log("Get rocket from the pool");
        rocket.transform.position = shootPoint.position;
        rocket.transform.rotation = Quaternion.identity;
        rocket.gameObject.SetActive(true);
        rocket.isKinematic = false;
        rocket.velocity = fpsCamera.transform.forward * rocketInitialVelocity;
    }

    private void ReturnRocket(Rigidbody rocket)
    {
        Debug.Log("Returning rocket");
        rocket.isKinematic = true;
        rocket.gameObject.SetActive(false);
    }

    private void DestroyRocket(Rigidbody rocket)
    {
        Debug.Log("Destroying rocket");
        Destroy(rocket.gameObject);
    }

    private void OnRocketHit(Rigidbody rocket)
    {
        rocketPool.Release(rocket);
    }
}
