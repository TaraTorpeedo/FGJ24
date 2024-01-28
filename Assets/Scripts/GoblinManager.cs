using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinManager : MonoBehaviour
{
    public GameObject[] Goblins;
    public int lifetime;
    public int lifetimeThreshold;

    public Transform SpawnPoint;
    public AudioClip[] LaughClips;
    public GameObject temp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LiveAndDie());
    }

    IEnumerator LiveAndDie()
    {

        int RandomGoblin = Random.Range(0, Goblins.Length);
        Goblins[RandomGoblin].GetComponent<GoblinNPC>().animator.SetBool("isRunning", false);

        int ReaLifeTime = lifetime + Random.Range(-lifetimeThreshold , lifetimeThreshold);
        yield return new WaitForSeconds(ReaLifeTime);

        Goblins[RandomGoblin].GetComponent<GoblinNPC>().agent.SetDestination(Goblins[RandomGoblin].transform.position);
        Goblins[RandomGoblin].GetComponent<GoblinNPC>().isWalking = true; //Stops walking

        Goblins[RandomGoblin].GetComponent<GoblinNPC>().animator.SetBool("laughAndDie", true);

        int laughRnd = Random.Range(0, LaughClips.Length);
        Goblins[RandomGoblin].GetComponent<GoblinNPC>().source.clip = LaughClips[laughRnd];
        Goblins[RandomGoblin].GetComponent<GoblinNPC>().source.Play();

        yield return new WaitForSeconds(LaughClips[laughRnd].length + 1); //Audio time

        Instantiate(temp, Goblins[RandomGoblin].gameObject.transform.position, Quaternion.identity);

        Goblins[RandomGoblin].SetActive(false);
        Goblins[RandomGoblin].transform.position = SpawnPoint.position;


        yield return new WaitForSeconds(1);
        Goblins[RandomGoblin].SetActive(true);

        Goblins[RandomGoblin].GetComponent<GoblinNPC>().isWalking = false; //Starts walking

        StartCoroutine(LiveAndDie());

    }

}
