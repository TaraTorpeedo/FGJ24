using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GoblinKing : MonoBehaviour
{
    [SerializeField] private int health = 0;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform Player;
    [SerializeField] private string[] Jokes;
    [SerializeField] private string[] JokesSecondPart;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] JokeClips;
    [SerializeField] private AudioClip[] JokeSecondPartClips;
    [SerializeField] private AudioClip[] LaughClips;
    [SerializeField] private AudioClip[] YellClips;
    [SerializeField] private AudioDetection audioDetector;
    [SerializeField] private float loudnessSensibility = 100;
    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private InfoSettings settings;
    [SerializeField] private GameManager manager;
    [SerializeField] private LayerMask WhatIsPlayer;
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private Transform[] walkPoints;
    [SerializeField] private float timeBetweenJokes, timeBetweenSecondPart;
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;
    [SerializeField] private int JokeLengthDivider;
    [SerializeField] private TextMeshProUGUI JokeText;
    [SerializeField] private Animator animator;

    private bool isYelling;
    private bool ableToYell;
    private bool canAttack = true;
    private LayerMask WhatIsGround = 3;
    private int jokeBefore = 0;
    private bool alreadyJoked;
    private bool walkPointSet;


    private void MicCatch()
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

    protected void Update()
    {
        if(transform.hasChanged)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        if (settings.IsMicApproved())
        {
            MicCatch();
        }
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
                ChasePlayer(2f);
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
            ChasePlayer(5f);
        }

    }

    private void Joking()
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

    private void SearchWalkPoint()
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
        canAttack = false;
        yield return new WaitForSeconds(1);
        Player.GetComponent<PlayerHealth>().TakeDamage(1);
        canAttack = true;
    }

    private IEnumerator TellJoke()
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
    private void ResetJoke()
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

    public void TakeFood(int damage)
    {
        health += damage;
        hpSlider.value = health;
        if(health >= 60)
        {
            manager.WinGame();
        }
    }
}
