using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class End_Ui_yes_yes : MonoBehaviour
{
    [SerializeField] Transform _camCine;
    [SerializeField] Transform _camPlayer;

    [SerializeField] Animator _animatorFade;

    [SerializeField] float _speed;
    [SerializeField] int end_start;
    [SerializeField] float _timer;
    [SerializeField] string _nameScene;

    float _time;
    bool _isPlaying;

    private void Start()
    {
        Cursor.visible = false;
        _time = 0;
    }

    private void Update()
    {
        if (_isPlaying)
        {
            _time += Time.deltaTime;

            float time = (_speed * Time.deltaTime) / Vector3.Distance(_camPlayer.position, _camCine.position);

            _camCine.position = Vector3.Lerp(_camCine.position, _camPlayer.position, time);
            _camCine.rotation = Quaternion.Lerp(_camCine.rotation, _camPlayer.rotation, time);

            _animatorFade.SetTrigger("FadeOut");

            if (_time >= end_start)
            {
                print("test");
                _animatorFade.enabled = false;
                SceneManager.LoadScene(_nameScene);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null || other.GetComponent<Slope>() != null)
        {
            _isPlaying = true;
        }
    }
}