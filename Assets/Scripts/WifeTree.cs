using UnityEngine;

public class WifeTree : MonoBehaviour
{
    public GameObject replacementPrefab;

    private bool playerInRange = false;

    public Transform focusPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            GameManager.Instance.ShowFeedPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            GameManager.Instance.ShowFeedPrompt(false);
        }
    }

    public bool CanInteract(GameObject heldItem)
    {
        return playerInRange && heldItem != null;
    }

    public void ReplaceTree()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        Instantiate(replacementPrefab, pos, rot);
        Destroy(gameObject);
        GameManager.Instance.ShowFeedPrompt(false);
    }
}