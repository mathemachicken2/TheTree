using UnityEngine;
using UnityEngine.InputSystem;

public class MeatPickup : MonoBehaviour
{
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.pickupSound);
        GameManager.Instance.ShowPrompt(false);
        GameManager.Instance.Pickup(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            GameManager.Instance.ShowPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            GameManager.Instance.ShowPrompt(false);
        }
    }
}