using FMOD;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class PlayerSphereSlope : MonoBehaviour
{
    public GameObject Sphere;
    bool _isActive;
    private PlayerMovements _playerMovements;

    private void Awake()
    {
        _playerMovements = GetComponent<PlayerMovements>();
        Sphere.SetActive(_isActive);
    }
    private void Update()
    {
        _isActive = !_isActive;
        Sphere.SetActive(_isActive);
    }
}