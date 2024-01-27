using Cinemachine;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private PlayerUI playerUI;
    private InputManager inputManager;

    public void Start()
    {
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
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
}
