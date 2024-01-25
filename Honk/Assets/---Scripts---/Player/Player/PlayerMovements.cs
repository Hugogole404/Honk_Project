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
    [SerializeField] private float _maxSpeed, _baseSpeed;
    [SerializeField] private float _actualSpeed;

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

    [Header("Jump")]
    [SerializeField] private float _jumpPower;

    [Header("TimerCoyauteJump")]
    [SerializeField] private float _maxTimer;
    private float _currentTimer;
    private bool _canJump;

    [Header("States")]
    private bool _isWaking;
    private bool _isSliding;
    private bool _isSwiming;
    #endregion

    private void IncreaseTimer()
    {
        _currentTimer += Time.deltaTime;
    }
    private void ResetTimer()
    {
        _currentTimer = 0;
    }
    private void ResetJumpCounter()
    {
        _canJump = true;
    }
    private void CheckIsGroundedCoyauteJump()
    {
        if (!IsGrounded())
        {
            IncreaseTimer();
            //_canJump = false;
            if (/*_canJump && */_currentTimer > _maxTimer)
            {
                _canJump = false;
            }
        }
        if (IsGrounded())
        {
            ResetTimer();
            ResetJumpCounter();
        }
    }
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
        if (_actualSpeed > _baseSpeed && _canSpeedDecrease)
        {
            _actualSpeed -= _speedDecrease * Time.deltaTime;
        }
        if (_actualSpeed < _baseSpeed)
        {
            _actualSpeed = _baseSpeed;
        }
    }
    private void TeleportToSpawnPoint()
    {
        transform.position = _spawnPoint.position;
    }
    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0f)
        {
            _velocity = -1f;
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
    private bool IsGrounded() => _characterController.isGrounded;
    public void Move(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        _canSpeedAugment = true;
        _canSpeedDecrease = false;
        //}
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, _direction.y, _input.y);
        if (context.canceled)
        {
            _canSpeedAugment = false;
            _canSpeedDecrease = true;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (!IsGrounded() || _currentTimer >= _maxTimer)
        {
            _canJump = false;
            return;
        }
        if (_canJump)
        {
            _velocity += _jumpPower;
            _canJump = false;
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
        _actualSpeed = _baseSpeed;
    }
    private void FixedUpdate()
    {
        CheckIsGroundedCoyauteJump();
        ApplyMovement();
        ApplyGravity();
        ApplySpeed();
    }
    private void Update()
    {
        ApplyRotation();
    }
}