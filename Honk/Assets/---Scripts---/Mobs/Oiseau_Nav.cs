using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation.Editor;
using UnityEngine;
using UnityEngine.AI;

public class Oiseau_Nav : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    private NavMeshAgent agent;
    public Animator m_animator;
    public int contactBefFly;
    public int contactNumber = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(CheckTransform());
        contactBefFly = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CheckTransform()
    {
        
        yield return new WaitForSeconds(0.1f);
        
        if (Vector3.Distance(agent.transform.position, player.position) <= 4 && contactNumber <= contactBefFly)
        {
            Vector3 dirToPlayer = transform.position - player.position;
            Vector3 newPos = transform.position + dirToPlayer * 7;
            agent.destination = newPos;
            //StartCoroutine(JustRan());
            //StopCoroutine(CheckTransform());
            m_animator.SetBool("IsMoving", true);
            contactNumber++;
        }
        else if (Vector3.Distance(agent.transform.position, player.position) <= 4 && contactNumber > contactBefFly)
        {
            m_animator.SetTrigger("IsFlying");
        }
        else
        {
            agent.destination = agent.transform.position;
            m_animator.SetBool("IsMoving", false);
        }


        StartCoroutine(CheckTransform());
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
