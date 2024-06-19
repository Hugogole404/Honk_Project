using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    [Header("Components :")]
    public PlayableDirector playableDirector;

    [SerializeField] private AudioSource _sourceFadeOut;
    [SerializeField] private AreaUI _areaUI;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _blackFade;
    [SerializeField] private GameObject _fadeOut;

    [Header("Values :")]
    [SerializeField] private float _fadeDuration;
    [SerializeField] private string _nameScene;

    [Header("!-!-! Don't touch please - Danger zone !-!-!")]
    [SerializeField] private InputAction _action;
    [SerializeField] private float _currentTimer;
    [SerializeField] private bool _canTimer;

    private bool _sceneLoaded = false;
    private bool _timelineStarted = false;
    private bool _canPressAnyKey;

    private void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }

        // Enable the input action
        _canTimer = false;
        _action.Enable();
        _fadeOut.SetActive(false);
    }

    private void Update()
    {
        // Check for any key press or input action trigger
        if (/*!_timelineStarted && */(Input.anyKeyDown || _action.triggered) && _canPressAnyKey && !_canTimer)
        {
            _timelineStarted = true;
            _blackFade.SetActive(false);
            playableDirector.Play();

            if (_fadeOut != null)
                _fadeOut.SetActive(true);

            _canTimer = true;
        }

        if (_canTimer)
        {
            _currentTimer += Time.deltaTime;

            if (_sourceFadeOut != null)
                _sourceFadeOut.volume -= Time.deltaTime;

            if (_currentTimer >= _fadeDuration)
            {
                LoadScene();
            }
        }
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }

        // Disable the input action
        _action.Disable();
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            _canPressAnyKey = true;

            //LoadScene();
        }
    }

    private void LoadScene()
    {
        if (!_sceneLoaded)
        {
            _sceneLoaded = true;
            SceneManager.LoadScene(_nameScene);
        }
    }
}