using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject _platform;
    [SerializeField] private float _fallDistValue;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxTimerBaby;
    [SerializeField] private float _maxTimerDad;
    [SerializeField] private GameObject FXShake;
    [SerializeField] private ShakeTransform ShakeTransform;

    public ShakeData ShakeData;
    private float t_transform_initial;
    private float _currentTimer;
    private bool _canFall;
    private bool _canTimerIncrease;
    private bool _isDad;
    private bool _isBaby;
    private void Fall()
    {
        if (_canFall && _platform.transform.position.y > t_transform_initial)
        {
            _platform.transform.position -= new Vector3(0, _speed * Time.deltaTime, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovements>() != null)
        {
            _canTimerIncrease = true;
            _isDad = true;
            if (_canFall == false) { ShakeTransform.Begin(); FXShake.SetActive(true); }
        }
        else if (other.gameObject.GetComponent<TestBabyWalk>() != null)
        {
            _canTimerIncrease = true;
            _isBaby = true;
            if (_canFall == false) { ShakeTransform.Begin(); FXShake.SetActive(true); }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovements>() != null)
        {
            _canTimerIncrease = false;
            _currentTimer = 0;
        }
        else if (other.gameObject.GetComponent<TestBabyWalk>() != null)
        {
            _canTimerIncrease = false;
            _currentTimer = 0;
        }
    }
    private void Timer()
    {
        if (_canTimerIncrease)
        {
            _currentTimer += Time.deltaTime;

        }
        if (_isDad)
        {
            if (_currentTimer > _maxTimerDad)
            {
                ShakeTransform.Stop();
                FXShake.SetActive(false);
                _canFall = true;
            }
        }
        else if (_isBaby)
        {
            if (_currentTimer > _maxTimerBaby)
            {
                ShakeTransform.Stop();
                FXShake.SetActive(false);
                _canFall = true;
            }
        }
    }
    private void Update()
    {
        Timer();
        Fall();
    }
    private void Start()
    {
        t_transform_initial = _platform.transform.position.y - _fallDistValue;
    }
}