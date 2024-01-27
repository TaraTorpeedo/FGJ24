using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public GameObject panel;

    public void TakeDamage(float damageRate)
    {
        panel.GetComponent<Image>().color += new Color(0, 0, 0, 0.1f);

        health -= damageRate;

        if(health == 0)
        {
            //DEAD
        }
    }

}
