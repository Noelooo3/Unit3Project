using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera camera;
    
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private Rigidbody RocketPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform pickUpPoint;
    
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float playerRotationSpeed;
    [SerializeField] private float playerJumpVelocity;
    
    [SerializeField] private float bulletInitialVelocity;
    [SerializeField] private float rocketInitialVelocity;

    [SerializeField] private float maximumInteractableDistance;
    [SerializeField] private LayerMask selectableObjectLayer;
    [SerializeField] private LayerMask pickableObjectLayer;
    
    private float _cameraXRotation;
    
    private float _currentVerticalVelocity = -2f;
    private float _gravity = -9.81f;
    
    private bool _isGrounded;

    private ISelectable _currentSelectableObject;
    private IPickable _currentPickableObject;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cameraXRotation = 0f;
    }

    private void Update()
    {
        _isGrounded = characterController.isGrounded;
        Fire();
        InteractWithSelectableObject();
        PickAndDrop();
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
        Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.velocity = camera.transform.forward * bulletInitialVelocity;
    }

    private void FireRocket()
    {
        Rigidbody rocket = Instantiate(RocketPrefab, shootPoint.position, Quaternion.identity);
        rocket.velocity = camera.transform.forward * rocketInitialVelocity;
    }

    private void InteractWithSelectableObject()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //Second way to make the Ray:
        //Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maximumInteractableDistance, selectableObjectLayer))
        {
            // Ray hits something
            _currentSelectableObject = hit.transform.GetComponent<ISelectable>();
            if (_currentSelectableObject != null)
            {
                _currentSelectableObject.OnHoverEnter();
                if (Input.GetKeyDown(KeyCode.E))
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
    }

    private void PickAndDrop()
    {
        if (_currentPickableObject != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Drop object");
                _currentPickableObject.Drop();
                _currentPickableObject = null;
            }
            return;
        }
        
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maximumInteractableDistance, pickableObjectLayer))
        {
            IPickable pickableObject = hit.transform.GetComponent<IPickable>(); 
            if (pickableObject != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
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
        Vector3 startingPoint = camera.transform.position;
        Vector3 endPoint = camera.transform.position + camera.transform.forward * maximumInteractableDistance;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startingPoint, endPoint);
    }
}
