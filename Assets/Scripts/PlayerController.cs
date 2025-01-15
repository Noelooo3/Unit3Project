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
        Rotate();
        Move();
        MoveUpAndDown();
        Fire();
        InteractWithSelectableObject();
        PickAndDrop();
    }

    private void Move()
    {
        float frontAndBack = Input.GetAxis("Vertical");
        float leftAndRight = Input.GetAxis("Horizontal");

        Vector3 forwardMovement = frontAndBack * transform.forward * playerMoveSpeed * Time.deltaTime;
        Vector3 rightMovement = leftAndRight * transform.right * playerMoveSpeed * Time.deltaTime;
        Vector3 fullMovement = forwardMovement + rightMovement;

        bool isSprinting = Input.GetButton("Sprint");
        if (isSprinting)
        {
            fullMovement *= sprintMultiplier;
        }
        
        characterController.Move(fullMovement);
    }

    private void Rotate()
    {
        // Left and right is on the player
        float mouseHorizontal = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseHorizontal * playerRotationSpeed * Time.deltaTime);
        
        // up and down is on the camera
        float mouseVertical = Input.GetAxis("Mouse Y");
        _cameraXRotation += (-mouseVertical * playerRotationSpeed * Time.deltaTime);
        
        // make sure the limit is -80 degree to 80 degree
        _cameraXRotation = Mathf.Clamp(_cameraXRotation, -80f, 80f);
        
        camera.transform.localRotation = Quaternion.Euler(_cameraXRotation, 0, 0);
    }

    private void MoveUpAndDown()
    {
        if (_isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                _currentVerticalVelocity = playerJumpVelocity;
            }
            else
            {
                // Should be 0, but -2 to avoid floating
                _currentVerticalVelocity = -2f;
            }
        }
        else
        {
            // v = u + a * t
            _currentVerticalVelocity += _gravity * Time.deltaTime;
        }
        
        characterController.Move(new Vector3(0, _currentVerticalVelocity * Time.deltaTime, 0));
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
        // if (_currentPickableObject != null && Input.GetKeyDown(KeyCode.F))
        // {
        //     Debug.Log("Drop object");
        //     _currentPickableObject.Drop();
        //     _currentPickableObject = null;
        //     return;
        // }
        
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maximumInteractableDistance, pickableObjectLayer))
        {
            IPickable pickableObject = hit.transform.GetComponent<IPickable>(); 
            if (pickableObject != null)
            {
                Debug.Log("Get the object");
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
