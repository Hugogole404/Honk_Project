using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using DG.Tweening;
using System;

public class RiftBaby : MonoBehaviour
{
    [SerializeField] private GameObject _pointEnterRift;
    private GameObject baby;
    private HoldBaby _holdBaby;
    private Baby _baby;
    private TestBabyWalk _testBabyWalk;
    private PlayerMovements _playerMovements;
    public List<GameObject> Shrooms = new List<GameObject>();
    public int currentListNum;
    private Sequence mySequence = DOTween.Sequence();
    public float Scalemult = 0.5f;
    public float vitesseSwitch = 30;
    public GameObject EmptyFacing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TestBabyWalk>() != null)
        {
            _playerMovements.CanBabyTeleport = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<TestBabyWalk>() != null && _holdBaby.IsOnHisBack == false && _playerMovements.CanTeleportbabyRift)
        {
            currentListNum = 0;
            StartCoroutine(ChampiScale());

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
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TestBabyWalk>() != null)
        {
            _playerMovements.CanBabyTeleport = false;
        }
    }
    private void Start()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        _baby = FindAnyObjectByType<Baby>();
        _playerMovements = FindAnyObjectByType<PlayerMovements>();
        _testBabyWalk = FindAnyObjectByType<TestBabyWalk>();
    }

    private IEnumerator ChampiScale()
    {
        float distance = Vector3.Distance(Shrooms[currentListNum].transform.position, Shrooms[currentListNum + 1].transform.position);
        Shrooms[currentListNum].transform.DOPunchScale(Shrooms[currentListNum].transform.localScale * Scalemult, 1f, 0, 0);
        currentListNum++;
        yield return new WaitForSeconds(distance/vitesseSwitch);
        if (currentListNum < Shrooms.Count + 1)
        {
            StartCoroutine(ChampiScale());
        }
    }

    //private void FaceBreach()
    //{

    //}
}