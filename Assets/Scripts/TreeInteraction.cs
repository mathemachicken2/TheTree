using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class TreeInteractionTrigger : MonoBehaviour
{
    private Animator animator;
    private bool playerInRange = false;

    private int hitCount = 0;

    [Header("Loot Prefabs")]
    public GameObject meat1;
    public GameObject meat2;
    public GameObject meat3;

    private Vector3 lockedPosition;
    public HitUIFeedback hitUI;

    void Start()
    {
        animator = GetComponent<Animator>();
        lockedPosition = transform.position;
       
    }

    void Update()
    {
        transform.position = lockedPosition;
        if (!playerInRange) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HitTree();
        }
    }
    void LateUpdate()
    {
        transform.position = lockedPosition;
    }

    void HitTree()
    {
        hitCount++;
        animator.SetTrigger("Hit");
        hitUI.ShowHit();

        if (hitCount >= 3)
        {
            SpawnLoot();
            Destroy(gameObject);
        }
    }

    void SpawnLoot()
    {
        Vector3 spawnPos = transform.position;

        Instantiate(meat1, spawnPos + new Vector3(1, 3, 0), Quaternion.identity);
        Instantiate(meat2, spawnPos + new Vector3(-1, 1, 0), Quaternion.identity);
        Instantiate(meat3, spawnPos + new Vector3(0, 2, 1), Quaternion.identity);
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