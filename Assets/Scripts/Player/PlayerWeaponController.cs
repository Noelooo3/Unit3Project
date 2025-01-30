using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private Rigidbody rocketPrefab;
    [SerializeField] private Transform shootPoint;
    
    // Test code:
    [SerializeField] private MeshRenderer playerRenderer;
    [SerializeField] private Material blue;
    [SerializeField] private Material red;

    // Use a list for demo, just because list is visible in the inspector. But it's not ideal.
    public List<Rigidbody> bulletPool;
    // Use the one from Unity
    public ObjectPool<Rigidbody> rocketPool;

    private IShootStrategy _currentShootStrategy;
    private List<IShootStrategy> _currentShootStrategyList;

    private void Awake()
    {
        bulletPool = new List<Rigidbody>();
        _currentShootStrategyList = new List<IShootStrategy>();
        
        IShootStrategy shootRocketStrategy = new ShootRocketStrategy(shootPoint, fpsCamera.transform, rocketPrefab);
        _currentShootStrategyList.Add(shootRocketStrategy);
        IShootStrategy shootBulletStrategy = new ShootBulletStrategy(shootPoint, fpsCamera.transform, bulletPrefab);
        _currentShootStrategyList.Add(shootBulletStrategy);
    }

    private void OnPrimaryWeapon()
    {
        if (_currentShootStrategyList.Count >= 1)
        {
            _currentShootStrategy = _currentShootStrategyList[0];
            playerRenderer.material = red;
        }
    }

    private void OnSecondaryWeapon()
    {
        if (_currentShootStrategyList.Count >= 2)
        {
            _currentShootStrategy = _currentShootStrategyList[1];
            playerRenderer.material = blue;
        }
    }

    public void OnFire()
    {
        if (_currentShootStrategy == null)
        {
            Debug.Log("No shoot strategy");
            return;
        }
        _currentShootStrategy.Shoot();
    }
}
