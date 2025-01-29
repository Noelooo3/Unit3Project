using UnityEngine;

public interface IShootStrategy
{
    Transform ShootPoint { get; set; }
    Transform FpsCamera { get; set; }
    
    void Shoot();
}
