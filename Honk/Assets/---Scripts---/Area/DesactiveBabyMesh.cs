using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactiveBabyMesh : MonoBehaviour
{
    [SerializeField] private List<GameObject> _babyMeshList;
    [SerializeField] private LayerMask _lastLayer;
    [SerializeField] private LayerMask _newLayer;
    [SerializeField] bool _enter;
    [SerializeField] bool _exit;

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.GetComponent<TestBabyWalk>() != null)
        //{
            if(_enter)
            {
                DesactiveMesh();
            }
            else if (_exit)
            {
                ActiveMesh();
            }
        //}
    }

    void DesactiveMesh()
    {
        foreach(GameObject go in _babyMeshList)
        {
            go.layer = _newLayer;
        }
        print("DESACTIVE");
    }   
    void ActiveMesh()
    {
        foreach(GameObject go in _babyMeshList)
        {
            go.layer = _lastLayer;
        }
        print("ACTIVE");
    }
}
