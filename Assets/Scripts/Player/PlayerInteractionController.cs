using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private float maximumInteractableDistance;
    [SerializeField] private LayerMask selectableObjectLayer;
    [SerializeField] private LayerMask pickableObjectLayer;
    
    private ISelectable _currentSelectableObject;
    private IPickable _currentPickableObject;

    private bool _isSelectableInteracting;
    private bool _isPickableInteracting;
    
    private void Update()
    {
        InteractWithObject();
    }

    // This happens at the end of the game logic in a frame
    private void LateUpdate()
    {
        _isPickableInteracting = false;
        _isSelectableInteracting = false;
    }
    
    private void OnSelectableInteract()
    {
        _isSelectableInteracting = true;
    }

    private void OnPickableInteract()
    {
        _isPickableInteracting = true;
    }

    private void InteractWithObject()
    {
        Ray ray = fpsCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // First check on selectable objects
        RaycastHit selectableObjectHit;
        if (Physics.Raycast(ray, out selectableObjectHit, maximumInteractableDistance, selectableObjectLayer))
        {
            // Ray hits something
            _currentSelectableObject = selectableObjectHit.transform.GetComponent<ISelectable>();
            if (_currentSelectableObject != null)
            {
                _currentSelectableObject.OnHoverEnter();
                if (_isSelectableInteracting)
                {
                    _currentSelectableObject.Interact();
                }
            }
        }
        else
        {
            // Ray doesn't hit anything
            if (_currentSelectableObject != null)
            {
                _currentSelectableObject.OnHoverExit();
                _currentSelectableObject = null;
            }
        }
        
        // Check on pickable objects
        if (_currentPickableObject != null)
        {
            if (_isPickableInteracting)
            {
                Debug.Log("Drop object");
                _currentPickableObject.Drop();
                _currentPickableObject = null;
            }
            return;
        }

        RaycastHit pickableObjectHit;
        if (Physics.Raycast(ray, out pickableObjectHit, maximumInteractableDistance, pickableObjectLayer))
        {
            IPickable pickableObject = pickableObjectHit.transform.GetComponent<IPickable>(); 
            if (pickableObject != null)
            {
                if (_isPickableInteracting)
                {
                    Debug.Log("Pick up the object");
                    pickableObject.PickUp(pickUpPoint);
                    _currentPickableObject = pickableObject;
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Vector3 startingPoint = fpsCamera.transform.position;
        Vector3 endPoint = fpsCamera.transform.position + fpsCamera.transform.forward * maximumInteractableDistance;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startingPoint, endPoint);
    }
}
