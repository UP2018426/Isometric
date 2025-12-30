using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Player Controls")]
    [Range(0.01f, 0.1f)]
    public float rotationSpeed = 0.06f;

    public float playerSpeed = 5.0f;
    //private float jumpHeight = 1.5f; 
    [SerializeField] private float gravityValue = Physics.gravity.y;

    private bool isPlayerGrounded;

    public Transform cameraTransform;
    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Player Stats")]
    [SerializeField] private int currentHealth;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference meleeAction;
    public InputActionReference focusAction;

    [Header("Melee Item")]
    [SerializeField] private Transform meleeItem;
    private MeleeObject melee;
    [SerializeField] private float meleeSpeed;

    [Header("Targeting")] 
    private GameObject[] enemies;

    public Transform targetObject;

    private void Awake()
    {
        // Will try to find a CharacterController if one already exists on the object, otherwise it'll create a default one.
        if (TryGetComponent(out CharacterController characterController))
        {
            controller = characterController;
        }
        else
        {
            Debug.LogWarning("A Character Controller component was not found on this object. A default Character Controller was added");
            controller = gameObject.AddComponent<CharacterController>();
        }
        
        // Find all enemies
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (meleeItem != null)
        {
            if (meleeItem.TryGetComponent(out MeleeObject meleeObject))
            {
                melee = meleeObject;
            }
            else
            {
                Debug.LogWarning("Your MeleeItem on " + transform.name + " does not contain a MeleeObject component. One has been automatically added");

                melee = meleeItem.AddComponent<MeleeObject>();
            }
        }
    }

    private void OnEnable() // This is new input system stuffs
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        meleeAction.action.Enable();
        focusAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        meleeAction.action.Disable();
        focusAction.action.Disable();
    }

    void Update()
    {
        isPlayerGrounded = controller.isGrounded;
        if (isPlayerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Read input
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        Vector3 moveDirection = cameraTransform.TransformDirection(move);
        moveDirection.y = 0f; // Ignore Y-Axis
        moveDirection.Normalize();

        if (focusAction.action.WasPressedThisFrame())
        {
            targetObject = FindNearestEnemy();
        }
        else if (focusAction.action.WasReleasedThisFrame())
        {
            targetObject = null;
        }

        if(focusAction.action.inProgress && targetObject == null)
        {
            targetObject = FindNearestEnemy();
        }

        if (focusAction.action.inProgress && targetObject != null)
        {
            if (targetObject == null)
            {
                targetObject = FindNearestEnemy();
            }

            Vector3 lookDirection = targetObject.position - transform.position;
            lookDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }
        else if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }

        // Jump
        //if (jumpAction.action.triggered && isPlayerGrounded)
        //{
        //    playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        //}

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (moveDirection * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);

        if (meleeAction.action.triggered)
        {
            melee.OnAttackBegin();
        }
    }

    Transform FindNearestEnemy()
    {
        // Find all enemies
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        if (enemies.Length <= 0)
        {
            Debug.LogWarning("There are no enemies!");
            return null;
        }
        float closestDistance = Mathf.Infinity;
        Transform closestTransform = null;

        foreach (GameObject enemy in enemies)
        {
            if ((enemy.transform.position - transform.position).magnitude < closestDistance)
            {
                closestDistance = (enemy.transform.position - transform.position).magnitude;
                closestTransform = enemy.transform;
            }
        }

        return closestTransform;
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        // Destory the player in here, reset the level, or whatever...
        Debug.Log("Player Died!");
    }
}
