using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform characterGroundCheckPoint;
    
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float playerRotationSpeed;

    [SerializeField] private float playerJumpVelocity;
    
    private float _cameraXRotation;
    
    private float _currentVerticalVelocity = -2f;
    private float _gravity = -9.81f;
    private float _minimumDistanceToTheGround = 0.1f;
    
    private bool _isGrounded;
    private LayerMask _groundLayer;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cameraXRotation = 0f;
        _groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        Rotate();
        Move();
        MoveUpAndDown();
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
        _isGrounded = Physics.CheckSphere(characterGroundCheckPoint.position, _minimumDistanceToTheGround, _groundLayer);
        
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
}
