using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpeedUp : MonoBehaviour
{
    //public float VelocityMultiplier;
    [Header("Speed Max")]
    public float SpeedUpMax;
    public float SpeedUpSlopeMax;
    public float MaxSpeedToDecreasePerSec;
    [Header("Actual Speed")]
    public float VelocityPercent;
    public float ActualSpeedUp;
    public float ActualSpeedSlopeUp;

    private float _oldSpeedMax;
    private float _oldSpeedSlopeMax;

    private float _oldSpeed;
    private float _oldSpeedSlope;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            _oldSpeedMax = other.GetComponent<Slope>()._maxSpeed;
            _oldSpeedSlopeMax = other.GetComponent<Slope>()._maxSpeedSlope;

            _oldSpeed = other.GetComponent<Slope>().Speed;
            _oldSpeedSlope = other.GetComponent<Slope>()._speedSlope;

            other.GetComponent<Slope>()._maxSpeed = SpeedUpMax;
            other.GetComponent<Slope>()._maxSpeedSlope = SpeedUpSlopeMax;
            other.GetComponent<Slope>()._rigidbody.velocity += other.GetComponent<Slope>()._rigidbody.velocity * VelocityPercent;
            //other.GetComponent<Slope>()._rigidbody.velocity = other.GetComponent<Slope>()._rigidbody.velocity * VelocityMultiplier;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            other.GetComponent<Slope>().Speed += ActualSpeedUp * Time.deltaTime;
            other.GetComponent<Slope>()._speedSlope += ActualSpeedSlopeUp * Time.deltaTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            other.GetComponent<Slope>().OldSpeedMax = _oldSpeedMax;
            other.GetComponent<Slope>().OldSpeedSlopeMax = _oldSpeedSlopeMax;
            other.GetComponent<Slope>().SpeedToReduce = MaxSpeedToDecreasePerSec;

            other.GetComponent<Slope>().OldSpeed = _oldSpeed;
            other.GetComponent<Slope>().OldSpeedSlope = _oldSpeedSlope;
            other.GetComponent<Slope>().SpeedMaxCanDecrease = true;
        }
    }
}