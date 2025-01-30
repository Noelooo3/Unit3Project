using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera fpsCamera;
    
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private float playerRotationSpeed;
    [SerializeField] private float playerJumpVelocity;
    [SerializeField] private float sprintMultiplier;

    private float _cameraXRotation;
    private float _currentVerticalVelocity = -2f;

    private Vector2 _movementInput;
    private bool _isJumpPressed;
    private bool _isSprintPressed;
    
    private const float GRAVITY = -9.81f;

    private void Start()
    {
        // Maybe this should go somewhere else!
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
    }

    private void OnMove(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        _isJumpPressed = value.isPressed;
    }

    private void OnSprint(InputValue value)
    {
        _isSprintPressed = value.isPressed;
    }
    
    private void Move()
    {
        float frontAndBack = _movementInput.y;
        float leftAndRight = _movementInput.x;
        bool isGrounded = characterController.isGrounded;

        Vector3 forwardMovement = frontAndBack * transform.forward * playerMoveSpeed * Time.deltaTime;
        Vector3 rightMovement = leftAndRight * transform.right * playerMoveSpeed * Time.deltaTime;
        // at this point no up and down
        Vector3 fullMovement = forwardMovement + rightMovement;
        
        if (_isSprintPressed)
        {
            fullMovement *= sprintMultiplier;
        }
        
        if (isGrounded)
        {
            if (_isJumpPressed)
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
            _currentVerticalVelocity += GRAVITY * Time.deltaTime;
        }

        Vector3 upMovement = _currentVerticalVelocity * transform.up * Time.deltaTime;
        fullMovement += upMovement;
        
        characterController.Move(fullMovement);
    }

    private void OnLook(InputValue value)
    {
        Vector2 mouseValue = value.Get<Vector2>();
        
        float mouseHorizontal = mouseValue.x;
        transform.Rotate(Vector3.up * mouseHorizontal * playerRotationSpeed * Time.deltaTime);
        
        // up and down is on the camera
        float mouseVertical = mouseValue.y;
        _cameraXRotation += (-mouseVertical * playerRotationSpeed * Time.deltaTime);
        
        // make sure the limit is -80 degree to 80 degree
        _cameraXRotation = Mathf.Clamp(_cameraXRotation, -80f, 80f);
        
        fpsCamera.transform.localRotation = Quaternion.Euler(_cameraXRotation, 0, 0);
    }
}
