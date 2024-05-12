using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayArea : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listOfPrefab = new List<GameObject>();
    [SerializeField] private List<GameObject> _listOfObjToResetInScene = new List<GameObject>();
    [SerializeField] private bool PlayerHaveToBeReset;
    [SerializeField] private KeyCode _keyBind;

    private List<Vector3> _listOfPositions = new List<Vector3>();
    private PlayerMovements _playerMovements;
    private int _actualIndex;

    private void AreaReset()
    {
        foreach (GameObject obj in _listOfObjToResetInScene)
        {
            Destroy(obj);
        }
        _listOfObjToResetInScene.Clear();
        foreach (GameObject obj in _listOfPrefab)
        {
            GameObject go = Instantiate(_listOfPrefab[_actualIndex]);
            _listOfObjToResetInScene.Add(go);
            _actualIndex += 1;
        }
        _actualIndex = 0;
    }

    private void Awake()
    {
        _playerMovements= FindObjectOfType<PlayerMovements>();
    }

    private void Start()
    {
        foreach (GameObject obj in _listOfObjToResetInScene)
        {
            _listOfPositions.Add(obj.transform.position);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(_keyBind))
        {
            if(PlayerHaveToBeReset)
            {
                // reset player
            }
            // reset atelier
            AreaReset();
        }
    }

    // si on a besoin de reset un atelier mais pas ceux passé on avance l'index 
}