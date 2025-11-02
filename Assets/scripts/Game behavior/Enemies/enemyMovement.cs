using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class enemyMovement : MonoBehaviour
{
    public Transform playerTransform;
    private NavMeshAgent agent;
    private bool isStopped = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isStopped)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isStopped)
        {
            StartCoroutine(StopAndResume());
        }
    }

    IEnumerator StopAndResume()
    {
        // Stop the enemy
        isStopped = true;
        agent.isStopped = true;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Resume movement
        isStopped = false;
        agent.isStopped = false;
    }
}