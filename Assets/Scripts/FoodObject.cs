using UnityEngine;

public class FoodObject : Interactable
{
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private GameObject food;

    protected override void Interact()
    {
        Instantiate(food, weapon.transform);
        Destroy(this.gameObject);
    }
}
