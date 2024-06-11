using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AreaEndZone : MonoBehaviour
{
    public string NameSceneToLoad;

    [SerializeField] private VolumeManager _volumeManager;

    AreaUI _areaUI;
    float _maxTimer;
    float _currentTimer;
    bool _canTimer;
    PlayerMovements _playerMovements;

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
        _playerMovements = FindAnyObjectByType<PlayerMovements>();
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
        {
            _volumeManager.GetTimerMusic();

            if (_playerMovements != null)
            {
                _playerMovements.CanPlayerUseInputs = false;
                _playerMovements.Direction = Vector3.zero;
                _playerMovements.Input = Vector3.zero;
                _playerMovements.AnimatorHonk.SetBool("IsMoving", false);
            }

            if (_currentTimer >= _maxTimer + 0.75f)
            {
                SceneManager.LoadScene(NameSceneToLoad);
            }
        }
    }
}