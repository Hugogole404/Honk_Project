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

    private void Update()
    {
        if (Input.anyKeyDown || _action.triggered)
        {
            if (playableDirector != null)
            {
                playableDirector.stopped += OnPlayableDirectorStopped;
                _blackFade.SetActive(false);
                SceneManager.LoadScene(_nameScene);

                playableDirector.Play();
            }
        }
        if (_canTimer)
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= _fadeDuration)
            {
                SceneManager.LoadScene(_nameScene);
            }
        }
    }
    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            //_areaUI.UI_ToActivate_or_not = _canvasGroup;
            _canTimer = true;
            //_areaUI.FadeIn(_fadeDuration);
        }
    }
}