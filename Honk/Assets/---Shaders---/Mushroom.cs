using UnityEngine;

[ExecuteInEditMode]

public class Mushroom : MonoBehaviour
{
    [SerializeField] private Material material;

    private string positionProperty = "_SpherePosition";
    // could add [SerializeField] attribute or make public to set from inspector

    void Update()
    {
        material.SetVector(positionProperty, transform.position);
        /*
        Could also combine these into a single Vector4 property, e.g.
        Vector4 vector = transform.position;
        vector.w = transform.localScale.x;
        material.SetVector(combinedProperty, vector);
        */
    }
}
