using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slope : MonoBehaviour
{
    public GameObject SpawnPoint;
    public bool IsGrounded = false;
    public bool CanSpeedDown = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedSlope;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxSpeedSlope;
    [SerializeField] private float _speedDecreaseValue;
    [SerializeField] private float _speedAugmentationSlopeValue;
    [SerializeField] private float _boostSpeedStartSlope;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxTimerJump;

    [SerializeField] private float _gravityMultiplier;

    [SerializeField] private float _rotationSpeed;

    [SerializeField] private bool _modWalk = true;
    [SerializeField] private bool _modOnSlope = false;
    [SerializeField] private bool _modSlide = false;
    [SerializeField] private bool _slideTimer = false;

    [SerializeField] private float _maxTimerSlide;

    private bool _isMovingDown, _isMovingUp, _isMovingStraight;
    private float _originSpeedSlope;
    private float _gravity = -9.81f;
    private float _currentTimerJump;
    private float _currentTimerSlide;
    private bool _canJump;
    private Vector3 _lastPosition;
    private Vector2 _moveInput;
    private Rigidbody _rigidbody;
    private PlayerMovements _playerMovements;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        //if (context.canceled && _modWalk && IsGrounded)
        //{
        //    _rigidbody.velocity = new Vector3(0,0,0);
        //    _moveInput = new Vector2(0, 0);
        //}
    }
    public void StartSlide(InputAction.CallbackContext context)
    {
        if (context.started && _slideTimer)
        {
            _modSlide = true;
            _modWalk = false;
            _slideTimer = false;
        }
        if (context.canceled)
        {
            _modSlide = false;
            _modWalk = true;
            _modOnSlope = false;
            _currentTimerSlide = 0;
            _speedSlope = _originSpeedSlope;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && _canJump == true && IsGrounded == true)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _canJump = false;
            _currentTimerJump = 0;
        }
    }

    private void SpeedDown()
    {
        if (_modWalk && _rigidbody.velocity.magnitude > 1)
        {
            _rigidbody.velocity -= new Vector3(_rigidbody.velocity.x * Time.deltaTime * _speedDecreaseValue * 10, 0, _rigidbody.velocity.z * Time.deltaTime * _speedDecreaseValue * 10);
        }
        if ((_modSlide || _modOnSlope) && IsGrounded && (_isMovingStraight || _isMovingUp) && _rigidbody.velocity.magnitude > 1)
        {
            _rigidbody.velocity -= _rigidbody.velocity * Time.deltaTime * _speedDecreaseValue;
            _modOnSlope = true;
            _modSlide = false;
        }
    }
    private void CheckLastPosition()
    {
        if (_lastPosition.y - transform.position.y < 0)
        {
            _isMovingUp = true;
            _isMovingDown = false;
            _isMovingStraight = false;
            //Debug.Log("Il Monte");

            //if (_rigidbody.velocity.magnitude < 10f)
            //{
            //    _rigidbody.AddForce(-_rigidbody.velocity, ForceMode.Force);
            //}
        }
        else if (_lastPosition.y - transform.position.y > 0)
        {
            _isMovingUp = false;
            _isMovingDown = true;
            _isMovingStraight = false;
            if (_modOnSlope && IsGrounded)
            {
                _speedSlope += _speedAugmentationSlopeValue * Time.deltaTime;
            }
            //Debug.Log("Il descend");
        }
        else if (_lastPosition.y == transform.position.y)
        {
            _isMovingUp = false;
            _isMovingDown = false;
            _isMovingStraight = true;
            //Debug.Log("Il ne change pas de hauteur");
        }
        _lastPosition = transform.position;
    }
    private void CheckMaxSpeed()
    {
        if (_modWalk)
        {
            if (_rigidbody.velocity.magnitude > _maxSpeed)
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
            }
            if (_rigidbody.velocity.magnitude < -_maxSpeed)
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, -_maxSpeed);
            }
        }
        else
        {
            if (_rigidbody.velocity.magnitude > _maxSpeedSlope)
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeedSlope);
            }
            if (_rigidbody.velocity.magnitude < -_maxSpeedSlope)
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, -_maxSpeedSlope);
            }
        }
    }

    private void ApplyRotationSlope()
    {
        if (_modOnSlope)
        {
            _rigidbody.velocity += new Vector3(_rotationSpeed * _moveInput.x * Time.deltaTime, 0, _rotationSpeed * _moveInput.y * Time.deltaTime);
        }
    }
    private void ApplyMovement()
    {
        if (_modWalk)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * _speed * Time.deltaTime * 100, ForceMode.Force);
        }
        if (_modSlide /*&& _rigidbody.velocity.magnitude > 0.05f*/)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * _speedSlope * Time.deltaTime * 100, ForceMode.Impulse);
            _modOnSlope = true;
            _modSlide = false;
        }
        //if (IsGrounded && _playerMovements.IsOnSlope())
        //{
        //    _rigidbody.AddForce(_moveInput.normalized * _speedSlope, ForceMode.Force);
        //    _modOnSlope = true;
        //    _modSlide = false;
        //}

        // a voir si gardé 
        //if (!IsGrounded)
        //{
        //    transform.position += Vector3.down * 0.1f;
        //}
    }
    private void ApplyGravity()
    {
        _rigidbody.velocity += new Vector3(0, _gravity * Time.deltaTime * _gravityMultiplier, 0);
    }

    private void TimerSlide()
    {
        _currentTimerSlide += Time.deltaTime;

        if (_currentTimerSlide > _maxTimerSlide)
        {
            _slideTimer = true;
        }
    }
    private void TimerJump()
    {
        _currentTimerJump += Time.deltaTime;

        if (_currentTimerJump > _maxTimerJump)
        {
            _canJump = true;
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerMovements = FindAnyObjectByType<PlayerMovements>();
        _lastPosition = transform.position;
        transform.position = SpawnPoint.transform.position;
        _originSpeedSlope = _speedSlope;
    }
    private void Update()
    {
        TimerJump();
        TimerSlide();
    }
    private void FixedUpdate()
    {
        ApplyGravity();
        SpeedDown();
        ApplyRotationSlope();
        ApplyMovement();
        CheckLastPosition();
        CheckMaxSpeed();
    }
}