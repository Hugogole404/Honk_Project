using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    // Start is called before the first frame update

    public Material material;
    public string parameterName = "ColorChange";
    public float duration = 2.0f;

    private Coroutine lerpCoroutine;

    public void Start()
    {
        material = GetComponent<Material>();
    }
    public void StartLerp()
    {
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpShaderParameter());
    }

    private IEnumerator LerpShaderParameter()
    {
        float startTime = Time.time;
        float startValue = material.GetFloat(parameterName);
        float endValue = (startValue == 0.0f) ? 1.0f : 0.0f;

        while (Mathf.Abs(material.GetFloat(parameterName) - endValue) > 0.01f)
        {
            float t = (Time.time - startTime) / duration;
            t = Mathf.Clamp01(t);
            float lerpValue = Mathf.Lerp(startValue, endValue, t);
            material.SetFloat(parameterName, lerpValue);
            yield return null;
        }
        material.SetFloat(parameterName, endValue);
    }
}
