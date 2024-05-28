using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AreaUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup UI_ToActivate_or_not;
    [SerializeField] private float _fadeDuration;
    private Tween _fadeTween;

    private void Fade(float endVal, float duration, TweenCallback onEnd)
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill(false);
        }
        _fadeTween = UI_ToActivate_or_not.DOFade(endVal, duration);
        _fadeTween.onComplete += onEnd;
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () => { UI_ToActivate_or_not.interactable = true; UI_ToActivate_or_not.blocksRaycasts = true; });
    }
    public void FadeOut(float duration)
    {
        Fade(0f, duration, () => { UI_ToActivate_or_not.interactable = false; UI_ToActivate_or_not.blocksRaycasts = false; });
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            FadeIn(_fadeDuration);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            FadeOut(_fadeDuration);
        }
    }
}