using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayArea : MonoBehaviour
{
    public List<GameObject> ListOfObjToResetInScene = new List<GameObject>();
    [SerializeField] private bool PlayerHaveToBeReset;
    [SerializeField] private KeyCode _keyBind;

    public List<Vector3> _listOfPositions = new List<Vector3>();
    private PlayerMovements _playerMovements;
    private int _actualIndex;

    public void AreaReset()
    {
        if (PlayerHaveToBeReset)
        {
            _playerMovements._testBabyWalk.transform.parent = _playerMovements.BabyParent.transform;
        }
        foreach (GameObject obj in ListOfObjToResetInScene)
        {
            if(obj.GetComponent<Rigidbody>() != null)
            {
                obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if(obj.GetComponentInChildren<Platform>() != null)
            {
                obj.GetComponentInChildren<Platform>().CanFall = false;
                obj.GetComponentInChildren<Platform>()._isBaby = false;
                obj.GetComponentInChildren<Platform>()._isDad = false;
                obj.GetComponentInChildren<Platform>()._currentTimer = 0;
            }
            obj.transform.position = _listOfPositions[_actualIndex];
            _actualIndex++;
        }
        _actualIndex = 0;
    }

    private void Awake()
    {
        _playerMovements = FindObjectOfType<PlayerMovements>();
    }

    private void Start()
    {
        foreach (GameObject obj in ListOfObjToResetInScene)
        {
            _listOfPositions.Add(obj.transform.position);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(_keyBind))
        {
            if (PlayerHaveToBeReset)
            {
                _playerMovements.TeleportToSpawnPoint();
            }
            AreaReset();
        }
    }
    // si on a besoin de reset un atelier mais pas ceux déja fait, on avance l'index 
}