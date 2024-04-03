using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaMovements : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject[] _pointToPass;

    private int _originNbrPointToPass;
    private int _nbrPointToPass;
    private Rigidbody _rb;
    private GameObject _target;
    private PlayerMovements _playerMov;

    [HideInInspector] public bool HavePlayerAggro = false;

    private void Move()
    {
        if (HavePlayerAggro && transform.position != _playerMov.transform.position)
        {
            _target = _playerMov.gameObject;
        }
        if (HavePlayerAggro == false)
        {
            _target = _pointToPass[_nbrPointToPass];

            if ((transform.position.x - _target.transform.position.x) < 0.1f && (transform.position.x - _target.transform.position.x) > -0.1f
                && (transform.position.z - _target.transform.position.z) < 0.1f && (transform.position.z - _target.transform.position.z) > -0.1f)
            {
                _nbrPointToPass++;
            }
            if (_nbrPointToPass >= _originNbrPointToPass)
            {
                _nbrPointToPass = 0;
            }
        }

        if (transform.position.x < _target.transform.position.x)
        {
            transform.position += new Vector3(_speed * Time.deltaTime, 0);
        }
        if (transform.position.x > _target.transform.position.x)
        {
            transform.position += new Vector3(-_speed * Time.deltaTime, 0);
        }
        if (transform.position.z < _target.transform.position.z)
        {
            transform.position += new Vector3(0, 0, _speed * Time.deltaTime);
        }
        if (transform.position.z > _target.transform.position.z)
        {
            transform.position += new Vector3(0, 0, -_speed * Time.deltaTime);
        }
    }
    private void Orientation()
    {
        Vector3 playerPos = new Vector3(_target.transform.position.x, 0, _target.transform.position.z);
        Vector3 aimDir = playerPos - _rb.position;
        float aimAngle = Mathf.Atan2(aimDir.z, aimDir.x) * Mathf.Rad2Deg - 90f;
        transform.localEulerAngles = new Vector3(0, aimAngle, 0);
        //_rb.rotation = aimAngle;
    }
    private void Start()
    {
        _playerMov = FindAnyObjectByType<PlayerMovements>();
        _rb = GetComponent<Rigidbody>();
        foreach (var item in _pointToPass)
        {
            _originNbrPointToPass++;
        }
    }
    private void Update()
    {
        Move();
        Orientation();
    }
}