using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class enemyMovement : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player
    private NavMeshAgent agent;       // Controls pathfinding
    public bool isStopped = false;   // Whether the enemy is stopped
    


    // Rotation speed for facing the player
    public float rotationSpeed = 5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        if (!isStopped && playerTransform != null)
        {
            // Move toward the player
            agent.SetDestination(playerTransform.position);

            // Make the enemy face the player smoothly
            FacePlayer();
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
        // Stop the enemy movement
        isStopped = true;
        agent.isStopped = true;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Resume movement
        isStopped = false;
        agent.isStopped = false;
    }

    // Makes the enemy smoothly rotate to face the player
    void FacePlayer()
    {
        // Direction to the player (ignore y-axis to prevent tilting up/down)
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            // Compute the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate toward the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
