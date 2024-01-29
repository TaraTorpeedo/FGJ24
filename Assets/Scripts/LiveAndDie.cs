using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveAndDie : MonoBehaviour
{
    [SerializeField] AudioSource audio;

    void Start()
    {
        audio.Play();
        StartCoroutine(DieAndRespawn());
    }


    IEnumerator DieAndRespawn()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
