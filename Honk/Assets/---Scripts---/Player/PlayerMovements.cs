using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _speed, _maxSpeed;
    #endregion

    void TeleportToSpawnPoint()
    {
        transform.position = _spawnPoint.position;
    }

    private void Start()
    {
        TeleportToSpawnPoint();
    }
}