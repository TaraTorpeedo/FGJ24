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

    public AudioClip[] LaughClips;
    public AudioClip[] YellClips;
    [SerializeField] private AudioDetection audioDetector;
    [SerializeField] private float loudnessSensibility = 100;
    [SerializeField] private float threshold = 0.1f;
    bool isYelling;
    bool ableToYell;

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

    int jokeBefore = 0;

    bool canAttack = true;

    void MicCatch()
    {
        float loudness = audioDetector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness >= threshold)
        {
            if (!isYelling && ableToYell)
            {   
                StartCoroutine(YellToPlayer());
            }
        }
        else
        {
            loudness = 0;
        }
    }


    private void Update()
    {

        Debug.Log(agent.speed);
        MicCatch();
        float distance = Vector3.Distance(Player.transform.position, transform.position);

        if (!isYelling)
        {
            ableToYell = true;
            Joking();
        }

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

        if (!isYelling)
        {
            if (!playerInSightRange && !playerInAttackRange)
            {
                agent.stoppingDistance = 1;
                Patrolling();
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer(3.5f);
            }
            if (playerInAttackRange && playerInSightRange)
            {
                ableToYell = false;
                agent.stoppingDistance = 3;

                if (canAttack)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
        }
        else
        {
            ChasePlayer(10f);
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
    private void ChasePlayer(float speed)
    {
        ableToYell = false;
        agent.speed = speed;
        agent.SetDestination(Player.position);
        transform.LookAt(Player.position);

    }
    private IEnumerator AttackPlayer()
    {
        Debug.Log("Attack");
        canAttack = false;
        yield return new WaitForSeconds(1);
        Player.GetComponent<PlayerHealth>().TakeDamage(1);
        canAttack = true;
    }

    IEnumerator TellJoke()
    {
        Debug.Log(isYelling);
        int rnd = Random.Range(0, Jokes.Length);
        if(rnd == jokeBefore)
        {
            if(rnd < Jokes.Length)
            {
                rnd += 1;
            }
            else
            {
                rnd -= 1;
            }
        }

        jokeBefore = rnd;

        int laughRandom = Random.Range(0, 4);

        source.pitch = 0.9f;
        source.volume = 1f;

        JokeText.text = Jokes[rnd];
        source.clip = JokeClips[rnd];
        source.Play();
        yield return new WaitForSeconds(Jokes[rnd].Length / JokeLengthDivider);

        JokeText.text = JokesSecondPart[rnd];
        source.clip = JokeSecondPartClips[rnd];
        source.Play();

        if (isYelling)
        {
            ResetJoke();
            yield break;
        }

        //Laugh
        if (laughRandom == 1)
        {
            yield return new WaitForSeconds((JokesSecondPart[rnd].Length / JokeLengthDivider));
            JokeText.text = "";
            source.pitch = 0.8f;
            source.volume = 0.8f;

            int laughClipRnd = Random.Range(0, LaughClips.Length);        

            source.clip = LaughClips[laughClipRnd];
            source.Play();

            if(isYelling)
            {
                ResetJoke();
                yield break;
            }

            yield return new WaitForSeconds(source.clip.length);
        }
        else
        {
            if (isYelling)
            {
                ResetJoke();
                yield break;
            }
            yield return new WaitForSeconds((JokesSecondPart[rnd].Length / JokeLengthDivider) + 1);
        }

        if (isYelling)
        {
            ResetJoke();
            yield break;
        }
        JokeText.text = "";
        yield return new WaitForSeconds(1);
        JokeText.text = "";

        ResetJoke();
    }
    void ResetJoke()
    {
        alreadyJoked = false;
    }

    public IEnumerator YellToPlayer()
    {
        isYelling = true;

        source.Stop();
        JokeText.text = "";

        source.pitch = 1f;
        int rnd = Random.Range(0,YellClips.Length);
        source.clip = YellClips[rnd];
        source.Play();

        yield return new WaitForSeconds(YellClips[rnd].length + 0.5f);

        isYelling = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
