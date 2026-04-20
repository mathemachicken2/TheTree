using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitUIFeedback : MonoBehaviour
{
    public Image hitImage;
    private Coroutine currentRoutine;

    public void ShowHit()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        float duration = 0.2f;
        float timer = 0f;

        Color color = hitImage.color;

        // Fade IN
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / duration);
            hitImage.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Fade OUT
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / duration);
            hitImage.color = color;
            yield return null;
        }
    }
}