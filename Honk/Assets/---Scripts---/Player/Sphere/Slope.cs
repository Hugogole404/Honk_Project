using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slope : MonoBehaviour
{
    public bool IsGrounded = false;
    public bool CanSpeedDown = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedDownValue;
    [SerializeField] private float _boostSpeedStartSlope;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxTimerJump;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private bool _modWalk = true;
    [SerializeField] private bool _modSlide = false;
    private float _gravity = -9.81f;
    private float _currentTimerJump;
    private bool _canJump;
    private Vector2 _moveInput;
    private Rigidbody _rigidbody;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
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
        //_rigidbody.velocity.sqrMagnitude
    }
    private void ApplyMovement()
    {
        if (_modWalk)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * _speed, ForceMode.Force);
        }
        if (_modSlide)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * _speed, ForceMode.Force);
        }
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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_rigidbody.velocity);
        }
        _currentTimerJump += Time.deltaTime;
        if(_currentTimerJump > _maxTimerJump)
        {
            _canJump = true;
        }
        ApplyMovement();
        _rigidbody.AddForce(Vector3.down * _gravity * Time.deltaTime * _gravityMultiplier);
    }
    private void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
        }
        if (_rigidbody.velocity.magnitude < - _maxSpeed)
        {
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, - _maxSpeed);
        }
    }
}