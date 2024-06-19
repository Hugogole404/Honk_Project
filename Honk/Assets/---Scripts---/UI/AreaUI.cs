using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;

public class AreaUI : MonoBehaviour
{
    public bool IsKeyboard;
    [HideInInspector] public CanvasGroup UI_ToActivate_or_not;
    private Tween _fadeTween;

    public void ControllerPress(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            IsKeyboard = false;
        }
    }  
    public void KeyBoardPress(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            IsKeyboard = true;
        }
    }
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
}