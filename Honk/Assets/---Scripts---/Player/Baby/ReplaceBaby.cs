using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Properties;

public class ReplaceBaby : MonoBehaviour
{
    [SerializeField] private RiftBaby _riftBaby;
    [SerializeField] private GameObject _jumpEndPosition;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private GameObject _babyMesh;


    private TestBabyWalk _baby;
    private PlayerMovements _playerMovements;
    public void ReplaceBabyInRift(PlayerMovements player, TestBabyWalk baby, GameObject targetInsideWall)
    {
        _baby = baby;
        _playerMovements = player;

        baby.CanCheckPlayerMovements = false;
        baby._isDadMoving = true;

        player.CanBabyFollowPlayerInput = false;

        baby.CanBabyInputs = false;
        //player.AnimatorHonkJR.SetBool("IsActive", false);
        //player.BabyFollowPlayer();
        player.AnimatorHonkJR.SetBool("OnBack", false);
        player.AnimatorHonkJR.SetBool("IsActive", true);
        player.AnimatorHonkJR.SetBool("IsMoving", true);

        //player.BabyUnfollowPlayer();

        _playerMovements.IconFollowHonk.SetActive(false);
        _playerMovements.IconFollowHonkJR.SetActive(false);
        //player.AnimatorHonkJR.SetTrigger("ChangingState");

        player.Direction = Vector3.zero;
        baby.GetComponent<CharacterController>().enabled = false;
        baby.gameObject.transform.DOLookAt(new Vector3(targetInsideWall.transform.position.x, targetInsideWall.transform.position.y, targetInsideWall.transform.position.z), 1, AxisConstraint.None, Vector3.zero);
        baby.gameObject.transform.DOMove(targetInsideWall.transform.position, 2, false).OnComplete(TeleportBaby);
    }

    void TeleportBaby()
    {
        _babyMesh.SetActive(false);
        _riftBaby.ScaleShroom(MoveBaby);
    }

    private void MoveBaby()
    {
        _baby.CanCheckPlayerMovements = true;
        //_playerMovements.AnimatorHonkJR.SetBool("IsMoving", false);

        //_playerMovements.AnimatorHonkJR.SetBool("IsActive", false);
        //_playerMovements.AnimatorHonkJR.SetTrigger("ChangingState");

        _playerMovements.CanBabyTeleport = true;
        _baby.GetComponent<CharacterController>().enabled = false;
        _baby.gameObject.transform.position = _riftBaby.PointTpInsideWall.transform.position;
        _playerMovements.CanBabyTeleport = false;
        _riftBaby._holdBaby.CanHoldBaby = false;
        _babyMesh.SetActive(true);
        _baby.gameObject.transform.DOJump(_jumpEndPosition.transform.position, _jumpForce, 1, _jumpDuration, false).OnComplete(JumpOutBaby);
    }

    void JumpOutBaby()
    {
        _playerMovements.AnimatorHonkJR.SetBool("IsActive", false);
        _playerMovements.AnimatorHonkJR.SetBool("IsActive", false);
        _playerMovements.AnimatorHonkJR.SetBool("IsMoving", false);
        _baby.GetComponent<CharacterController>().enabled = true;
        _playerMovements.CanPlayerUseInputs = true;
        _baby.CanBabyInputs = true;
        _playerMovements.CanBabyFollowPlayerInput = true;
    }
}