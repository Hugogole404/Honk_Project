using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private CanvasGroup _menu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settings;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private VolumeProfile _volumeProfileGame;
    [SerializeField] private VolumeProfile _volumeProfileMenu;
    [SerializeField] private List<GameObject> _listButtonsOrange;
    [SerializeField] private VolumeManager _volumeManager;
    private float _oldPlayerSpeed;
    private bool _isMenuOpen;
    private AreaUI _areaUI;
    private PlayerMovements _playerMovements;
    private int _currentIndex;
    private bool _isSettingsOpen;
    private bool _canInterract;

    public void Menu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isMenuOpen == false)
            {
                OpenMenu();
            }
            else
            {
                if (_isSettingsOpen == false)
                {
                    CloseMenu();
                }
            }
        }
        if (context.canceled)
        {
            if(_isMenuOpen == false)
            {

            }
            _canInterract = true;
        }
    }
    public void NavigateInMenuTop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isMenuOpen)
            {
                _currentIndex -= 1;
                if (_currentIndex < 0)
                {
                    //_currentIndex = _listButtonsOrange.Count - 1;
                    _currentIndex = 0;
                }
                foreach (var button in _listButtonsOrange)
                {
                    button.gameObject.SetActive(false);
                }
                _listButtonsOrange[_currentIndex].gameObject.SetActive(true);
            }
        }
    }
    public void NavigateInMenuDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isMenuOpen)
            {
                _currentIndex += 1;
                if (_currentIndex > _listButtonsOrange.Count - 1)
                {
                    //_currentIndex = 0;
                    _currentIndex = _listButtonsOrange.Count - 1;
                }
                foreach (var button in _listButtonsOrange)
                {
                    button.gameObject.SetActive(false);
                }
                _listButtonsOrange[_currentIndex].gameObject.SetActive(true);
            }
        }
    }
    public void ValidSelection(InputAction.CallbackContext context)
    {
        if (_isMenuOpen)
        {
            if (context.performed)
            {
                if (_currentIndex == 0)
                {
                    CloseMenu();
                }
                if (_currentIndex == 1)
                {
                    _settings.SetActive(true);
                    _mainMenu.SetActive(false);
                    _isSettingsOpen = true;
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    }
    public void ReturnBeforeMenu(InputAction.CallbackContext context)
    {
        if (_isMenuOpen)
        {
            if (context.performed)
            {
                if (_isSettingsOpen)
                {
                    _volumeManager.SetVolume();
                    _settings.SetActive(false);
                    _mainMenu.SetActive(true);
                    _isSettingsOpen = false;
                }
            }
        }
    }

    private void OpenMenu()
    {
        Cursor.visible = true;
        _isMenuOpen = true;
        _playerMovements.CanPlayerUseInputs = false;
        _areaUI.UI_ToActivate_or_not = _menu;
        _areaUI.FadeIn(_fadeDuration);
        _playerMovements.BaseSpeed = 0;
        _playerMovements.Direction = Vector3.zero;
        _playerMovements.AnimatorHonk.SetBool("IsMoving", false);
    }
    private void CloseMenu()
    {
        Cursor.visible = false;
        _isMenuOpen = false;
        _playerMovements.CanPlayerUseInputs = true;
        _areaUI.UI_ToActivate_or_not = _menu;
        _areaUI.FadeOut(_fadeDuration);
        _playerMovements.BaseSpeed = _oldPlayerSpeed;
        _settings.SetActive(false);
        _mainMenu.SetActive(true);
    }
    private void Start()
    {
        _areaUI = FindObjectOfType<AreaUI>();
        _playerMovements = FindObjectOfType<PlayerMovements>();
        _isMenuOpen = false;
        _oldPlayerSpeed = _playerMovements.BaseSpeed;
    }
    //private void Update()
    //{
    //    print(_currentIndex);
    //}
}