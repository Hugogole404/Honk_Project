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
    [SerializeField] private Vector3 _direction;

    [SerializeField] private bool _modWalk = true;
    [SerializeField] private bool _modOnSlope = false;
    [SerializeField] private bool _modSlide = false;

    private Vector3 _lastPosition;
    private float _gravity = -9.81f;
    private float _currentTimerJump;
    private bool _canJump;
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
        if (context.started)
        {
            //_rigidbody.AddForce(new Vector3(_rigidbody.velocity.x * _boostSpeedStartSlope, 0, _rigidbody.velocity.z * _boostSpeedStartSlope), ForceMode.Impulse);
            _modSlide = true;
            _modWalk = false;
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

    private async void SpeedDown()
    {
        if (_modWalk)
        {
            _rigidbody.velocity -= new Vector3(_rigidbody.velocity.x * Time.deltaTime * _speedDecreaseValue * 10, 0, _rigidbody.velocity.z * Time.deltaTime * _speedDecreaseValue * 10);
        }
        if (_modSlide)
        {
            _rigidbody.velocity -= _rigidbody.velocity * Time.deltaTime * _speedDecreaseValue;
            _modOnSlope = true;
            _modSlide = false;
        }
    }
    private void CheckLastPosition()
    {
        //if (_lastPosition.y > transform.position.y)
        //{
        //    Debug.Log("Il descend");
        //}
        //else if (_lastPosition.y < transform.position.y)
        //{
        //    Debug.Log("Il monte");
        //}
        if (_lastPosition.y - transform.position.y < 0 - _toleranceSlopeValue)
        {
            Debug.Log("Il Monte");
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Walkable>() != null)
        {
            IsGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Walkable>() != null)
        {
            IsGrounded = false;
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lastPosition = transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_rigidbody.velocity);
        }

        _currentTimerJump += Time.deltaTime;

        if (_currentTimerJump > _maxTimerJump)
        {
            _canJump = true;
        }

        ApplyMovement();
        SpeedDown();
        ApplyRotationSlope();
        CheckLastPosition();
        ApplyGravity();
        Debug.Log(_rigidbody.velocity);
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