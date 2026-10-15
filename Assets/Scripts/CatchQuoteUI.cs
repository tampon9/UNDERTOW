using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Окно с иконкой + фразой после улова.
/// Иерархия:
/// CatchQuoteWindow (CanvasGroup)
///  ├─ Paper (Image)
///  ├─ Icon  (Image)  ← iconImage
///  └─ PhraseText (TMP_Text) ← phraseText
/// </summary>
public class CatchQuoteUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image iconImage;
    public TMP_Text phraseText;

    public float fadeInTime = 0.6f;
    public float holdTime = 2.8f;
    public float fadeOutTime = 1.0f;

    private Coroutine current;

    public void ShowCatch(CatchEntry entry)
    {
        if (entry == null) return;
        if (canvasGroup == null)
        {
            Debug.LogError("[CatchQuoteUI] canvasGroup не назначен!");
            return;
        }
        if (phraseText == null)
        {
            Debug.LogError("[CatchQuoteUI] phraseText не назначен!");
            return;
        }

        if (current != null) StopCoroutine(current);
        current = StartCoroutine(ShowRoutine(entry));
    }

    private IEnumerator ShowRoutine(CatchEntry entry)
    {
        if (iconImage != null)
        {
            iconImage.sprite = entry.silhouette;
            iconImage.enabled = entry.silhouette != null;
        }

        phraseText.text = entry.phrase ?? "";

        yield return Fade(canvasGroup.alpha, 1f, fadeInTime);
        yield return new WaitForSeconds(holdTime);
        yield return Fade(1f, 0f, fadeOutTime);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (canvasGroup == null) yield break;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}