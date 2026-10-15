using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Логика кнопок главного меню. Повесить на любой объект в сцене меню
/// (проще всего — на сам Canvas), и в инспекторе у каждой кнопки
/// (Button (Legacy) → секция On Click ()) подписать нужный публичный метод:
/// StartGame → PlayGame, Settings → OpenSettings, ExitGame → QuitGame.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Переход в игру")]
    [Tooltip("Точное имя игровой сцены. Сцена должна быть добавлена в " +
             "File → Build Settings → Scenes In Build, иначе LoadScene не найдёт её.")]
    public string gameSceneName = "Undertow";

    [Header("Настройки")]
    [Tooltip("Панель настроек внутри этой же сцены меню — просто включается/выключается, " +
             "отдельная сцена не нужна")]
    public GameObject settingsPanel;

    private void Awake()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    /// <summary>Кнопка "Играть".</summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>Кнопка "Настройки".</summary>
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    /// <summary>Кнопка "Назад"/"Закрыть" внутри самой панели настроек.</summary>
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    /// <summary>Кнопка "Выход".</summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        // В редакторе Application.Quit() ничего не делает — останавливаем Play Mode вручную,
        // чтобы кнопка работала одинаково и в билде, и при тесте в редакторе.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
