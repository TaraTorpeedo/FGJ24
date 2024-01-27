using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform throwPos;
    [SerializeField] private Vector3 throwDir = new Vector3(0,1,0);
    [SerializeField] private float cooldown;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float maxForce = 20f;
    [SerializeField] private AudioClip throwObjectSound;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject[] projectiles;

    private bool readyToThrow;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 3.0f;
    public float sprintSpeed = 10.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private InputManager inputManager;
    private Transform camTransform;
    private bool isCharging = false;
    private float chargeTime = 0f;


    public void Start()
    {
        inputManager = InputManager.Instance;
        camTransform = Camera.main.transform;
        readyToThrow = true;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = camTransform.forward * move.z + camTransform.right * move.x;
        move.y = 0f;

        if(inputManager.IsSprinting() && !inputManager.ReleaseSprint())
        {
            controller.Move(move * Time.deltaTime * sprintSpeed);
        }
        else
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
        }
  

        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

      
        if(inputManager.PlayerStartThrow() && readyToThrow)
        {
            StartThrowing();
        }
        if(isCharging)
        {
            ChargeThrow();
        }
        if (inputManager.ReleaseThrow())
        {
            ReleaseThrow();
        }
    }

    public void StartThrowing()
    {
        isCharging = true;
        chargeTime = 0f;

        var oldProjectile = GameObject.FindGameObjectWithTag("Food");

        if (!oldProjectile)
        {
            return;
        }
        lineRenderer.enabled = true;
    }

    public void ChargeThrow()
    {
        chargeTime += Time.deltaTime;

        Vector3 objectVelocity = (camTransform.forward + throwDir).normalized * Mathf.Min(chargeTime * throwForce, maxForce);
        CreateLine(throwPos.position + throwPos.forward, objectVelocity);
    }

    public void ReleaseThrow()
    {
        ThrowObject(Mathf.Min(chargeTime * throwForce, maxForce));
        isCharging = false;

        lineRenderer.enabled = false;
    }

    public void ThrowObject(float force)
    {
        readyToThrow = false;

        var oldProjectile = GameObject.FindGameObjectWithTag("Food");

        if (!oldProjectile)
        {
            Invoke(nameof(ResetThrow), cooldown);
            return;
        }

        GameObject projectile = null;
        var oldProjectileId = oldProjectile.GetComponent<FoodData>();

        switch (oldProjectileId.FoodID)
        {
            case 0:
                projectile = Instantiate(projectiles[0], throwPos.position, camTransform.rotation);
                break;
            case 1:
                projectile = Instantiate(projectiles[1], throwPos.position, camTransform.rotation);
                break;
            case 2:
                projectile = Instantiate(projectiles[2], throwPos.position, camTransform.rotation);
                break;
            case 3:
                projectile = Instantiate(projectiles[3], throwPos.position, camTransform.rotation);
                break;
        }

        Destroy(oldProjectile.gameObject);

        var forceDirection = camTransform.transform.forward;
        var finalThrowDir = (forceDirection + throwDir).normalized;
        var projectileRigitbody = projectile.GetComponent<Rigidbody>();
        projectileRigitbody.AddForce(finalThrowDir * force, ForceMode.VelocityChange);
        AudioManager.Instance.PlayOneShot(throwObjectSound, 0.5f);

        Invoke(nameof(ResetThrow), cooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void CreateLine(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];
        lineRenderer.positionCount = points.Length;

        for(int i = 0; i < points.Length; ++i)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + 0.5f * Physics.gravity * time * time;
        }

        lineRenderer.SetPositions(points);
    }
}
