using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atelier02 : MonoBehaviour
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
            Camera.gameObject.SetActive(true);
            dolly.m_Position = 1f;
            StartPos = honkCollider.transform.position.x;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == honkCollider)
        {
            Camera.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        dolly.m_Position = NormalizeNumber(honkCollider.transform.position.x, StartPos, 180f, 0f, 1f);
    }
    public float NormalizeNumber(float value, float minOriginal, float maxOriginal, float minNew, float maxNew)
    {
        float normalizedValue = (value - minOriginal) / (maxOriginal - minOriginal);
        float newValue = minNew + normalizedValue * (maxNew - minNew);

        return newValue;
    }
}
