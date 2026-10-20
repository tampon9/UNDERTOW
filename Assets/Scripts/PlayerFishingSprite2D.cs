using UnityEngine;

/// <summary>
/// Переключает Animator.IsFishing (анимации ведёт Animator Controller)
/// и переносит игрока к точке рыбалки.
/// </summary>
public class PlayerFishingSprite2D : MonoBehaviour
{
    private static readonly int IsFishingHash = Animator.StringToHash("IsFishing");

    [Header("Ссылки")]
    public Animator animator;
    public FishingManager fishingManager;

    [Header("Точка рыбалки (у берега)")]
    public Transform fishingStandPoint;

    private Rigidbody2D playerRb;
    private bool wasFishing;

    private void Awake()
    {
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

        if (animator != null)
            animator.SetBool(IsFishingHash, isFishing);

        // Телепорт только в момент начала рыбалки, а не при каждой смене состояния.
        if (isFishing && !wasFishing && fishingStandPoint != null)
            TeleportTo(fishingStandPoint.position);

        wasFishing = isFishing;
    }

    private void TeleportTo(Vector2 position)
    {
        if (playerRb != null)
        {
            // Пишем в Rigidbody, иначе MovePosition в PlayerController вернёт игрока назад.
            playerRb.position = position;
            playerRb.linearVelocity = Vector2.zero;
        }
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}