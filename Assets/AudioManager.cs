using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public static AudioManager Instance;

    void Start()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayOneShot(AudioClip clip, float clipVolume = 0.2f)
    {
        audioSource.PlayOneShot(clip, clipVolume);
    }
}
