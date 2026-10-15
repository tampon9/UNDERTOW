using UnityEngine;

/// <summary>
/// Переключает Animator.IsFishing вместо прямой подмены SpriteRenderer.sprite —
/// теперь это делает Animator Controller (IdlePlayer / WalkPlayer / IdleFishingPlayer).
/// Телепорт к точке рыбалки оставлен как было.
/// </summary>
public class PlayerFishingSprite2D : MonoBehaviour
{
    [Header("Ссылки")]
    public Animator animator;
    public FishingManager fishingManager;

    [Header("Точка рыбалки (у берега)")]
    public Transform fishingStandPoint;

    private Transform playerTransform;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerTransform = transform;
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (fishingManager != null)
            fishingManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (fishingManager != null)
            fishingManager.OnStateChanged -= HandleStateChanged;
    }

    private void Start()
    {
        if (fishingManager != null)
            HandleStateChanged(fishingManager.State);
    }

    private void HandleStateChanged(FishingState state)
    {
        bool isFishing = state != FishingState.Idle;
        animator?.SetBool("IsFishing", isFishing);

        if (isFishing && fishingStandPoint != null)
        {
            playerTransform.position = fishingStandPoint.position;
            if (playerRb != null)
                playerRb.linearVelocity = Vector2.zero;
        }
    }
}