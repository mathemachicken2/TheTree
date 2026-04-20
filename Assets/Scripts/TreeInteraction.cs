using UnityEngine;
using UnityEngine.InputSystem;

public class TreeInteractionTrigger : MonoBehaviour
{
    private Animator animator;
    private bool playerInRange = false;

    private Vector3 lockedPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        lockedPosition = transform.position; // save original position
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Hit");
        }
    }

    void LateUpdate()
    {
        transform.position = lockedPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}