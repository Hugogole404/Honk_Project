using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Start_cine_start : MonoBehaviour
{
    [SerializeField] PlayableDirector _timeLine;

    [SerializeField] Animator _animatorHonk;
    [SerializeField] Animator _animatorText;
    [SerializeField] Animator _animatorBip;

    [SerializeField] Transform _camCine;
    [SerializeField] Transform _camPlayer;

    [SerializeField] float _speed;
    [SerializeField] float _timer;

    float _time;
    bool _isTiming;

    private void Start()
    {
        _isTiming = false;
        _time = 0;
        _animatorHonk.SetBool("Sleep", true);
    }

    private void Update()
    {
        if (_time < _timer || _isTiming)
            _time += Time.deltaTime;
        else
        {
            if (_time > _timer)
                _time = _timer;

            if (Input.anyKeyDown)
            {
                _timeLine.Play();

                _animatorHonk.SetBool("Sleep", false);
                _animatorText.SetTrigger("Disapear");
                _animatorBip.SetTrigger("Bip");

                _isTiming = true;
            }
        }

        if (_timeLine.duration + _timer < _time)
        {
            float chrono = _time - (float)(_timeLine.duration + _timer);
            float time = (chrono * _speed * Time.deltaTime) / Vector3.Distance(_camPlayer.position, _camCine.position);
            _camCine.position = Vector3.Lerp(_camCine.position, _camPlayer.position, time);
            _camCine.rotation = Quaternion.Lerp(_camCine.rotation, _camPlayer.rotation, time);

            if (time >= 1)
            {
                _camCine.gameObject.SetActive(false);
                _camPlayer.gameObject.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}
