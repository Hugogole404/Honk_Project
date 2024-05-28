using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lapin_Nav : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Animator m_animator;
    private NavMeshAgent agent;
    public bool stopchecking = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(CheckTransform());
    }

    IEnumerator CheckTransform()
    {
        yield return new WaitForSeconds(0.05f);
        if (Vector3.Distance(agent.transform.position, player.position) <= 30 || stopchecking == true)
        {
            if (Vector3.Distance(agent.transform.position, player.position) <= 2)
            {
                Vector3 dirToPlayer = transform.position - player.position;
                Vector3 newPos = transform.position + dirToPlayer * 4;
                agent.destination = newPos;
                //StartCoroutine(JustRan());
                //StopCoroutine(CheckTransform());
                m_animator.SetBool("IsMoving", true);
            }
            else if (Vector3.Distance(agent.transform.position, player.position) >= 3)
            {
                agent.destination = player.position;
                m_animator.SetBool("IsMoving", true);
            }
            else
            {
                agent.destination = agent.transform.position;
                m_animator.SetBool("IsMoving", false);
            }
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
