using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpeedUp : MonoBehaviour
{
    public float VelocityMultiplier;
    public float SpeedUp;
    public float SpeedUpSlope;
    private float _oldSpeed;
    private float _oldSpeedSlope;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            _oldSpeed = other.GetComponent<Slope>().Speed;
            _oldSpeedSlope = other.GetComponent<Slope>()._speedSlope;
            other.GetComponent<Slope>().Speed = SpeedUp;
            other.GetComponent<Slope>().Speed = SpeedUpSlope;
            other.GetComponent<Slope>()._rigidbody.velocity = other.GetComponent<Slope>()._rigidbody.velocity * VelocityMultiplier;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            other.GetComponent<Slope>().Speed = _oldSpeed;
            other.GetComponent<Slope>()._speedSlope = _oldSpeedSlope;
        }
    }
}