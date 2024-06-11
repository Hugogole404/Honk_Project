using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    public PlayableDirector playableDirector;
    [SerializeField] private AreaUI _areaUI;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private string _nameScene;
    [SerializeField] private float _currentTimer;
    [SerializeField] private bool _canTimer;
    [SerializeField] InputAction _action;
    [SerializeField] private GameObject _blackFade;

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
    }

    private void Update()
    {
        // Check for any key press or input action trigger
        if (/*!_timelineStarted && */(Input.anyKeyDown || _action.triggered) && _canPressAnyKey)
        {
            _timelineStarted = true;
            _blackFade.SetActive(false);
            playableDirector.Play();
            _canTimer = true;
        }

        if (_canTimer)
        {
            _currentTimer += Time.deltaTime;
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