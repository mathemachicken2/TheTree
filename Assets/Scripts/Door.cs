using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("Input")]
    public InputAction interactAction;

    private bool playerInRange = false;
    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    public GameObject prompt;

    void OnEnable()
    {
        interactAction.Enable();
        
        interactAction.performed += OnInteract;
    }

    void OnDisable()
    {
        interactAction.performed -= OnInteract;
        interactAction.Disable();
    }

    void Start()
    {
        prompt.SetActive(false);
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (playerInRange)
        {
            isOpen = !isOpen;
        }
        SoundManager.Instance.PlaySound(SoundManager.Instance.Door);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
        prompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
        prompt.SetActive(false);
    }
}