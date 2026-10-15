using System.Collections;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform player;
    public PlayerController playerController;
    public SpriteRenderer playerSprite;
    public Animator playerAnimator;          // ← добавь

    [Header("Точки")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Стены, которые мешают входу")]
    public Collider2D[] wallsToDisableDuringIntro;

    [Header("Настройки")]
    public float walkSpeed = 1.6f;
    public float delayBeforeWalk = 0.5f;
    public bool faceRight = true;

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.inputLocked = true;

        // Отключаем мешающие стены
        if (wallsToDisableDuringIntro != null)
        {
            foreach (var col in wallsToDisableDuringIntro)
                if (col != null) col.enabled = false;
        }

        if (player != null && startPoint != null)
            player.position = startPoint.position;

        if (playerSprite != null)
            playerSprite.flipX = !faceRight;

        yield return new WaitForSeconds(delayBeforeWalk);

        // Включаем анимацию ходьбы
        if (playerAnimator != null)
            playerAnimator.SetFloat("Speed", 1f);

        if (player != null && endPoint != null)
        {
            while (Vector2.Distance(player.position, endPoint.position) > 0.05f)
            {
                player.position = Vector2.MoveTowards(
                    player.position,
                    endPoint.position,
                    walkSpeed * Time.deltaTime
                );
                yield return null;
            }
            player.position = endPoint.position;
        }

        // Останавливаем анимацию
        if (playerAnimator != null)
            playerAnimator.SetFloat("Speed", 0f);

        // Включаем стены обратно
        if (wallsToDisableDuringIntro != null)
        {
            foreach (var col in wallsToDisableDuringIntro)
                if (col != null) col.enabled = true;
        }

        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.inputLocked = false;
    }
}