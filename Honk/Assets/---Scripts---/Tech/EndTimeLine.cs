using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndTimeLine : MonoBehaviour
{
    public PlayableDirector playableDirector;
    [SerializeField] private AreaUI _areaUI;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private string _nameScene;
    [SerializeField] private float _currentTimer;
    [SerializeField] private bool _canTimer;

    private void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
            playableDirector.Play();
        }
    }
    private void Update()
    {
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
            _areaUI.UI_ToActivate_or_not = _canvasGroup;
            _canTimer = true;
            _areaUI.FadeIn(_fadeDuration);
        }
    }
}