using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Минималистичный показ фразы: fade-in → пауза → fade-out.
/// ShowLine мгновенно гасит предыдущий показ перед стартом нового — без этого
/// быстрый повторный вызов визуально "доливал" альфу поверх уже идущего фейда,
/// из-за чего казалось, что текст завис дольше положенного.
/// </summary>
public class DialogueTextUI : MonoBehaviour
{
    public TMP_Text label;
    public CanvasGroup canvasGroup;

    public float fadeInTime = 0.6f;
    public float holdTime = 2.5f;
    public float fadeOutTime = 1.0f;

    public bool IsShowing { get; private set; }

    private Coroutine current;

    public void ShowLine(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        if (current != null) StopCoroutine(current);
        if (canvasGroup != null) canvasGroup.alpha = 0f; // чистый старт, без наложения

        current = StartCoroutine(ShowRoutine(text));
    }

    private IEnumerator ShowRoutine(string text)
    {
        IsShowing = true;
        label.text = text;
        yield return Fade(0f, 1f, fadeInTime);
        yield return new WaitForSeconds(holdTime);
        yield return Fade(1f, 0f, fadeOutTime);
        IsShowing = false;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
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