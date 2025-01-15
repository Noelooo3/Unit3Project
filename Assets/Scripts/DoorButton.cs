using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour, ISelectable
{
    [SerializeField] private MeshRenderer buttonMeshRenderer;
    // Yellow material
    [SerializeField] private Material buttonDefaultMaterial;
    // Green material
    [SerializeField] private Material buttonInteractableMaterial;
    [SerializeField] private Door door;

    private bool _doorLocked = true;
    
    public void OnHoverEnter()
    {
        buttonMeshRenderer.material = buttonInteractableMaterial;
    }

    public void OnHoverExit()
    {
        buttonMeshRenderer.material = buttonDefaultMaterial;
    }

    public void Interact()
    {
        if (_doorLocked == true)
        {
            door.UnlockDoor();
            _doorLocked = false;
            Debug.Log("Unlock the door");
        }
        else
        {
            door.LockDoor();
            _doorLocked = true;
            Debug.Log("Lock the door");
        }
    }
}
