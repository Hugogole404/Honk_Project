using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class Start_cine_start : MonoBehaviour
{
    [SerializeField] PlayableDirector _timeLine;

    [SerializeField] Animator _animatorHonk;
    [SerializeField] Animator _animatorText;
    [SerializeField] Animator _animatorBip;
    [SerializeField] Animator _animatorFade;

    [SerializeField] Transform _camCine;
    [SerializeField] Transform _camPlayer;

    [SerializeField] float _speed;
    [SerializeField] float _timer;
    [SerializeField] string _nameScene;

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

        if (_timeLine.duration + _timer - 2 < _time)
        {
            float chrono = _time - (float)(_timeLine.duration + _timer - 2);
            float time = (chrono * _speed * Time.deltaTime) / Vector3.Distance(_camPlayer.position, _camCine.position);
            _camCine.position = Vector3.Lerp(_camCine.position, _camPlayer.position, time);
            _camCine.rotation = Quaternion.Lerp(_camCine.rotation, _camPlayer.rotation, time);
            _animatorFade.SetTrigger("FadeOut");

            if (time >= 1)
            {
                //_camCine.gameObject.SetActive(false);
                //_camPlayer.gameObject.SetActive(true);
                //Destroy(gameObject);
                SceneManager.LoadScene(_nameScene);
            }
        }
    }
}
