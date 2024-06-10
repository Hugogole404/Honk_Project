using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Oiseau_Nav : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Animator m_animator;
    public int contactBefFly;
    public int contactNumber = 0;
    [Range(0.0f, 10.0f)] public float distance = 1f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(CheckTransform());
        contactBefFly = Random.Range(1, 4);
    }

    IEnumerator CheckTransform()
    {
        
        yield return new WaitForSeconds(0.1f);
        if (Vector3.Distance(agent.transform.position, player.position) <= 100)
        {
            if (Vector3.Distance(agent.transform.position, player.position) <= 4 && contactNumber <= contactBefFly)
            {
                Vector3 dirToPlayer = transform.position - player.position;
                Vector3 newPos = transform.position + dirToPlayer;
                newPos += dirToPlayer;
                newPos += dirToPlayer;
                newPos += dirToPlayer;
                newPos += dirToPlayer;
                agent.destination = newPos;
                //StartCoroutine(JustRan());
                //StopCoroutine(CheckTransform());
                m_animator.SetBool("IsMoving", true);
                contactNumber++;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(CheckTransform());
                yield break;
            }
            else if (Vector3.Distance(agent.transform.position, player.position) <= 4 && contactNumber > contactBefFly)
            {
                m_animator.SetTrigger("IsFlying");
                yield return new WaitForSeconds(2f);
                Destroy(gameObject);
            }
            else
            {
                int doImove = Random.Range(1, 10);
                if (doImove == 1)
                {
                    agent.destination = agent.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
                    m_animator.SetBool("IsMoving", true);
                    yield return new WaitForSeconds(0.3f);
                    StartCoroutine(CheckTransform());
                    yield break;
                }
                else
                {
                    agent.destination = agent.transform.position;
                    m_animator.SetBool("IsMoving", false);
                }
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
