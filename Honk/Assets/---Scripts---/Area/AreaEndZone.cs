using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEndZone : MonoBehaviour
{
    public string NameSceneToLoad;
    AreaUI _areaUI;
    float _maxTimer;
    float _currentTimer;
    bool _canTimer;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null || other.GetComponent<Slope>() != null)
        {
            _areaUI.FadeIn(1.5f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null || other.GetComponent<Slope>() != null)
        {
            _canTimer = true;
        }
    }
    private void Awake()
    {
        _areaUI = FindAnyObjectByType<AreaUI>();
    }
    private void Start()
    {
        _currentTimer = 0;
        _maxTimer = 1.5f;
    }
    private void Update()
    {
        if (_canTimer)
        {
            _currentTimer += Time.deltaTime;
        }
        if (_currentTimer >= _maxTimer)
            SceneManager.LoadScene(NameSceneToLoad);
    }
}