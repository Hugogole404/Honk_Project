using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class UI_Timer : MonoBehaviour
{
    [SerializeField] private CanvasGroup UI_ToActivate_or_not;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _maxTimerUI;
    [SerializeField] private GameObject _uiKeyBoard;
    [SerializeField] private GameObject _uiController;
    private float _currentTimerUI;
    private bool _canBeSee;
    private bool _fadeInIsPlayed;
    private bool _fadeOutIsPlayed;
    public AreaUI _areaUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            _areaUI.UI_ToActivate_or_not = UI_ToActivate_or_not;
            _canBeSee = true;
        }
    }
    private void CheckTimer()
    {
        if (_canBeSee && _fadeOutIsPlayed == false)
        {
            if (_fadeInIsPlayed == false)
            {
                _areaUI.FadeIn(_fadeDuration);
                _fadeInIsPlayed = true;
            }
            if (_currentTimerUI >= _maxTimerUI)
            {
                if (_fadeOutIsPlayed == false)
                {
                    _areaUI.FadeOut(_fadeDuration);
                    _fadeOutIsPlayed = true;
                }
            }
            _currentTimerUI += Time.deltaTime;
        }
    }
    void ChooseGoodUI()
    {
        if (_areaUI.IsKeyboard)
        {
            _uiKeyBoard.SetActive(true);
            _uiController.SetActive(false);
        }
        else
        {
            _uiKeyBoard.SetActive(false);
            _uiController.SetActive(true);
        }
    }

    private void Start()
    {
        _areaUI = FindObjectOfType<AreaUI>();
    }
    private void Update()
    {
        CheckTimer();
        ChooseGoodUI();
    }
}