using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VolumetricLightsDemo;

public class PlayerSwim : MonoBehaviour
{
    [SerializeField] private GameObject _swimmingSprite;
    [SerializeField] private GameObject _player;
    private PlayerMovements _playerMovements;
    private RaycastHit hit;

    private void Awake()
    {
        _playerMovements = GetComponent<PlayerMovements>();
    }
    void FixedUpdate()
    {
        Ray myRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(myRay, out hit, 1))
        {
            IsSwimming(hit);
        }
        else
        {
            _playerMovements.IsSwimming = false;
            _player.SetActive(true);
            _swimmingSprite.SetActive(false);
        }
        Debug.DrawRay(transform.position, Vector3.down, Color.green, 10);
    }
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
}