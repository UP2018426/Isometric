using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Player Controls")]
    [Range(0.01f, 0.1f)]
    public float rotationSpeed = 0.06f;

    public float playerSpeed = 5.0f;
    private float jumpHeight = 1.5f; 
    [SerializeField] private float gravityValue = Physics.gravity.y;

    private bool isPlayerGrounded;

    public Transform cameraTransform;
    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference meleeAction;

    [Header("Melee Item")]
    [SerializeField] private Transform meleeItem;
    private MeleeObject melee;
    [SerializeField] private float meleeSpeed;
    private float meleeTime;

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
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        meleeAction.action.Disable();
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

        Vector3 targetDirection = Vector3.zero;

        if (move != Vector3.zero)
        {
            targetDirection = cameraTransform.TransformDirection(move); // Find look direction based on "move" in the local-space of cameraTransform
            targetDirection.y = 0f; // Ignore Y-Axis

            transform.forward = Vector3.Slerp(transform.forward, targetDirection, rotationSpeed);
        }

        // Jump
        if (jumpAction.action.triggered && isPlayerGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (targetDirection * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);

        if (meleeAction.action.triggered)
        {
            melee.OnAttackBegin();
        }
    }
}
