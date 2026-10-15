using System.Collections;
using UnityEngine;

/// <summary>
/// Полноэкранное затемнение поверх всего. Используется для переходов, где нужен
/// честный "экран чёрный -> что-то происходит -> экран возвращается", в отличие от
/// пиксельной анимации растекания (та решается покадрово, без этого компонента).
/// Повесить на Canvas (Render Mode: Screen Space - Overlay, Sort Order выше остального UI):
/// дочерний Image на весь экран, чёрный, на родителе — CanvasGroup с alpha = 0.
/// </summary>
public class ScreenFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        SetAlpha(0f);
    }

    public IEnumerator FadeOut(float duration)
    {
        yield return Fade(canvasGroup.alpha, 1f, duration);
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return Fade(canvasGroup.alpha, 0f, duration);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        canvasGroup.blocksRaycasts = true;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(from, to, duration <= 0f ? 1f : t / duration));
            yield return null;
        }
        SetAlpha(to);
        if (to <= 0f) canvasGroup.blocksRaycasts = false;
    }

    private void SetAlpha(float a) => canvasGroup.alpha = a;
}
