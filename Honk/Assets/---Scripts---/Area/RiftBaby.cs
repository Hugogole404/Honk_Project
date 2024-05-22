using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using DG.Tweening;

public class RiftBaby : MonoBehaviour
{
    [SerializeField] private GameObject _pointEnterRift;
    [SerializeField] private GameObject _replaceBaby;

    private GameObject baby;
    private HoldBaby _holdBaby;
    private Baby _baby;
    private TestBabyWalk _testBabyWalk;
    private PlayerMovements _playerMovements;
    public List<GameObject> Shrooms = new List<GameObject>();
    public int currentListNum;
    public float Scalemult = 0.5f;
    public float vitesseSwitch = 30;
    public GameObject EmptyFacing;
    public AudioSource Tp_Sound;

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

            // lancer l'anim où il rentre 
            if (Shrooms.Count != 0)
            {
                currentListNum = 0;
                StartCoroutine(ChampiScale());
                Tp_Sound.Play();
            }
            other.gameObject.transform.position = _pointEnterRift.transform.position;

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
        Shrooms[currentListNum].transform.DOPunchScale(Shrooms[currentListNum].transform.localScale * Scalemult, 1f, 0, 0);
        currentListNum++;
        if (currentListNum < Shrooms.Count - 1)
        {
            float distance = Vector3.Distance(Shrooms[currentListNum].transform.position, Shrooms[currentListNum + 1].transform.position);
            yield return new WaitForSeconds(distance / vitesseSwitch);
            StartCoroutine(ChampiScale());
        }
    }
    //private void FaceBreach()
    //{
    //}
}