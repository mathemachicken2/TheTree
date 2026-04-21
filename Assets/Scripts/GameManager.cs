using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject pressEText;
    public Transform holdPoint;
    public GameObject consumedPrefab;

    private GameObject heldObject;
    private Rigidbody heldRb;

    private bool isConsuming = false;

    public Image eatBloodOverlay;

    public ParticleSystem bloodEffect;

    private void Awake()
    {
        Instance = this;
        pressEText.SetActive(false);
    }

    private void Update()
    {
        if (heldObject != null && !isConsuming && Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartCoroutine(ConsumeRoutine());
        }
    }

    public void ShowPrompt(bool show)
    {
        if (pressEText != null)
            pressEText.SetActive(show);
    }

    public void Pickup(GameObject obj)
    {
        if (heldObject != null) return;

        heldObject = obj;
        heldRb = obj.GetComponent<Rigidbody>();

        if (heldRb != null)
        {
            heldRb.isKinematic = true;
            heldRb.detectCollisions = false;
        }

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    
    private IEnumerator ConsumeRoutine()
    {
        ParticleSystem bloodInstance = Instantiate(bloodEffect, heldObject.transform.position, Quaternion.identity);
        StartCoroutine(FadeRoutine());
        isConsuming = true;

        PlayerMovement.inputLocked = true;

        
        yield return new WaitForSeconds(1f);

        if (heldObject == null)
        {
            PlayerMovement.inputLocked = false;
            isConsuming = false;
            yield break;
        }

        Vector3 pos = heldObject.transform.position;
        Quaternion rot = heldObject.transform.rotation;

        Destroy(heldObject);
        

        GameObject spawned = null;

        if (consumedPrefab != null)
        {
            spawned = Instantiate(consumedPrefab, pos, rot);
        }

        heldObject = null;
        heldRb = null;

        
        yield return new WaitForSeconds(2f);

        if (spawned != null)
        {
            yield return new WaitForSeconds(1f);
            Destroy(spawned);
        }

        yield return new WaitForSeconds(1f);
        //  UNLOCK AFTER EVERYTHING IS DONE
        PlayerMovement.inputLocked = false;

        isConsuming = false;
        bloodInstance.gameObject.SetActive(false);
    }

    IEnumerator FadeRoutine()
    {
        float duration = 4f;
        float timer = 0f;

        Color color = eatBloodOverlay.color;

        // Fade IN
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / duration);
            eatBloodOverlay.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Fade OUT
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / duration);
            eatBloodOverlay.color = color;
            yield return null;
        }
    }
}