using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerNewControls playerControls;

    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }
    // Start is called before the first frame update
    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        playerControls = new PlayerNewControls();
       Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnEnable() => playerControls.Enable();

    public void OnDisable() => playerControls.Disable();

    public Vector2 GetPlayerMovement() => playerControls.Player.Movement.ReadValue<Vector2>();
    public Vector2 GetMouseDelta() => playerControls.Player.Look.ReadValue<Vector2>();

    public bool PlayerJumpedThisFrame() => playerControls.Player.Jump.triggered;

    public bool PlayerInteracted() => playerControls.Player.Interact.triggered;

    public bool PlayerStartThrow() => playerControls.Player.Shoot.triggered;

    public bool ReleaseThrow() => playerControls.Player.Shoot.WasReleasedThisFrame();

    public bool IsSprinting() => playerControls.Player.Sprint.IsPressed();

    public bool ReleaseSprint() => playerControls.Player.Sprint.WasReleasedThisFrame();
}
