using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GoblinKing : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform Player;
    public string[] Jokes;
    public string[] JokesSecondPart;

    public AudioSource source;
    public AudioClip[] JokeClips;
    public AudioClip[] JokeSecondPartClips;

    public LayerMask WhatIsGround, WhatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public Transform[] walkPoints;

    //Joking
    public float timeBetweenJokes, timeBetweenSecondPart;
    bool alreadyJoked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public int JokeLengthDivider;

    public TextMeshProUGUI JokeText;


    private void Update()
    {

        float distance = Vector3.Distance(Player.transform.position, transform.position);

        Joking();

        if (distance < sightRange)
        {
            playerInSightRange = true;
        }
        else
        {
            playerInSightRange = false;
        }
        if (distance < attackRange)
        {
            playerInAttackRange = true;
        }
        else
        {
            playerInAttackRange = false;
        }


        if (!playerInSightRange && !playerInAttackRange)
        {
            agent.stoppingDistance = 1;
            Patrolling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            agent.stoppingDistance = 3;
            AttackPlayer();
        }

    }

    void Joking()
    {
        if (!alreadyJoked)
        {
            alreadyJoked = true;

            StartCoroutine(TellJoke());
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);    
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;   

        if(distanceToWalkPoint.magnitude <= agent.stoppingDistance)
        {
            walkPointSet = false;   
        }
    }

    void SearchWalkPoint()
    {
        int randomPoint = Random.Range(0, walkPoints.Length);

        walkPoint = new Vector3(walkPoints[randomPoint].position.x, transform.position.y, walkPoints[randomPoint].position.z);

        if(Physics.Raycast(walkPoint,-transform.up, 2f, WhatIsGround))
        {
            walkPointSet = true;    
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(Player.position);

        transform.LookAt(Player.position);

    }
    private void AttackPlayer()
    {
        //Only damage
    }

    IEnumerator TellJoke()
    {
        int rnd = Random.Range(0, Jokes.Length);

        Debug.Log(rnd);

        JokeText.text = Jokes[rnd];
        source.clip = JokeClips[rnd];
        source.Play();
        yield return new WaitForSeconds(Jokes[rnd].Length / JokeLengthDivider);

        JokeText.text = JokesSecondPart[rnd];
        source.clip = JokeSecondPartClips[rnd];
        source.Play();
        yield return new WaitForSeconds((JokesSecondPart[rnd].Length / JokeLengthDivider) + 1);


        JokeText.text = "";
        yield return new WaitForSeconds(1);
        JokeText.text = "";

        ResetJoke();
    }
    void ResetJoke()
    {
        alreadyJoked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
