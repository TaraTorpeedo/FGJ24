using Cinemachine;
using Unity.Mathematics;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private PlayerUI playerUI;
    private InputManager inputManager;

    public void Start() => inputManager = InputManager.Instance;

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        var ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.gameObject.GetComponent<Interactable>() != null)
            {
                var interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);

                if(inputManager.PlayerInteracted())
                {
                    interactable.BaseInteract();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Interactable>())
        {
            var interactable = other.gameObject.GetComponent<Interactable>();
            interactable.BaseInteract();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
