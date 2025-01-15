using UnityEngine;

public interface IPickable
{
    void PickUp(Transform attachPoint);
    void Drop();
}
