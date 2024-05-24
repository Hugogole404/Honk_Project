using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class AreaDeathZone : MonoBehaviour
{
    private ResetPlayArea _resetArea;
    private PlayerMovements _playerMovements;
    private TestBabyWalk _testBabyWalk;
    private void Awake()
    {
        _resetArea = FindAnyObjectByType<ResetPlayArea>();
        _playerMovements = FindAnyObjectByType<PlayerMovements>();
        _testBabyWalk = FindAnyObjectByType<TestBabyWalk>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            other.transform.position = other.GetComponent<Slope>().SpawnPoint.transform.position;
        }
        if (other.GetComponent<PlayerMovements>() != null)
        {
            //other.GetComponent<CharacterController>().enabled = false;
            //other.transform.position = other.GetComponent<PlayerMovements>().SpawnPoint.transform.position;
            //other.transform.rotation = other.GetComponent<PlayerMovements>().PlayerOriginRotation;
            //other.GetComponent<CharacterController>().enabled = true;
            _playerMovements.TeleportToSpawnPoint();
            if (_resetArea.ListOfObjToResetInScene.Count > 0)
            {
                _resetArea.AreaReset();
            }
        }
        else if (other.GetComponent<TestBabyWalk>() != null)
        {
            //GameObject parent = FindAnyObjectByType<PlayerMovements>().gameObject;
            //parent.GetComponent<CharacterController>().enabled = false;
            //parent.transform.position = parent.GetComponent<PlayerMovements>().SpawnPoint.transform.position;
            //parent.transform.rotation = parent.GetComponent<PlayerMovements>().PlayerOriginRotation;
            //parent.GetComponent<CharacterController>().enabled = true;

            ////parent.GetComponent<PlayerMovements>()._inputsForBaby.SetActive(false);
            //parent.GetComponent<PlayerMovements>()._holdBaby.Baby.gameObject.transform.parent = gameObject.transform;
            //parent.GetComponent<PlayerMovements>()._holdBaby.Baby.gameObject.transform.position = new Vector3(parent.GetComponent<PlayerMovements>()._holdBaby.BasePositionBaby.transform.position.x,
            //    parent.GetComponent<PlayerMovements>()._holdBaby.BasePositionBaby.transform.position.y, parent.GetComponent<PlayerMovements>()._holdBaby.BasePositionBaby.transform.position.z);
            //parent.GetComponent<PlayerMovements>()._holdBaby.IsOnHisBack = true;
            //parent.GetComponent<PlayerMovements>()._holdBaby.Baby.GetComponent<Rigidbody>().isKinematic = true;
            //parent.GetComponent<PlayerMovements>()._holdBaby.CanHoldBaby = false;
            //parent.GetComponent<PlayerMovements>().CanBabyFollow = false;
            //parent.GetComponent<PlayerMovements>().AnimatorHonkJR.SetBool("OnBack", true);
            _playerMovements.TeleportToSpawnPoint();
            if (_resetArea.ListOfObjToResetInScene.Count > 0)
            {
                _resetArea.AreaReset();
            }
        }
    }
}