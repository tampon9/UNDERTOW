using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Титры: текст едет снизу вверх. Esc — пропустить, Пробел/Enter — ускорить.
/// Повесить на CreditsText (TMP с Content Size Fitter, Pivot (0.5, 1)).
/// </summary>
public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private RectTransform content;   // CreditsText
    [SerializeField] private float scrollSpeed = 80f; // пикселей в секунду
    [SerializeField] private float fastMultiplier = 4f;
    [SerializeField] private string nextSceneName = "MainMenu";

    private float screenHeight;
    private float endY;
    private bool finished;

    private void Start()
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        screenHeight = ((RectTransform)content.parent).rect.height;

        // старт: текст полностью под экраном, конец: полностью над экраном
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, -screenHeight);
        endY = content.rect.height;
    }

    private void Update()
    {
        if (finished) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Finish();
            return;
        }

        float speed = scrollSpeed;
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
            speed *= fastMultiplier;

        content.anchoredPosition += Vector2.up * speed * Time.unscaledDeltaTime;

        if (content.anchoredPosition.y >= endY)
            Finish();
    }

    private void Finish()
    {
        finished = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}
