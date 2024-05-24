using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSounds : MonoBehaviour
{
    public Liste_sound _listSounds;
    [SerializeField] bool _walkInCave;
    [SerializeField] bool _walkInGrass;
    [SerializeField] bool _walkInSnow;
    [SerializeField] bool _walkInIce;
    [SerializeField] bool _walkInFlower;
    bool _playerIsDetected;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            _playerIsDetected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            _playerIsDetected = false;
        }
    }

    private void Update()
    {
        if (_playerIsDetected)
        {
            if (_walkInCave)
            {
                _listSounds.WalkInCave = true;
            }
            if (_walkInGrass)
            {
                _listSounds.WalkInGrass = true;
            }
            if (_walkInSnow)
            {
                _listSounds.WalkInSnow = true;
            }
            if (_walkInIce)
            {
                _listSounds.WalkInIce = true;
            }
            if (_walkInFlower)
            {
                _listSounds.WalkInflower = true;
            }
        }
        else
        {
            if (_walkInCave)
            {
                _listSounds.WalkInCave = false;
            }
            if (_walkInGrass)
            {
                _listSounds.WalkInGrass = false;
            }
            if (_walkInSnow)
            {
                _listSounds.WalkInSnow = false;
            }
            if (_walkInIce)
            {
                _listSounds.WalkInIce = false;
            }
            if (_walkInFlower)
            {
                _listSounds.WalkInflower = false;
            }
        }
    }
}