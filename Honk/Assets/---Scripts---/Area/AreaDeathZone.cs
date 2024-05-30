using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class AreaDeathZone : MonoBehaviour
{
    private ResetPlayArea _resetArea;
    private PlayerMovements _playerMovements;
    private void Awake()
    {
        _resetArea = FindAnyObjectByType<ResetPlayArea>();
        _playerMovements = FindAnyObjectByType<PlayerMovements>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            other.transform.position = other.GetComponent<Slope>().SpawnPoint.transform.position;
        }
        if (other.GetComponent<PlayerMovements>() != null)
        {
            _playerMovements.TeleportToSpawnPoint();
            if (_resetArea.ListOfObjToResetInScene.Count > 0)
            {
                _resetArea.AreaReset();
            }
        }
        else if (other.GetComponent<TestBabyWalk>() != null)
        {
            _playerMovements.TeleportToSpawnPoint();
            if (_resetArea.ListOfObjToResetInScene.Count > 0)
            {
                _resetArea.AreaReset();
            }
        }
    }
}