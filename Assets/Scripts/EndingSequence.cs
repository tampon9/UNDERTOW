using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Концовка: после последней записки — пауза, затемнение, сброс прогресса, загрузка титров.
/// Повесить на пустой объект EndingSequence в сцене Game.
/// </summary>
public class EndingSequence : MonoBehaviour
{
    public ScreenFader fader;
    public float delayAfterLastNote = 3f;   // пауза после того, как записка исчезла
    public float fadeDuration = 2f;
    public float holdBlack = 0.5f;
    public string creditsSceneName = "Credits";

    private bool started;

    public void StartEnding(float noteDuration)
    {
        if (started) return;
        started = true;
        StartCoroutine(Run(noteDuration));
    }

    private IEnumerator Run(float noteDuration)
    {
        // игрок больше не должен двигаться и забрасывать удочку
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.inputLocked = true;

        // ждём, пока записка покажется и исчезнет, плюс пауза
        yield return new WaitForSeconds(noteDuration + delayAfterLastNote);

        yield return fader.FadeOut(fadeDuration);
        yield return new WaitForSeconds(holdBlack);

        // GameFlowManager живёт между сценами — сбрасываем, чтобы новая игра началась с 0/20
        if (GameFlowManager.Instance != null)
            Destroy(GameFlowManager.Instance.gameObject);

        Time.timeScale = 1f;
        SceneManager.LoadScene(creditsSceneName);
    }
}
