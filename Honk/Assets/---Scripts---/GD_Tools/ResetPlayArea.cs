using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayArea : MonoBehaviour
{
    public List<GameObject> ListOfObjToResetInScene = new List<GameObject>();
    [SerializeField] private List<GameObject> _listOfPrefab = new List<GameObject>();
    [SerializeField] private bool PlayerHaveToBeReset;
    [SerializeField] private KeyCode _keyBind;

    public List<Vector3> _listOfPositions = new List<Vector3>();
    private PlayerMovements _playerMovements;
    private TestBabyWalk _testBabyWalk;
    private int _actualIndex;

    public void AreaReset()
    {
        if (PlayerHaveToBeReset)
        {
            _playerMovements._testBabyWalk.transform.parent = _playerMovements.BabyParent.transform;
        }
        foreach (GameObject obj in ListOfObjToResetInScene)
        {
            //if (obj.GetComponentInChildren<TestBabyWalk>() != null)
            //{
            //    //GameObject baby = FindObjectOfType<TestBabyWalk>().gameObject; /*obj.GetComponentInChildren<TestBabyWalk>().gameObject;*/
            //    GameObject baby = obj.GetComponentInChildren<TestBabyWalk>().gameObject;
            //    //baby.transform.position = _playerMovements.TPBabyPos;
            //    baby.transform.position = new Vector3(0, 0, 0);
            //    baby.transform.parent = _playerMovements.BabyParent.gameObject.transform;

            //    //baby.transform.parent = obj.GetComponentInChildren<PushObstacles>().ParentBabyAfterDeath.transform;
            //    Debug.Log(baby.gameObject.transform.parent.name);
            //    Debug.Log(baby.name);
            //}
            //Destroy(obj);
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
            //obj.transform.position = _listOfPositions[_actualIndex];
            //Debug.Log(_testBabyWalk);
            //Debug.Log(_testBabyWalk.transform.parent.gameObject.name);
            _actualIndex++;
        }

        /// ESSAYER DE NE PAS DETRUIRE LES OBJETS MAIS DE RESET LEUR POSITIONS 
        //ListOfObjToResetInScene.Clear();
        //foreach (GameObject obj in _listOfPrefab)
        //{
        //    GameObject go = Instantiate(_listOfPrefab[_actualIndex]);
        //    ListOfObjToResetInScene.Add(go);
        //    _actualIndex += 1;
        //}

        _actualIndex = 0;
    }

    private void Awake()
    {
        _playerMovements = FindObjectOfType<PlayerMovements>();
        _testBabyWalk = FindObjectOfType<TestBabyWalk>();
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