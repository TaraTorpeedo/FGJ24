using UnityEngine;

public class FoodObject : Interactable
{
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private GameObject food;
    [SerializeField]
    private int id;

    [SerializeField]
    private float cooldown = 10f;

    public int FoodID => id;

    protected override void Interact()
    {
        if(weapon.transform.childCount > 0)
        {

            Destroy(weapon.transform.GetChild(0).gameObject);
        }
        Instantiate(food, weapon.transform);
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if(!gameObject.activeInHierarchy)
        {
            cooldown -= Time.deltaTime;

            if(cooldown <= 0)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
