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
    [SerializeField] private ParticleSystem FXFall;
    [SerializeField] private ShakeTransform ShakeTransform;
    [SerializeField] private GameObject _deathZone;

    public ShakeData ShakeData;
    private float t_transform_initial;
    public float _currentTimer;
    public bool CanFall;
    private bool _canTimerIncrease;
    public bool _isDad;
    public bool _isBaby;
    private bool _shake = true;
    public AudioSource Sound_plateform;
    private void Fall()
    {
        if (_shake == true && _platform.transform.position.y <= t_transform_initial)
        {

            ScreenShake.Instance.Shake(ShakeData);
            FXFall.Play();
            _shake = false;
        }

        if (CanFall && _platform.transform.position.y > t_transform_initial)
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
            if (CanFall == false) { ShakeTransform.Begin(); } //FXShake.SetActive(true);
        }
        else if (other.gameObject.GetComponent<TestBabyWalk>() != null)
        {
            _canTimerIncrease = true;
            _isBaby = true;
            if (CanFall == false) { ShakeTransform.Begin(); } //FXShake.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovements>() != null)
        {
            ShakeTransform.Stop();
            _canTimerIncrease = false;
            _currentTimer = 0;
        }
        else if (other.gameObject.GetComponent<TestBabyWalk>() != null)
        {
            ShakeTransform.Stop();
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
                //FXShake.SetActive(false);
                CanFall = true;
            }
        }
        else if (_isBaby)
        {
            if (_currentTimer > _maxTimerBaby)
            {
                ShakeTransform.Stop();
                //FXShake.SetActive(false);
                if (_deathZone != null)
                {
                    _deathZone.SetActive(true);
                }
                CanFall = true;
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