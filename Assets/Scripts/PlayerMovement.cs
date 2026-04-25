using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float gravity = -9.81f;

    [Header("Look")]
    public float mouseSensitivity = 2f;
    public Transform cameraPivot;

    CharacterController controller;

    Vector2 moveInput;
    Vector2 lookInput;

    float xRotation;
    float yVelocity;

    // INPUT ACTIONS
    InputAction moveAction;
    InputAction lookAction;

    public static bool inputLocked;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // CREATE INPUT ACTIONS
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        lookAction = new InputAction("Look", InputActionType.Value);
        lookAction.AddBinding("<Mouse>/delta");
    }

    void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    void Update()
    {
        if (inputLocked)
            return;
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        Move();
        Look();
    }

    void Move()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (controller.isGrounded && yVelocity < 0)
            yVelocity = -2f;

        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;

        controller.Move(move * speed * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}