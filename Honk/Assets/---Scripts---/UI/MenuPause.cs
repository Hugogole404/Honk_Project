using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private CanvasGroup _menu;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private VolumeProfile _volumeProfileGame;
    [SerializeField] private VolumeProfile _volumeProfileMenu;
    private bool _isMenuOpen;
    private AreaUI _areaUI;
    private PlayerMovements _playerMovements;


    public void Menu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isMenuOpen == false)
            {
                _isMenuOpen = true;
                _playerMovements.CanPlayerUseInputs = false;
                _areaUI.UI_ToActivate_or_not = _menu;
                _areaUI.FadeIn(_fadeDuration);
            }
            else
            {
                _isMenuOpen = false;
                _playerMovements.CanPlayerUseInputs = true;
                _areaUI.UI_ToActivate_or_not = _menu;
                _areaUI.FadeOut(_fadeDuration);
            }
        }
    }
    private void Start()
    {
        _areaUI = FindObjectOfType<AreaUI>();
        _playerMovements = FindObjectOfType<PlayerMovements>();
        _isMenuOpen = false;
    }
}