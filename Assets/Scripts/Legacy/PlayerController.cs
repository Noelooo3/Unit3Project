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
        _cameraXRotation = 0f;
    }
}
