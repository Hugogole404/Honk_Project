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


    private TestBabyWalk _baby;
    private PlayerMovements _playerMovements;
    public void ReplaceBabyInRift(PlayerMovements player, TestBabyWalk baby, GameObject targetInsideWall)
    {
        _baby = baby;
        _playerMovements = player;
        //player.CanPlayerUseInputs = false;
        baby.CanBabyInputs = false;
        player.AnimatorHonkJR.SetBool("IsWalking", true);
        player.Direction = Vector3.zero;
        baby.GetComponent<CharacterController>().enabled = false;
        baby.gameObject.transform.DOLookAt(new Vector3(targetInsideWall.transform.position.x, targetInsideWall.transform.position.y, targetInsideWall.transform.position.z), 1, AxisConstraint.None, Vector3.zero);
        baby.gameObject.transform.DOMove(targetInsideWall.transform.position, 2, false).OnComplete(TeleportBaby);
    }

    void TeleportBaby()
    {
        _riftBaby.ScaleShroom(MoveBaby);
    }

    private void MoveBaby()
    {
        _playerMovements.AnimatorHonkJR.SetBool("IsWalking", false);
        _playerMovements.CanBabyTeleport = true;
        _baby.GetComponent<CharacterController>().enabled = false;
        _baby.gameObject.transform.position = _riftBaby.PointTpInsideWall.transform.position;
        _playerMovements.CanBabyTeleport = false;
        _riftBaby._holdBaby.CanHoldBaby = false;
        _baby.gameObject.transform.DOJump(_jumpEndPosition.transform.position, _jumpForce, 1, _jumpDuration, false).OnComplete(JumpOutBaby);
    }

    void JumpOutBaby()
    {
        _baby.GetComponent<CharacterController>().enabled = true;
        _playerMovements.CanPlayerUseInputs = true;
        _baby.CanBabyInputs = true;
    }
}