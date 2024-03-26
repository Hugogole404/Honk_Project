using UnityEngine;

public class PlayerSphereSlope : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PlayerCheckStates _playerCheckStates;

    private void Start()
    {
        _playerCheckStates.WalkToSlope();
        transform.position = _spawnPoint.position;
    }
}