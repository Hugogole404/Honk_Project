using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftBaby : MonoBehaviour
{
    [SerializeField] private GameObject _pointEnterRift;
    private HoldBaby _holdBaby;
    private Baby _baby;
    private TestBabyWalk _testBabyWalk;
    private PlayerMovements _playerMovements;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<TestBabyWalk>() != null && _holdBaby.IsOnHisBack == false)
        {
            // lancer l'anim où il rentre 
            // deplacer la cam 
            other.gameObject.transform.position = _pointEnterRift.transform.position;

            //_baby.LastPositionPlayer.Add(_pointEnterRift.gameObject.transform.position);
            //_baby.Offset = _playerMovements.gameObject.transform.position - _pointEnterRift.gameObject.transform.position;
            //_baby.Offset.y = 0;


            //_testBabyWalk.Offset = _playerMovements.gameObject.transform.position - _pointEnterRift.gameObject.transform.position;
            //_testBabyWalk.Offset.y = 0;

            // lancer l'anim ou il ressort 
        }
    }
    private void Start()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        _baby = FindAnyObjectByType<Baby>();
        _playerMovements = FindAnyObjectByType<PlayerMovements>();
        _testBabyWalk = FindAnyObjectByType<TestBabyWalk>();
    }
}