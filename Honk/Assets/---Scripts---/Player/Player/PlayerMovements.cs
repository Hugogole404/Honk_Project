using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[HelpURL("https://app.milanote.com/1RscWs1SJGPM9j/playermovements?p=5Aw4gcZ0pqp")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovements : MonoBehaviour
{
    #region Variables
    [Header("SpawnPoint")]
    [SerializeField] private Transform _spawnPoint;

    [Header("Movements")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _actualSpeed = 10;

    [SerializeField] private float _speedAugmentation;
    [SerializeField] private float _speedDecrease;

    private Vector2 _input;
    private Vector3 _direction;

    [SerializeField] private float _smoothTime;
    [SerializeField] private float _currentVelocity;
    private CharacterController _characterController;

    private bool _canSpeedAugment = false, _canSpeedDecrease = true;

    [Header("Gravity")]
    [SerializeField] private float _gravityMultiplier;
    private float _gravity = -9.81f;
    private float _velocity;
    #endregion

    private void AugmentSpeedToMaxSpeed()
    {
        if (_canSpeedAugment)
        {
            if (_actualSpeed < _maxSpeed)
            {
                _actualSpeed += _speedAugmentation * Time.deltaTime;
            }
            if (_actualSpeed > _maxSpeed)
            {
                _actualSpeed = _maxSpeed;
            }
        }
    }
    private void DecreaseSpeed()
    {
        if (_currentVelocity > 0 && _canSpeedDecrease)
        {
            _actualSpeed -= _speedDecrease * Time.deltaTime;
        }
    }
    private void TeleportToSpawnPoint()
    {
        transform.position = _spawnPoint.position;
    }
    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity < 0f)
        {
            _velocity = 0f;
        }
        else
        {
            _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }
        _direction.y = _velocity;
    }
    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0)
        {
            return;
        }
        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, _smoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    private void ApplyMovement()
    {
        _characterController.Move(_direction * _actualSpeed * Time.deltaTime);
    }
    private void ApplySpeed()
    {
        AugmentSpeedToMaxSpeed();
        DecreaseSpeed();
    }
    public void Move(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _canSpeedAugment = true;
            _canSpeedDecrease = false;
        }
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0f, _input.y);
        if (context.canceled)
        {
            _canSpeedAugment = false;
            _canSpeedDecrease = true;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump");
        }
    }
    public void HonkNoise(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("HonkNoise");
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        TeleportToSpawnPoint();
    }
    private void Update()
    {
        ApplyRotation();
        ApplyMovement();
        ApplyGravity();
        ApplySpeed();
    }
}