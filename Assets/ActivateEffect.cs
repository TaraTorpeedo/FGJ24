using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEffect : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void OnEnable()
    {
        particleSystem.Play();
    }
}
