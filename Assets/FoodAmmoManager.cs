using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodAmmoManager : MonoBehaviour
{
    [SerializeField]
    private float cooldown = 10f;
    private FoodObject[] foodObjects;

    public void Start()
    {
        foodObjects = FindObjectsOfType<FoodObject>();
    }

    void Update()
    {
        foreach (var foodObject in foodObjects)
        {
            if (!foodObject.gameObject.activeInHierarchy)
            {
                cooldown -= Time.deltaTime;

                if (cooldown <= 0)
                {
                    foodObject.gameObject.SetActive(true);
                    cooldown = 10f;
                }
            }
        }
    }
}