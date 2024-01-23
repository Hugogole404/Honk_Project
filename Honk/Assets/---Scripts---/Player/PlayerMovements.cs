using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    #region Variables
    [Header("SpawnPoint")]
    [SerializeField] private Transform _spawnPoint;

    [Header("Movements")]
    [SerializeField] private float _maxSpeed;
    private float _actualSpeed;


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