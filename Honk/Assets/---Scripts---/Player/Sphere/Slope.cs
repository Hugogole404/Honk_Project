using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slope : MonoBehaviour
{
    public bool IsGrounded = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxTimerJump;
    private float _currentTimerJump;
    private bool _canJump;
    private Vector2 _moveInput;
    private Rigidbody _rigidbody;
    private bool _modWalk;
    private bool _modSlide;


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        if (_modWalk)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * _speed, ForceMode.Force);
        }
        if (_modSlide)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * _speed * 5f, ForceMode.Force);
        }
    }
    public void StartSlide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
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