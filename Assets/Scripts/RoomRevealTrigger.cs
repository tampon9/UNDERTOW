using System.Collections;
using UnityEngine;

/// <summary>
/// Ставится в проходе справа, после невидимой стены, которая снимается по
/// GameFlowManager.onGunRoomUnlocked (см. RoomTransitionTrigger). Когда игрок
/// физически доходит сюда — экран уходит в чёрное, комната с пистолетом
/// (изначально выключенный в сцене root-объект gunRoomRoot) активируется,
/// экран возвращается. Срабатывает один раз.
/// </summary>
public class RoomRevealTrigger : MonoBehaviour
{
    public ScreenFader fader;
    public GameObject gunRoomRoot; // изначально SetActive(false) в сцене

    [Header("Тайминги")]
    public float fadeOutTime = 0.8f;
    public float blackHoldTime = 0.4f;
    public float fadeInTime = 0.8f;

    private bool hasRevealed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasRevealed) return;
        if (!other.CompareTag("Player")) return;
        if (GameFlowManager.Instance == null || !GameFlowManager.Instance.gunRoomUnlocked) return;

        hasRevealed = true;
        StartCoroutine(RevealSequence());
    }

    private IEnumerator RevealSequence()
    {
        GameFlowManager.Instance.inputLocked = true;

        yield return fader.FadeOut(fadeOutTime);
        yield return new WaitForSeconds(blackHoldTime);

        gunRoomRoot.SetActive(true);

        yield return fader.FadeIn(fadeInTime);

        GameFlowManager.Instance.inputLocked = false;
    }
}
