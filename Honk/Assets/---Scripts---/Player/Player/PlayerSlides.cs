using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerSlides : MonoBehaviour
{
    [Range(0.0f, 1.0f)] public float mySliderFloat;

    public bool IsSliding;
    public float AngleSlide;
    private PlayerMovements _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovements>();
    }
    public void Slide(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        _playerMovement.transform.rotation = Quaternion.EulerRotation(AngleSlide, _playerMovement.transform.rotation.y, _playerMovement.transform.rotation.z);
    }
}