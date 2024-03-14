using UnityEngine;
using UnityEngine.InputSystem;

public class Slope : MonoBehaviour
{
    public bool IsGrounded = false;
    public bool CanSpeedDown = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedDecreaseValue;
    [SerializeField] private float _boostSpeedStartSlope;
    [SerializeField] private float _maxSpeed;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxTimerJump;

    [SerializeField] private float _gravityMultiplier;

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _toleranceSlopeValue;

    [SerializeField] private bool _modWalk = true;
    [SerializeField] private bool _modOnSlope = false;
    [SerializeField] private bool _modSlide = false;
    [SerializeField] private bool _slideTimer = false;

    [SerializeField] private float _maxTimerSlide;

    private float _gravity = -9.81f;
    private float _currentTimerJump;
    private float _currentTimerSlide;
    private bool _canJump;
    private Vector3 _lastPosition;
    private Vector2 _moveInput;
    private Rigidbody _rigidbody;

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
            //_rigidbody.AddForce(new Vector3(_rigidbody.velocity.x * _boostSpeedStartSlope, 0, _rigidbody.velocity.z * _boostSpeedStartSlope), ForceMode.Impulse);
            _modSlide = true;
            _modWalk = false;
            _slideTimer = false;
            _currentTimerSlide = 0;
        }
        if (context.canceled)
        {
            _modSlide = false;
            _modWalk = true;
            _modOnSlope = false;
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
        if (_modWalk)
        {
            _rigidbody.velocity -= new Vector3(_rigidbody.velocity.x * Time.deltaTime * _speedDecreaseValue * 10, 0, _rigidbody.velocity.z * Time.deltaTime * _speedDecreaseValue * 10);
        }
        if ((_modSlide || _modOnSlope) && IsGrounded)
        {
            _rigidbody.velocity -= _rigidbody.velocity * Time.deltaTime * _speedDecreaseValue;
            _modOnSlope = true;
            _modSlide = false;
        }
    }
    private void CheckLastPosition()
    {
        if (_lastPosition.y - transform.position.y < 0 - _toleranceSlopeValue)
        {
            Debug.Log("Il Monte");
            if (_rigidbody.velocity.magnitude < 10f)
            {
                _rigidbody.AddForce(-_rigidbody.velocity, ForceMode.Force);
            }
        }
        else if (_lastPosition.y - transform.position.y > 0 + _toleranceSlopeValue)
        {
            Debug.Log("Il descend");
        }
        else if (_lastPosition.y == transform.position.y)
        {
            Debug.Log("Il ne change pas de hauteur");
        }
        _lastPosition = transform.position;
    }

    private void ApplyRotationSlope()
    {
        if (_modOnSlope)
        {
            /// faire une soustraction ou addition en fonction de la magnitude actuelle 
            _rigidbody.velocity += new Vector3(_rotationSpeed * _moveInput.x * Time.deltaTime, 0, _rotationSpeed * _moveInput.y * Time.deltaTime);
        }
    }
    private void ApplyMovement()
    {
        if (_modWalk)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * _speed, ForceMode.Force);
        }
        if (_modSlide)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * _speed * 5, ForceMode.Impulse);
            _modOnSlope = true;
            _modSlide = false;
        }
    }
    private void ApplyGravity()
    {
        //_rigidbody.AddForce(Vector3.down * _gravity * Time.deltaTime * _gravityMultiplier, ForceMode.Force);
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
        _lastPosition = transform.position;
    }
    private void Update()
    {
        TimerJump();
        TimerSlide();

        ApplyMovement();
        ApplyRotationSlope();
        ApplyGravity();

        SpeedDown();
        CheckLastPosition();
    }
    private void FixedUpdate()
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
}