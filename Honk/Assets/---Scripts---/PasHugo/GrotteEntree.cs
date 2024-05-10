using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrotteEntree : MonoBehaviour
{
    public Collider honkCollider;
    public CinemachineVirtualCamera Camera;
    public CinemachineDollyCart dolly;
    private float StartPos;

    // Start is called before the first frame update

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == honkCollider)
        {
            Camera.enabled = true;
            dolly.m_Position = 1f;
            StartPos = honkCollider.transform.position.z;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == honkCollider)
        {
            Camera.enabled = false;
        }
    }
    private void Update()
    {
        dolly.m_Position = 1f - NormalizeNumber(honkCollider.transform.position.z, StartPos, 169f, 0f, 1f);
    }
    public float NormalizeNumber(float value, float minOriginal, float maxOriginal, float minNew, float maxNew)
    {
        float normalizedValue = (value - minOriginal) / (maxOriginal - minOriginal);
        float newValue = minNew + normalizedValue * (maxNew - minNew);

        return newValue;
    }
}
