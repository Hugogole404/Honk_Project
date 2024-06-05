using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlStopped : MonoBehaviour
{
    [SerializeField] private PlayerMovements _playerMovements;
    [SerializeField] private float _maxTimerToStopPlayerInputs;
    private float _currentTimerToStopPlayerInputs;
    private bool _timerCanIncrease;

    private void Start()
    {
        _timerCanIncrease = true;
    }
    private void Update()
    {
        TimerCancelPlayerInputs();
    }

    private void TimerCancelPlayerInputs()
    {
        if (_timerCanIncrease)
        {
            _playerMovements.CanPlayerUseInputs = false;
            _currentTimerToStopPlayerInputs += Time.deltaTime;
            if(_currentTimerToStopPlayerInputs >= _maxTimerToStopPlayerInputs)
            {
                _timerCanIncrease = false;
                _playerMovements.CanPlayerUseInputs = true;
            }
        }
    }
}