using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveAndDie : MonoBehaviour
{
    public GameObject Goblin;
    public Transform SpawnPoint;

    void Start()
    {
        StartCoroutine(DieAndRespawn());
    }


    IEnumerator DieAndRespawn()
    {
        int random = Random.Range(5, 20);

        yield return new WaitForSeconds(random);

        //Do animation
        yield return new WaitForSeconds(2); //Animation time
        Goblin.SetActive(false);

        transform.position = SpawnPoint.position;

        yield return new WaitForSeconds(2);
        Goblin.SetActive(true);


        StartCoroutine(DieAndRespawn());
    }
}
