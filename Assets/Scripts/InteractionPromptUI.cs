using UnityEngine;
using TMPro;

/// <summary>
/// Простая подсказка "Нажмите E, чтобы...". Плавно появляется/исчезает через CanvasGroup.
/// Повесить на Canvas с TMP_Text; сюда же можно добавить иконку клавиши, если нужно.
/// </summary>
public class InteractionPromptUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TMP_Text label;
    public float fadeSpeed = 8f;

    private bool wantVisible;

    public void Show(string text)
    {
        label.text = text;
        wantVisible = true;
    }

    public void Hide()
    {
        wantVisible = false;
    }

    private void Update()
    {
        float target = wantVisible ? 1f : 0f;
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, fadeSpeed * Time.deltaTime);
    }
}
