using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShootRocketStrategy : IShootStrategy
{
    public Transform ShootPoint { get; set; }
    public Transform FpsCamera { get; set; }
    
    // Ideally, the strategy should know where the prefab is!
    private Rigidbody _rocketPrefab;
    // Ideally, the strategy should know the weapon data, instead of the weapon controller.
    private float _rocketInitialVelocity = 50f;
    
    private ObjectPool<Rigidbody> _rocketPool;
    
    public ShootRocketStrategy(Transform shootPoint, Transform fpsCamera, Rigidbody rocketPrefab)
    {
        ShootPoint = shootPoint;
        FpsCamera = fpsCamera;
        
        _rocketPrefab = rocketPrefab;
        
        _rocketPool = new ObjectPool<Rigidbody>(CreateRocket, GetRocket, ReleaseRocket, DestroyRocket, defaultCapacity: 0, maxSize: 5);
    }

    public void Shoot()
    {
        _rocketPool.Get();
    }

    private Rigidbody CreateRocket()
    {
        Debug.Log("Create a new rocket");
        Rigidbody rocket = GameObject.Instantiate(_rocketPrefab, ShootPoint.position, Quaternion.identity);
        rocket.GetComponent<Bullet>().OnHitListener = OnRocketHit;
        return rocket;
    }

    private void GetRocket(Rigidbody rocket)
    {
        Debug.Log("Get rocket from the pool");
        rocket.transform.position = ShootPoint.position;
        rocket.transform.rotation = Quaternion.identity;
        rocket.gameObject.SetActive(true);
        rocket.isKinematic = false;
        rocket.velocity = FpsCamera.transform.forward * _rocketInitialVelocity;
    }

    private void ReleaseRocket(Rigidbody rocket)
    {
        Debug.Log("Releasing rocket");
        rocket.isKinematic = true;
        rocket.gameObject.SetActive(false);
    }

    private void DestroyRocket(Rigidbody rocket)
    {
        Debug.Log("Destroying rocket");
        GameObject.Destroy(rocket.gameObject);
    }

    private void OnRocketHit(Rigidbody rocket)
    {
        _rocketPool.Release(rocket);
    }
}
