using FMOD;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class PlayerSphereSlope : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    bool _isActive;

    private void Start()
    {
        transform.position = _spawnPoint.position;
    }
}