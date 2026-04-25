using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
        hitCount++;
        animator.SetTrigger("Hit");
        hitUI.ShowHit();


        if (hitCount >= 3)
        {

            SpawnLoot();
            Destroy(gameObject);
        }
    }
    public void ChangeMesh(GameObject obj, Mesh newMesh)
    {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            meshFilter.mesh = newMesh;
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