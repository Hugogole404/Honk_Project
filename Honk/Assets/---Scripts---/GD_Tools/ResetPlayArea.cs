using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayArea : MonoBehaviour
{
    public List<GameObject> ListOfObjToResetInScene = new List<GameObject>();
    [SerializeField] private List<GameObject> _listOfPrefab = new List<GameObject>();
    [SerializeField] private bool PlayerHaveToBeReset;
    [SerializeField] private KeyCode _keyBind;

    private List<Vector3> _listOfPositions = new List<Vector3>();
    private PlayerMovements _playerMovements;
    private TestBabyWalk _testBabyWalk;
    private int _actualIndex;

    public void AreaReset()
    {
        foreach (GameObject obj in ListOfObjToResetInScene)
        {
            Destroy(obj);
        }
        ListOfObjToResetInScene.Clear();
        foreach (GameObject obj in _listOfPrefab)
        {
            GameObject go = Instantiate(_listOfPrefab[_actualIndex]);
            ListOfObjToResetInScene.Add(go);
            _actualIndex += 1;
        }
        _actualIndex = 0;
    }

    private void Awake()
    {
        _playerMovements= FindObjectOfType<PlayerMovements>();
        _testBabyWalk= FindObjectOfType<TestBabyWalk>();    
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
            if(PlayerHaveToBeReset)
            {
                // reset player
            }
            AreaReset();
        }
    }
    // si on a besoin de reset un atelier mais pas ceux déja fait, on avance l'index 
}