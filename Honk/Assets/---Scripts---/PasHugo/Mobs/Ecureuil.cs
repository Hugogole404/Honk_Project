using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Ecureuil : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Animator m_animator;
    private NavMeshAgent agent;
    private GameObject NextStop;
    public GameObject Stop1;
    public GameObject Stop2;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        agent.destination = Stop1.transform.position;
    }

    //IEnumerator JustRan()
    //{
    //    StopCoroutine(CheckTransform());
    //    StartCoroutine(CheckAlentours());
    //    yield return new WaitForSeconds(3f);
    //    StartCoroutine(CheckTransform());
    //    StopCoroutine(CheckAlentours());
    //}
}
