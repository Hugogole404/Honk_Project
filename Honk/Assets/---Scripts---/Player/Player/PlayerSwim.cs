using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwim : MonoBehaviour
{
    [SerializeField] private GameObject _swimmingSprite;
    [SerializeField] private GameObject _player;

    private RaycastHit _hit;
    private PlayerMovements _playerMovements;

    public void IsSwimming(RaycastHit hit)
    {
        if (!_playerMovements.IsSwimming)
        {
            if (hit.collider.gameObject.tag == "Water")
            {
                _playerMovements.IsSwimming = true;
                _player.SetActive(false);
                _swimmingSprite.SetActive(true);
            }
        }
        else
        {
            if (hit.collider.gameObject.tag != "Water")
            {
                _playerMovements.IsSwimming = false;
                _player.SetActive(true);
                _swimmingSprite.SetActive(false);
            }
        }
    }
    public void Slide(InputAction.CallbackContext context)
    {
        if (context.started && _playerMovements.IsSwimming)
        {
            _playerMovements.ActualSpeed = 15f;
        }
        if (context.canceled || !_playerMovements.IsSwimming)
        {
            _playerMovements.ActualSpeed = 7f;
        }
    }

    private void Awake()
    {
        _playerMovements = GetComponent<PlayerMovements>();
    }
    void FixedUpdate()
    {
        Ray myRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(myRay, out _hit, 1))
        {
            IsSwimming(_hit);
        }
        else
        {
            _playerMovements.IsSwimming = false;
            _player.SetActive(true);
            _swimmingSprite.SetActive(false);
        }
        Debug.DrawRay(transform.position, Vector3.down, Color.green, 10);
    }
}