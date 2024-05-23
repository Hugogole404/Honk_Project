using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTransform : MonoBehaviour
{
    [Header("Info")]
    private Vector3 _startPos;
    private Vector3 _randomPos;

    [Header("Settings")]
    [Range(0f, 2f)]
    public float _distance = 0.1f;
    [Range(0f, 0.1f)]
    public float _delayBetweenShakes = 0f;
    public GameObject Platform;
    public float OffsetY;
    public static ShakeTransform Instance;

    private void Awake()
    {
        Instance = this;
        _startPos = Platform.transform.position - new Vector3(0, OffsetY, 0);
    }

    public void Begin()
    {
        Platform.GetComponent<Platform>().Sound_plateform.Play();
        StopAllCoroutines();
        StartCoroutine(Shake());
        
    }

    public void Stop()
    {
        StopAllCoroutines();
    }


    private IEnumerator Shake()
    {
        while (true)
        {

            _randomPos = _startPos + (Random.insideUnitSphere * _distance);

            transform.position = _randomPos;

            if (_delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }
    }
}
