using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShootBulletStrategy : IShootStrategy
{
    public Transform ShootPoint { get; set; }
    public Transform FpsCamera { get; set; }
    
    private Rigidbody _bulletPrefab;
    private float _bulletInitialVelocity = 20f;
    
    private ObjectPool<Rigidbody> _bulletPool;
    
    public ShootBulletStrategy(Transform shootPoint, Transform fpsCamera, Rigidbody bulletPrefab)
    {
        ShootPoint = shootPoint;
        FpsCamera = fpsCamera;
        
        _bulletPrefab = bulletPrefab;
        _bulletPool = new ObjectPool<Rigidbody>(Create, Get, Release, Destroy, defaultCapacity: 0, maxSize: 5);
    }
    
    public void Shoot()
    {
        _bulletPool.Get();
    }
    
    private Rigidbody Create()
    {
        Debug.Log("Create a new bullet");
        Rigidbody bullet = GameObject.Instantiate(_bulletPrefab, ShootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().OnHitListener = OnBulletHit;
        return bullet;
    }

    private void Get(Rigidbody bullet)
    {
        Debug.Log("Get bullet from the pool");
        bullet.transform.position = ShootPoint.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.gameObject.SetActive(true);
        bullet.isKinematic = false;
        bullet.velocity = FpsCamera.transform.forward * _bulletInitialVelocity;
    }

    private void Release(Rigidbody bullet)
    {
        Debug.Log("Releasing bullet");
        bullet.isKinematic = true;
        bullet.gameObject.SetActive(false);
    }

    private void Destroy(Rigidbody bullet)
    {
        Debug.Log("Destroying bullet");
        GameObject.Destroy(bullet.gameObject);
    }

    private void OnBulletHit(Rigidbody bullet)
    {
        _bulletPool.Release(bullet);
    }
}
