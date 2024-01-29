using Unity.VisualScripting;
using UnityEngine;

public class FoodObject : Interactable
{
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private GameObject food;
    [SerializeField]
    private float cooldown = 10f;
    [SerializeField]
    private GameObject foodModel;

    protected override void Interact()
    {
        if(weapon.transform.childCount > 0)
        {
            Destroy(weapon.transform.GetChild(0).gameObject);
        }
        Instantiate(food, weapon.transform);
        foodModel.SetActive(false);
    }

    public void Update()
    {
        if (!foodModel.activeInHierarchy)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                foodModel.SetActive(true);
                cooldown = 10f;
            }
        }
    }
}