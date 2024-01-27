using UnityEngine;

public class FoodObject : Interactable
{
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private GameObject food;

    [SerializeField]
    private float cooldown = 10f;

    protected override void Interact()
    {
        if(weapon.transform.childCount > 0)
        {
            Destroy(weapon.transform.GetChild(0).gameObject);
        }
        Instantiate(food, weapon.transform);
        gameObject.SetActive(false);
    }
}