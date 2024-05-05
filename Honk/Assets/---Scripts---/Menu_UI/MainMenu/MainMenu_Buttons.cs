using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu_Buttons : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private float _scalePointer;
    [SerializeField] private float _punchPower;
    [SerializeField] private float _duration;
    float _oldScale;

    public void OnClick()
    {
        transform.DOPunchScale(new Vector3(_punchPower, _duration, 0), 0.3f).OnComplete(() => { SceneManager.LoadScene($"{_sceneName}"); });
    }
    private void Start()
    {
        _oldScale = transform.localScale.x;
    }
    public void OnMouseEnter()
    {
        transform.DOComplete();
        transform.DOScale(_scalePointer, 0.2f);
    }
    public void OnMouseExit()
    {
        transform.DOComplete();
        transform.DOScale(_oldScale, 0.2f);
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}