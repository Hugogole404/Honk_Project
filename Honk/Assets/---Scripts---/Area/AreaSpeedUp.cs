using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpeedUp : MonoBehaviour
{
    //public float VelocityMultiplier;
    public float SpeedUpMax;
    public float SpeedUpSlopeMax;
    public float SpeedToReducePerSec;
    private float _oldSpeedMax;
    private float _oldSpeedSlopeMax;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            _oldSpeedMax = other.GetComponent<Slope>()._maxSpeed;
            _oldSpeedSlopeMax = other.GetComponent<Slope>()._speedSlope;

            other.GetComponent<Slope>()._maxSpeed = SpeedUpMax;
            other.GetComponent<Slope>()._maxSpeed = SpeedUpSlopeMax;
            //other.GetComponent<Slope>()._rigidbody.velocity = other.GetComponent<Slope>()._rigidbody.velocity * VelocityMultiplier;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            other.GetComponent<Slope>().OldSpeed = _oldSpeedMax;
            other.GetComponent<Slope>().OldSpeedSlope = _oldSpeedSlopeMax;
            other.GetComponent<Slope>().SpeedToReduce = SpeedToReducePerSec;
            other.GetComponent<Slope>().SpeedMaxCanDecrease = true;
        }
    }
}