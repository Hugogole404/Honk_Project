using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerCheckStates : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    public GameObject Sphere;

    public void SlopeToWalk()
    {
        _player.GetComponent<PlayerMovements>().enabled = true;
        _player.GetComponent<PlayerSlides>().enabled = true;
        _player.GetComponent<PlayerSwim>().enabled = true;
        _player.GetComponent<CharacterController>().enabled = true;
    }
    public void WalkToSlope()
    {
        _player.GetComponent<PlayerMovements>().enabled = false;
        _player.GetComponent<PlayerSlides>().enabled = false;
        _player.GetComponent<PlayerSwim>().enabled = false;
        _player.GetComponent<CharacterController>().enabled = false;
    }

    void Update()
    {
        if (_player.GetComponent<PlayerMovements>().enabled == false && Sphere.activeInHierarchy == false)
        {
            SlopeToWalk();
        }
    }
}