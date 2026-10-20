using System.Collections;
using UnityEngine;

/// <summary>
/// Выстрел -> падение ровно на позицию, зеркальную озеру комнаты 1 -> пиксельная
/// анимация растекания в силуэт этого озера -> появление настоящего 3D-озера ->
/// катсцена. Никакого fade-to-black и скрытой подмены объектов — переход собран
/// как последовательность видимых, непрерывных шагов.
/// </summary>
public class DeathAndRebirthSequence : MonoBehaviour
{
    [Header("Персонаж")]
    public Animator playerAnimator;
    public string shootTriggerName = "Shoot";
    public GameObject playerVisual;

    [Tooltip("Пустой Transform — позиция, зеркальная озеру комнаты 1, куда падает тело")]
    public Transform lakeSpotMirror;

    [Header("Пиксельная анимация")]
    public PixelDissolveEffect dissolveEffect;

    [Header("Настоящее озеро (изначально выключено в сцене)")]
    public GameObject realLakeObject;

    [Header("Катсцена")]
    public EndingCutscene endingCutscene;

    [Header("Тайминги")]
    public float fallAnimationDuration = 1.2f;

    private bool playing;

    public void Play()
    {
        // Защита от двойного запуска: пистолет может вызвать и напрямую, и через событие onGunFired
        if (playing) return;
        playing = true;
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        GameFlowManager.Instance.inputLocked = true;

        if (playerAnimator != null)
            playerAnimator.SetTrigger(shootTriggerName);

        yield return new WaitForSeconds(fallAnimationDuration);

        // Тело помещаем точно на позицию, зеркальную озеру, и прячем 3D-модель —
        // дальше историю рассказывает только пиксельная анимация.
        if (playerVisual != null)
        {
            playerVisual.transform.position = lakeSpotMirror.position;
            playerVisual.SetActive(false);
        }

        bool dissolveDone = false;
        void OnDone() => dissolveDone = true;
        dissolveEffect.OnDissolveFinished += OnDone;

        dissolveEffect.transform.position = lakeSpotMirror.position;
        dissolveEffect.Play();

        while (!dissolveDone) yield return null;
        dissolveEffect.OnDissolveFinished -= OnDone;

        realLakeObject.SetActive(true);
        dissolveEffect.gameObject.SetActive(false);

        endingCutscene.Play();
    }
}
