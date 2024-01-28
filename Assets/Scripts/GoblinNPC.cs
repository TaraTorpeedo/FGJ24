using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinNPC : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform GoblinKing;

    Vector3 EscapePoint;

    public bool isWalking = false;

    public AudioSource source;

    public Animator animator;

    Vector3 lastPos;

    private void Update()
    {

        if(transform.hasChanged)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (!isWalking)
        {
            NewPoint();
        }

        float distanceToWalkPoint = Vector3.Distance(transform.position, EscapePoint);

        if (distanceToWalkPoint <= agent.stoppingDistance+1)
        {
            isWalking = false;
        }
    }

    void NewPoint()
    {
        int randomX = Random.Range(5, 30);
        int randomZ = Random.Range(5, 30);

        if (transform.position.x < GoblinKing.position.x)
        {
            EscapePoint.x = GoblinKing.position.x - randomX;
        }
        else
        {
            EscapePoint.x = GoblinKing.position.x + randomX;
        }

        if (transform.position.z < GoblinKing.position.z)
        {
            EscapePoint.z = GoblinKing.position.z - randomZ;
        }
        else
        {
            EscapePoint.z = GoblinKing.position.z + randomZ;
        }

        CalculateNewPoint();

    }

    void CalculateNewPoint()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(EscapePoint, out hit, 0.1f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            agent.SetDestination(EscapePoint);
            isWalking = true; 
        }

    }
}
