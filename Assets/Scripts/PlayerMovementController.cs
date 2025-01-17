using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private const float GRAVITY = -9.81f;

    private void Update()
    {
        Move();
        Rotate();
    }
    
    private void Move()
    {
        float frontAndBack = Input.GetAxis("Vertical");
        float leftAndRight = Input.GetAxis("Horizontal");
        bool isJumping = Input.GetButton("Jump");
        bool isSprinting = Input.GetButton("Sprint");
        bool isGrounded = characterController.isGrounded;

        Vector3 forwardMovement = frontAndBack * transform.forward * playerMoveSpeed * Time.deltaTime;
        Vector3 rightMovement = leftAndRight * transform.right * playerMoveSpeed * Time.deltaTime;
        // at this point no up and down
        Vector3 fullMovement = forwardMovement + rightMovement;
        
        if (isSprinting)
        {
            fullMovement *= sprintMultiplier;
        }
        
        if (isGrounded)
        {
            if (isJumping)
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
        
        fpsCamera.transform.localRotation = Quaternion.Euler(_cameraXRotation, 0, 0);
    }
}
