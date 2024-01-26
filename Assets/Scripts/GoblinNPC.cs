using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinNPC : MonoBehaviour
{
    public NavMeshAgent agent;
    NavMeshPath path;

    public Transform GoblinKing;

    Vector3 EscapePoint;

    bool isWalking = false;

    private void Update()
    {
        if (!isWalking)
        {
            NewPoint();
        }

        float distanceToWalkPoint = Vector3.Distance(transform.position, EscapePoint);

        if (distanceToWalkPoint <= agent.stoppingDistance)
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

        path = new NavMeshPath();

        CalculateNewPoint();

    }

    void CalculateNewPoint()
    {
        if (NavMesh.CalculatePath(transform.position, EscapePoint, NavMesh.AllAreas, path))
        {
            isWalking = true;
            //move to target
            agent.SetPath(path);
            agent.SetDestination(EscapePoint);
        }

    }
}
