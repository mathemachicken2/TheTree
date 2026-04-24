using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject pressEText;
    public GameObject pressFeedText;
    public Transform holdPoint;
    public GameObject consumedPrefab;

    private GameObject heldObject;
    private Rigidbody heldRb;

    private bool isConsuming = false;

    public Image eatBloodOverlay;
    public Image redFade;
    public GameObject messageText;

    public ParticleSystem bloodEffect;

    public Transform playerCamera;
    public float zoomDuration = 1f;

    private bool inTreeRange = false;
    private WifeTree currentTree;

    public Image blackImage;

    public GameObject introText;
    

    private void Awake()
    {
        Instance = this;
        pressEText.SetActive(false);
        pressFeedText.SetActive(false);
        messageText.SetActive(false);
        StartCoroutine(IntroSequence());
    }

    private void Update()
    {
        if (heldObject != null && !isConsuming && Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartCoroutine(ConsumeRoutine());
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryUseTree();
        }

    }
    IEnumerator IntroSequence()
    {
        PlayerMovement.inputLocked = true;

        Color color = blackImage.color;
        color.a = 1f;
        blackImage.color = color;

        introText.SetActive(true);

        TMP_Text textComponent = introText.GetComponent<TMP_Text>();
        textComponent.text = "";

        string[] lines = new string[]
        {
        "The world is turning to flesh.",
        "Your wife is a tree.",
        "Rescue her by feeding her a chunk of meat."
        };

        // Show lines one by one
        for (int i = 0; i < lines.Length; i++)
        {
            textComponent.text += lines[i] + "\n";
            yield return new WaitForSeconds(1f);
        }

        // Wait before fade
        yield return new WaitForSeconds(2f);

        // Fade out
        float duration = 2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            t = t * t * (3f - 2f * t); // smoothstep

            color.a = Mathf.Lerp(1f, 0f, t);
            blackImage.color = color;

            yield return null;
        }

        introText.SetActive(false);

        PlayerMovement.inputLocked = false;
    }
    IEnumerator FocusOnTree(Transform target)
    {
        PlayerMovement.inputLocked = true;

        Vector3 startPos = playerCamera.position;
        Quaternion startRot = playerCamera.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float time = 0f;

        while (time < zoomDuration)
        {
            time += Time.deltaTime;
            float t = time / zoomDuration;

            playerCamera.position = Vector3.Lerp(startPos, endPos, t);
            playerCamera.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }
    }
    void TryUseTree()
    {
        if (heldObject == null) return;

        WifeTree tree = FindClosestWifeTree();
        if (tree == null) return;

        if (!tree.CanInteract(heldObject)) return;

        tree.ReplaceTree();
        Debug.Log(currentTree);
        StartCoroutine(FocusOnTree(tree.focusPoint));
        // StartCoroutine(TreeSequence(currentTree));
       
        StartCoroutine(FadeToRed(2f));

        Destroy(heldObject);
        heldObject = null;
        heldRb = null;
    }
    IEnumerator FadeToRed(float duration)
    {
        yield return new WaitForSeconds(2f);
        float timer = 0f;
        Color color = redFade.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / duration);
            redFade.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        if (messageText != null)
            messageText.SetActive(true);

    }
    IEnumerator TreeSequence(WifeTree tree)
    {
        yield return StartCoroutine(FocusOnTree(tree.focusPoint));

        // optional pause for dramatic effect
        yield return new WaitForSeconds(0.5f);

        //tree.ReplaceTree();

        //Destroy(heldObject);
        heldObject = null;
        heldRb = null;

        yield return new WaitForSeconds(1f);

        PlayerMovement.inputLocked = false;
    }
    WifeTree FindClosestWifeTree()
    {
        return FindObjectOfType<WifeTree>();
    }

    public void ShowPrompt(bool show)
    {
        if (pressEText != null)
            pressEText.SetActive(show);
    }
    public void ShowFeedPrompt(bool show)
    {
        if (pressFeedText != null)
            pressFeedText.SetActive(show);
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