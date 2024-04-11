using UnityEngine;
using UnityEngine.InputSystem;

public class Baby : MonoBehaviour
{
    private Transform ToFollow;

    [SerializeField] private Vector3 _offset;
    private HoldBaby _holdBaby;

    void Start()
    {
        _offset = ToFollow.position - transform.position;
    }
    private void MoveBaby()
    {
        // pas encore operationnel
        if(_holdBaby.IsOnHisBack == false)
        {
            transform.position = ToFollow.position - _offset;
            transform.rotation = ToFollow.rotation;
        }
    }

    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        ToFollow = FindAnyObjectByType<PlayerMovements>().transform;
    }
    private void Update()
    {
        MoveBaby();
    }
}