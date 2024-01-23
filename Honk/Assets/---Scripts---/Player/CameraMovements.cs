using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://app.milanote.com/1Rsc3R1SJGPM9g/cameramovement?p=5Aw4gcZ0pqp")]
public class CameraMovements : MonoBehaviour
{
    [SerializeField] private float _offsetY, offsetZ;
    private PlayerMovements Player;

    private void Start()
    {
        Player = FindAnyObjectByType<PlayerMovements>();
    }
    private void Update()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + _offsetY, Player.transform.position.z - offsetZ);
    }
}