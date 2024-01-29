using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public GameObject panel;
    public GameManager manager;

    public void TakeDamage(float damageRate)
    {
        panel.GetComponent<Image>().color += new Color(0, 0, 0, 0.1f);

        health -= damageRate;

        if (health == 0)
        {
            manager.LoseGame();
        }
    }
}
