using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class RiftBaby : MonoBehaviour
{
    public GameObject PointTpInsideWall;
    [SerializeField] private GameObject _replaceBaby;
    [SerializeField] private GameObject _targetInsideWall;

    public List<GameObject> Shrooms = new List<GameObject>();
    public int currentListNum;
    public float Scalemult = 0.5f;
    public float vitesseSwitch = 30;
    //public GameObject EmptyFacing;
    public AudioSource Tp_Sound;
    [HideInInspector] public HoldBaby _holdBaby;
    public ParticleSystem fxOut;
    public float DelayFX;

    private GameObject baby;
    private Baby _baby;
    private TestBabyWalk _testBabyWalk;
    private PlayerMovements _playerMovements;
    private ReplaceBaby _replaceBabyScript;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TestBabyWalk>() != null && _holdBaby.IsOnHisBack == false)
        {
            _replaceBabyScript.ReplaceBabyInRift(_playerMovements, _testBabyWalk, _targetInsideWall);
            //_playerMovements.CanBabyTeleport = true;
            //other.gameObject.GetComponent<TestBabyWalk>().gameObject.GetComponent<CharacterController>().enabled = false;
            //other.gameObject.GetComponent<TestBabyWalk>().gameObject.transform.position = _pointEnterRift.transform.position;
            //other.gameObject.GetComponent<TestBabyWalk>().gameObject.GetComponent<CharacterController>().enabled = true;

            //_playerMovements.CanBabyTeleport = false;
            //_holdBaby.CanHoldBaby = false;
        }
    }
    public void ScaleShroom(Action action)
    {
        if (Shrooms.Count != 0)
        {
            currentListNum = 0;
            StartCoroutine(ChampiScale(action));
            Tp_Sound.Play();
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
        _replaceBabyScript = _replaceBaby.GetComponent<ReplaceBaby>();
    }

    private IEnumerator ChampiScale(Action action)
    {
        Shrooms[currentListNum].transform.DOPunchScale(Shrooms[currentListNum].transform.localScale * Scalemult, 1f, 0, 0);
        currentListNum++;
        if (currentListNum == Shrooms.Count - 8)
        {
            action?.Invoke();
            StartCoroutine(ChampiScale(action));
        }
        else if (currentListNum < Shrooms.Count - 1)
        {
            float distance = Vector3.Distance(Shrooms[currentListNum].transform.position, Shrooms[currentListNum + 1].transform.position);
            yield return new WaitForSeconds(distance / vitesseSwitch);
            StartCoroutine(ChampiScale(action));
        } 
        else
        {
            // END
            yield return new WaitForSeconds(DelayFX);
            fxOut.Play();
            yield return new WaitForSeconds(0.5f);
            
        }
    }
}